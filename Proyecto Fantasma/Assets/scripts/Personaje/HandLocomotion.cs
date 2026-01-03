using UnityEngine;

public class HandLocomotion : MonoBehaviour
{
    [Header("Referencias")]
    public Transform manoIzquierda;
    public Transform manoDerecha;
    public Transform camaraPrincipal; // El Head del XR Origin
    public CharacterController characterController;

    [Header("Configuración de Movimiento")]
    public float multiplicadorVelocidad = 2.5f;
    public float gravedad = 9.81f;
    public float umbralMovimiento = 0.012f; // Para ignorar temblores leves

    private Vector3 posAnteriorIzquierda;
    private Vector3 posAnteriorDerecha;
    private float velocidadVertical;

    void Start()
    {
        // Guardamos las posiciones iniciales locales
        posAnteriorIzquierda = manoIzquierda.localPosition;
        posAnteriorDerecha = manoDerecha.localPosition;
    }

    void Update()
    {
        // 1. Calculamos cuánto se han movido las manos respecto al frame anterior
        float distIzquierda = Vector3.Distance(manoIzquierda.localPosition, posAnteriorIzquierda);
        float distDerecha = Vector3.Distance(manoDerecha.localPosition, posAnteriorDerecha);

        float movimientoTotal = distIzquierda + distDerecha;

        // 2. Si el braceo es suficiente, movemos al jugador
        if (Time.deltaTime > 0 && movimientoTotal > umbralMovimiento)
        {
            // El movimiento es en la dirección donde mira la cámara
            Vector3 direccion = camaraPrincipal.forward;
            direccion.y = 0; // Evitamos que el jugador camine hacia arriba/abajo

            float velocidad = (movimientoTotal / Time.deltaTime) * multiplicadorVelocidad;
            characterController.Move(direccion * velocidad * Time.deltaTime);
        }

        // 3. Aplicar Gravedad básica
        AplicarGravedad();

        // 4. Actualizar posiciones para el siguiente frame
        posAnteriorIzquierda = manoIzquierda.localPosition;
        posAnteriorDerecha = manoDerecha.localPosition;
    }

    void AplicarGravedad()
    {
        if (characterController.isGrounded)
        {
            velocidadVertical = -0.5f;
        }
        else
        {
            velocidadVertical -= gravedad * Time.deltaTime;
        }
        characterController.Move(new Vector3(0, velocidadVertical, 0) * Time.deltaTime);
    }
}