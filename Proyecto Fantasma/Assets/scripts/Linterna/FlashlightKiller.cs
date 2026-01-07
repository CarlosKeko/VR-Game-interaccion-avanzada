using UnityEngine;

public class FlashlightKiller : MonoBehaviour
{
    [Header("Haz de la linterna")]
    public Transform origenHaz;
    public float rango = 8f;
    [Range(1f, 179f)] public float angulo = 25f;
    public LayerMask capaEnemigos;
    public LayerMask capaObstaculos;

    [Header("Tiempo de exposición")]
    public float tiempoParaMatar = 2f; // segundos enfocando para morir

    [Header("Audio mientras mata")]
    public AudioSource audioMatar; // AudioSource en loop (no Play On Awake)

    // acumulador por enemigo (para que no sea instantáneo)
    private readonly System.Collections.Generic.Dictionary<EnemyFlashlightKill, float> tiempoVisto
        = new System.Collections.Generic.Dictionary<EnemyFlashlightKill, float>();

    void Reset()
    {
        origenHaz = transform;
    }

    void Update()
    {
        if (!origenHaz) return;

        bool estaMatandoEsteFrame = false;

        Collider[] hits = Physics.OverlapSphere(origenHaz.position, rango, capaEnemigos, QueryTriggerInteraction.Ignore);

        var vistosEsteFrame = new System.Collections.Generic.HashSet<EnemyFlashlightKill>();

        foreach (var col in hits)
        {
            var enemy = col.GetComponentInParent<EnemyFlashlightKill>();
            if (!enemy || !enemy.gameObject.activeInHierarchy) continue;

            Vector3 dir = (enemy.transform.position - origenHaz.position);
            float dist = dir.magnitude;
            if (dist <= 0.001f) continue;

            dir /= dist;

            float a = Vector3.Angle(origenHaz.forward, dir);
            if (a > angulo) continue;

            // Si hay un obstáculo entre la linterna y el enemigo, no cuenta
            if (Physics.Raycast(origenHaz.position, dir, out RaycastHit hit, dist, capaObstaculos, QueryTriggerInteraction.Ignore))
                continue;

            // Si llegamos aquí, el enemigo está iluminado (estamos “matando”)
            estaMatandoEsteFrame = true;

            vistosEsteFrame.Add(enemy);

            if (!tiempoVisto.ContainsKey(enemy))
                tiempoVisto[enemy] = 0f;

            tiempoVisto[enemy] += Time.deltaTime;

            if (tiempoVisto[enemy] >= tiempoParaMatar)
            {
                enemy.Morir();
                tiempoVisto.Remove(enemy);
            }
        }

        // Limpiar enemigos que ya no están iluminados
        var keys = new System.Collections.Generic.List<EnemyFlashlightKill>(tiempoVisto.Keys);
        foreach (var e in keys)
        {
            if (!vistosEsteFrame.Contains(e))
                tiempoVisto.Remove(e);
        }

        // Control de audio (play/stop)
        if (audioMatar != null)
        {
            if (estaMatandoEsteFrame && !audioMatar.isPlaying) audioMatar.Play();
            if (!estaMatandoEsteFrame && audioMatar.isPlaying) audioMatar.Stop();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!origenHaz) return;
        Gizmos.DrawWireSphere(origenHaz.position, rango);
    }
}
