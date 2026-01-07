using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    // Boolean para dejar el enemigo de referencia quieto mientras hacemos el desarrollo
    public bool controladoPorSpawner = false;

    public Transform objetivo;
    public float velocidad = 1.0f;
    public float distanciaStop = 0.6f;

    [Header("Rotación")]
    public float velocidadRotacion = 10f; // suavidad al girar (más alto = gira más rápido)
    public float yawOffset = -90f;          // si el modelo mira “de lado”, prueba 90 o -90

    void Update()
    {
        if (!controladoPorSpawner) return;
        if (!objetivo) return;

        Vector3 to = objetivo.position - transform.position;
        to.y = 0f;

        float d = to.magnitude;
        if (d < 0.001f) return;

        Vector3 dir = to / d;

        // ROTAR hacia el jugador SIEMPRE (aunque ya esté cerca y no se mueva)
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion objetivoRot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, yawOffset, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, objetivoRot, velocidadRotacion * Time.deltaTime);
        }

        // Mover solo si está lejos
        if (d <= distanciaStop) return;

        transform.position += dir * velocidad * Time.deltaTime;
    }
}
