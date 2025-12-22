using UnityEngine;
using System.Collections; // Necesario para las Corrutinas

public class RadioReparacionSimple : MonoBehaviour
{
    [Header("Configuración de Reparación")]
    public float tiempoNecesario = 3f;
    private float tiempoActual = 0f;
    public bool estaArreglada = true; // Empezamos con la radio bien
    private bool tocandoDial = false;

    [Header("Configuración de Averías")]
    public float tiempoMinParaRomper = 10f; // Mínimo 10 segundos funcionando
    public float tiempoMaxParaRomper = 30f; // Máximo 30 segundos funcionando

    [Header("Audio")]
    public AudioSource ruidoBlanco;
    public AudioSource musicaLimpia;

    void Start()
    {
        // Iniciamos la radio en estado normal
        musicaLimpia.volume = 1f;
        ruidoBlanco.volume = 0f;
        musicaLimpia.Play();
        ruidoBlanco.Play();

        // Lanzamos el ciclo de averías aleatorias
        StartCoroutine(CicloDeAverias());
    }

    IEnumerator CicloDeAverias()
    {
        while (true) // Bucle infinito para que pase durante toda la partida
        {
            // 1. Esperar a que la radio esté arreglada
            yield return new WaitUntil(() => estaArreglada);

            // 2. Esperar un tiempo aleatorio antes de que se estropee
            float esperaAleatoria = Random.Range(tiempoMinParaRomper, tiempoMaxParaRomper);
            yield return new WaitForSeconds(esperaAleatoria);

            // 3. ¡ESTROPEAR LA RADIO!
            EstropearRadio();
        }
    }

    void EstropearRadio()
    {
        estaArreglada = false;
        tiempoActual = 0f;
        ruidoBlanco.volume = 1f;
        musicaLimpia.volume = 0f;
        Debug.Log("⚠️ ¡La radio se ha estropeado!");
    }

    void Update()
    {
        if (!estaArreglada && tocandoDial)
        {
            tiempoActual += Time.deltaTime;
            float progreso = tiempoActual / tiempoNecesario;

            // Feedback de audio: a medida que reparas, vuelve la música
            ruidoBlanco.volume = 1f - progreso;
            musicaLimpia.volume = progreso;

            if (tiempoActual >= tiempoNecesario)
            {
                ArreglarRadio();
            }
        }
        else if (!estaArreglada && !tocandoDial)
        {
            // Si sueltas, vuelve a sonar solo el ruido
            ruidoBlanco.volume = 1f;
            musicaLimpia.volume = 0f;
        }
    }

    private void ArreglarRadio()
    {
        estaArreglada = true;
        ruidoBlanco.volume = 0f;
        musicaLimpia.volume = 1f;
        Debug.Log("✅ ¡Radio arreglada!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hand")) tocandoDial = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hand")) tocandoDial = false;
    }
}