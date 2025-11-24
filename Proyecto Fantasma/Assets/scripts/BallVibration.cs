using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BallVibration : MonoBehaviour
{
    public Transform hand;
    public XRBaseInputInteractor controller; 

    public float maxDistance = 2f;
    public float maxAmplitude = 0.5f;
    public float maxDuration = 0.1f;

    [System.Obsolete]
    void Update()
    {
        if (hand == null || controller == null)
            return;

        float distance = Vector3.Distance(transform.position, hand.position);
        float intensity = Mathf.Clamp01(1 - (distance / maxDistance));

        if (intensity > 0f)
        {
            controller.SendHapticImpulse(intensity * maxAmplitude, maxDuration);
        }
    }
}   
