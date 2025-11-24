using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class TriggerOtherSoundOnGrab : MonoBehaviour
{
    [Tooltip("El objeto que emitirá el sonido (debe tener un AudioSource)")]
    public AudioSource targetAudioSource;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (targetAudioSource != null)
        {
            targetAudioSource.Play();
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (targetAudioSource != null)
        {
            targetAudioSource.Stop();
        }
    }
}
