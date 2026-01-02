using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RespawnEnCinturon : MonoBehaviour
{
    public Transform puntoCinturon;

    [Header("Rotación al respawnear (por objeto)")]
    public Vector3 respawnEulerOffset; // ej: (0, 90, 0) para mirar a la derecha

    private XRGrabInteractable grab;
    private Rigidbody rb;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(CuandoSeCoge);
        grab.selectExited.AddListener(CuandoSeSuelta);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(CuandoSeCoge);
        grab.selectExited.RemoveListener(CuandoSeSuelta);
    }

    void Start() => PonerEnCinturon();

    void CuandoSeCoge(SelectEnterEventArgs args) => PonerEnModoMundo();

    void CuandoSeSuelta(SelectExitEventArgs args) => StartCoroutine(RespawnSiguienteFrame());

    IEnumerator RespawnSiguienteFrame()
    {
        yield return null;
        if (grab != null && grab.isSelected) yield break;
        PonerEnCinturon();
    }

    void PonerEnCinturon()
    {
        if (!puntoCinturon) return;

        Vector3 worldScale = transform.lossyScale;

        transform.SetParent(puntoCinturon, true);

        transform.localPosition = Vector3.zero;

        transform.rotation = puntoCinturon.rotation * Quaternion.Euler(respawnEulerOffset);


        ApplyWorldScale(worldScale);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void PonerEnModoMundo()
    {
        transform.SetParent(null, true);

        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void ApplyWorldScale(Vector3 targetWorldScale)
    {
        Transform p = transform.parent;
        if (p == null)
        {
            transform.localScale = targetWorldScale;
            return;
        }

        Vector3 parentScale = p.lossyScale;

        transform.localScale = new Vector3(
            parentScale.x != 0 ? targetWorldScale.x / parentScale.x : transform.localScale.x,
            parentScale.y != 0 ? targetWorldScale.y / parentScale.y : transform.localScale.y,
            parentScale.z != 0 ? targetWorldScale.z / parentScale.z : transform.localScale.z
        );
    }
}
