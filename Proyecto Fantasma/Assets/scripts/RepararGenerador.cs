using UnityEngine;

public class RepararGenerador : MonoBehaviour
{
    [Header("Configuración")]
    public Light repairLight;
    public Color colorReparado = Color.green;
    public float tiempoNecesario = 3.0f;

    [Header("Obstáculo")]
    public GameObject wall;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoReparando;
    public AudioClip sonidoExito;

    [Header("Guía de Salida")]
    public GameObject[] lucesGuia; // El vector de objetos LampLights

    public GameObject exitPoint; //Area donde el jugador saldra del juego

    private float timer = 0f;
    private bool estaPresionando = false;
    private bool yaReparado = false;
    private bool jugadorEnArea = false; // Nueva variable de control

    void Update()
    {
        // Solo progresa si presiona Y está dentro del área
        if (estaPresionando && jugadorEnArea && !yaReparado)
        {
            timer += Time.deltaTime;

            if (audioSource != null && sonidoReparando != null && !audioSource.isPlaying)
            {
                audioSource.clip = sonidoReparando;
                audioSource.loop = true;
                audioSource.volume = 10f;
                audioSource.Play();
            }

            if (timer >= tiempoNecesario)
            {
                CompletarReparacion();
            }
        }
        else
        {
            if (audioSource != null && audioSource.clip == sonidoReparando)
            {
                audioSource.Stop();
            }
        }
    }

    // Detectar entrada al área
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnArea = true;
            Debug.Log("Jugador cerca del generador");
        }
    }

    // Detectar salida del área
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnArea = false;
            estaPresionando = false; // Detenemos la reparación si se aleja
            timer = 0f;
            Debug.Log("Jugador se ha alejado");
        }
    }

    public void IniciarReparacion()
    {
        if (!yaReparado && jugadorEnArea) estaPresionando = true;
    }

    public void DetenerReparacion()
    {
        estaPresionando = false;
        if (!yaReparado) timer = 0f;
    }

    private void CompletarReparacion()
    {
        yaReparado = true;
        estaPresionando = false;
        if (repairLight != null) repairLight.color = colorReparado;
        
        if (wall != null) wall.SetActive(false);

        ActivarLuces();

        exitPoint.SetActive(true); // Activa el punto de salida

        if (audioSource != null)
        {
            audioSource.Stop();
            if (sonidoExito != null) audioSource.PlayOneShot(sonidoExito);
        }
    }

    void ActivarLuces()
    {
        if (lucesGuia.Length == 0) return;

        foreach (GameObject luz in lucesGuia)
        {
            if (luz != null)
            {
                luz.SetActive(true); // Enciende la luz
            }
        }
        Debug.Log("Luces de salida activadas");
    }
}