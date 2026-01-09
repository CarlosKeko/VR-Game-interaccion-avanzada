using UnityEngine;

public class UIRaysToggle : MonoBehaviour
{
    public UIRayEnabler leftRay;
    public UIRayEnabler rightRay;

    public bool startOff = true;

    void Start()
    {
        if (startOff) SetRays(false);
    }

    public void SetRays(bool on)
    {
        if (rightRay) rightRay.SetOn(on);
        if (leftRay) leftRay.SetOn(on);
    }
}
