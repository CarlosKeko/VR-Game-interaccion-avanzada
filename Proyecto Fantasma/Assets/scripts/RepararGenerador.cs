using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RepararGenerador : MonoBehaviour
{
    [Header("Configuración")]
    public Light repairLight;
    public Color colorReparado = Color.green;
    public float tiempoNecesario = 3.0f; // Segundos que hay que mantener
    public GameObject Wall;

    [Header("Audio")]
    public AudioSource audioSource; // Arrastra aquí un componente AudioSource
    public AudioClip sonidoReparando; // El sonido de chispas/motor
    public AudioClip sonidoExito;    // El sonido de "clinc" al terminar

    private float timer = 0f;
    private bool estaPresionando = false;
    private bool yaReparado = false;

    void Update()
    {
        if (estaPresionando && !yaReparado)
        {
            timer += Time.deltaTime;

            // Si tienes el AudioSource y el sonido, que suene mientras reparas
            if (audioSource != null && sonidoReparando != null && !audioSource.isPlaying)
            {
                audioSource.clip = sonidoReparando;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (timer >= tiempoNecesario)
            {
                CompletarReparacion();
            }
        }
        else
        {
            // Si deja de presionar antes de tiempo, detenemos el sonido
            if (audioSource != null && audioSource.clip == sonidoReparando)
            {
                audioSource.Stop();
            }
        }
    }

    public void IniciarReparacion()
    {
        if (!yaReparado) estaPresionando = true;
    }

    public void DetenerReparacion()
    {
        estaPresionando = false;
        if (!yaReparado) timer = 0f; // Opcional: reiniciar el progreso si suelta
    }

    private void CompletarReparacion()
    {
        yaReparado = true;
        estaPresionando = false;

        audioSource.clip = sonidoExito;
        audioSource.loop = false;

        if (repairLight != null) repairLight.color = colorReparado;

        if (audioSource != null)
        {
            audioSource.Stop(); // Para el sonido de reparación
            if (sonidoExito != null) audioSource.Play(); // Suena el éxito
        }

        Wall.SetActive(false);

        Debug.Log("¡Reparación Completa!");
    }
}