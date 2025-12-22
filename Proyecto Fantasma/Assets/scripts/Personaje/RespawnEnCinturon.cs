using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RespawnEnCinturon : MonoBehaviour
{
    public Transform puntoCinturon;   // SlotLinterna
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectExited.AddListener(CuandoSeSuelta);
    }

    void OnDisable()
    {
        grab.selectExited.RemoveListener(CuandoSeSuelta);
    }

    void CuandoSeSuelta(SelectExitEventArgs args)
    {
        // Reposicionar
        transform.position = puntoCinturon.position;
        transform.rotation = puntoCinturon.rotation;

        // Resetear físicas
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
