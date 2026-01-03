using UnityEngine;
using UnityEngine.XR.Hands; // Necesitas el paquete XR Hands de Unity

public class HandTrackingSwinger : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cameraTransform;
    public CharacterController characterController;

    [Header("Ajustes de Velocidad")]
    public float sensitivity = 3.0f;
    public float gravity = 9.81f;

    // Referencias a los subsistemas de manos de Unity
    private XRHandSubsystem handSubsystem;
    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;
    private float verticalVelocity;

    void Update()
    {
        // 1. Obtener el subsistema de manos si no lo tenemos
        if (handSubsystem == null)
        {
            var handSubsystems = new System.Collections.Generic.List<XRHandSubsystem>();
            SubsystemManager.GetSubsystems(handSubsystems);
            if (handSubsystems.Count > 0) handSubsystem = handSubsystems[0];
            return;
        }

        // 2. Obtener posiciones de las manos (Joint del centro de la palma)
        Vector3 currentLeftPos = GetHandPosition(Handedness.Left);
        Vector3 currentRightPos = GetHandPosition(Handedness.Right);

        // 3. Calcular movimiento
        float movement = Vector3.Distance(currentLeftPos, lastLeftHandPos) +
                         Vector3.Distance(currentRightPos, lastRightHandPos);

        if (Time.deltaTime > 0 && movement > 0.01f)
        {
            Vector3 direction = cameraTransform.forward;
            direction.y = 0;

            float speed = (movement / Time.deltaTime) * sensitivity;
            characterController.Move(direction * speed * Time.deltaTime);
        }

        // 4. Gravedad y guardado de posición
        ApplyGravity();
        lastLeftHandPos = currentLeftPos;
        lastRightHandPos = currentRightPos;
    }

    Vector3 GetHandPosition(Handedness handedness)
    {
        var hand = handedness == Handedness.Left ? handSubsystem.leftHand : handSubsystem.rightHand;
        if (hand.isTracked)
        {
            // Retorna la posición de la palma
            return hand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose pose) ? pose.position : Vector3.zero;
        }
        return Vector3.zero;
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded) verticalVelocity = -0.5f;
        else verticalVelocity -= gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }
}