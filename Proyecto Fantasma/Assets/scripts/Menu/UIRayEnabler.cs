using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class UIRayEnabler : MonoBehaviour
{
    XRRayInteractor ray;
    XRInteractorLineVisual line;
    XRInteractorReticleVisual reticle;

    void Awake()
    {
        ray = GetComponent<XRRayInteractor>();
        line = GetComponent<XRInteractorLineVisual>();
        reticle = GetComponent<XRInteractorReticleVisual>();
    }

    public void SetOn(bool on)
    {
        if (ray) ray.enabled = on;
        if (line) line.enabled = on;
        if (reticle) reticle.enabled = on;
    }
}
