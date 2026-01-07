using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using System.Collections;

public class RadioControlador : MonoBehaviour
{
    public EnemySpawner spawner;
    public bool eliminarEnemigosAlReparar = true;

    [Header("Configuración de Reparación")]
    public float tiempoNecesario = 3f;
    private float tiempoActual = 0f;
    public bool estaArreglada = true;

    [Header("Configuración de Averías")]
    public float tiempoMinParaRomper = 5f;
    public float tiempoMaxParaRomper = 10f;

    [Header("Referencias de Acción")]
    public InputActionReference accionReparar;

    [Header("Audio")]
    public AudioSource ruidoBlanco;
    public AudioSource musicaLimpia;

    private XRGrabInteractable grabPadre; // Referencia al grab del padre
    private bool estaSiendoAgarrada = false;
    private bool pulsandoBotonReparar = false;
    private bool tocandoConDedo = false;

    void Awake()
    {
        if (accionReparar != null) accionReparar.action.Enable();

        // Buscamos el Grab Interactable específicamente en el objeto de arriba (el padre)
        // Usamos transform.parent para ir directamente a la "Radio"
        if (transform.parent != null)
        {
            grabPadre = transform.parent.GetComponent<XRGrabInteractable>();
        }

        // Si no lo encuentra por jerarquía directa, lo buscamos en los ancestros
        if (grabPadre == null)
        {
            grabPadre = GetComponentInParent<XRGrabInteractable>();
        }

        // DEBUG: Para estar seguros de qué objeto estamos escuchando
        if (grabPadre != null)
            Debug.Log("Escuchando el agarre de: " + grabPadre.gameObject.name);
    }

    void Start()
    {
        StartCoroutine(CicloDeAverias());
    }

    IEnumerator CicloDeAverias()
    {
        while (true)
        {
            yield return new WaitUntil(() => estaArreglada);
            yield return new WaitForSeconds(Random.Range(tiempoMinParaRomper, tiempoMaxParaRomper));
            if (estaArreglada) EstropearRadio();
        }
    }

    public void EstropearRadio()
    {
        estaArreglada = false;
        tiempoActual = 0f;
        ruidoBlanco.volume = 1f;
        musicaLimpia.volume = 0f;

        if (spawner)
        {
            spawner.SetSpawning(true); // empieza el spawn continuo
            spawner.SpawnNow();        // spawnea 1 inmediatamente
        }

        Debug.Log("Radio estropeada.");
    }

    void OnEnable()
    {
        if (accionReparar != null)
        {
            accionReparar.action.performed += AlPulsarBoton;
            accionReparar.action.canceled += AlSoltarBoton;
        }

        // Nos suscribimos a los eventos del PADRE
        if (grabPadre != null)
        {
            grabPadre.selectEntered.AddListener(AlSerAgarrada);
            grabPadre.selectExited.AddListener(AlSerSoltada);
        }
    }

    void OnDisable()
    {
        if (accionReparar != null)
        {
            accionReparar.action.performed -= AlPulsarBoton;
            accionReparar.action.canceled -= AlSoltarBoton;
        }

        if (grabPadre != null)
        {
            grabPadre.selectEntered.RemoveListener(AlSerAgarrada);
            grabPadre.selectExited.RemoveListener(AlSerSoltada);
        }
    }

    private void AlSerAgarrada(SelectEnterEventArgs args) => estaSiendoAgarrada = true;
    private void AlSerSoltada(SelectExitEventArgs args)
    {
        estaSiendoAgarrada = false;
        ReiniciarProgreso();
    }

    private void AlPulsarBoton(InputAction.CallbackContext context) => pulsandoBotonReparar = true;
    private void AlSoltarBoton(InputAction.CallbackContext context) => pulsandoBotonReparar = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hand")) tocandoConDedo = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hand")) tocandoConDedo = false;
    }

    void Update()
    {
        if (estaArreglada) return;

        // Ahora detectamos si el padre está agarrado Y pulsamos el botón
        bool intentandoRepararConBoton = estaSiendoAgarrada && pulsandoBotonReparar;
        bool intentandoRepararConDedo = tocandoConDedo;

        if (intentandoRepararConBoton || intentandoRepararConDedo)
        {
            // Mantenemos el test de reparación instantánea para el botón
            if (intentandoRepararConBoton)
            {
                ArreglarRadio();
            }
            else
            {
                tiempoActual += Time.deltaTime;
                float progreso = Mathf.Clamp01(tiempoActual / tiempoNecesario);
                ruidoBlanco.volume = 1f - progreso;
                musicaLimpia.volume = progreso;

                if (tiempoActual >= tiempoNecesario) ArreglarRadio();
            }
        }
        else
        {
            ReiniciarProgreso();
        }
    }

    void ReiniciarProgreso()
    {
        tiempoActual = 0f;
        if (!estaArreglada)
        {
            ruidoBlanco.volume = 1f;
            musicaLimpia.volume = 0f;
        }
    }

    void ArreglarRadio()
    {
        estaArreglada = true;
        ruidoBlanco.volume = 0f;
        musicaLimpia.volume = 1f;
        tiempoActual = 0f;

        if (spawner)
        {
            spawner.SetSpawning(false);
            if (eliminarEnemigosAlReparar) spawner.DespawnAll();
        }

        Debug.Log("✅ Radio arreglada con éxito.");
    }
}