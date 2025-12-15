using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadioDialController : MonoBehaviour
{
    // Esta será la variable global que afectará al juego (sonido, enemigos, etc.)
    public float SintonizacionGlobal = 0f;

    // Variable para almacenar la rotación inicial de la rueda.
    private float rotationOffset;
    private float previousRotation = 0f;

    // Puedes ajustar cuánto afecta la rotación a la sintonización
    [Tooltip("Grados de rotación necesarios para cambiar la sintonización en 1 unidad.")]
    public float gradosPorUnidad = 5f;

    private void Start()
    {
        // En este ejemplo, usaremos la rotación alrededor del eje Y local
        rotationOffset = transform.localEulerAngles.y;
        previousRotation = GetClampedRotation();
    }

    private void Update()
    {
        // Obtenemos la rotación actual.
        float currentRotation = GetClampedRotation();

        // Calculamos cuánto ha cambiado la rotación desde el frame anterior.
        float rotationDelta = currentRotation - previousRotation;

        // Manejar el "wrap-around" (cuando pasa de 359 a 0 o viceversa).
        // Esto es esencial si la rueda gira 360 grados sin límites.
        if (rotationDelta > 180)
        {
            rotationDelta -= 360;
        }
        else if (rotationDelta < -180)
        {
            rotationDelta += 360;
        }

        // Convertimos el cambio de rotación en un cambio en la sintonización.
        float tuningDelta = rotationDelta / gradosPorUnidad;

        // Aplicamos el cambio a la variable global.
        SintonizacionGlobal += tuningDelta;

        // Opcional: Limitar la sintonización a un rango (ej: 0 a 100).
        SintonizacionGlobal = Mathf.Clamp(SintonizacionGlobal, 0f, 100f);

        // Actualizamos la rotación anterior para el siguiente frame.
        previousRotation = currentRotation;

        print(rotationDelta);

        // Lógica de sonido/juego (llamar a la función que revisa la sintonización)
        CheckRadioState();
    }

    // Función que lee la rotación y la "aplana" a un rango de 0-360.
    private float GetClampedRotation()
    {
        // Usamos transform.localEulerAngles y elegimos el eje correcto.
        float rotation = transform.localEulerAngles.y;

        // Normalizamos de 0 a 360.
        if (rotation < 0)
        {
            rotation += 360;
        }

        return rotation;
    }

    // --- Lógica del Juego (Sintonización y Sonido) ---
    private void CheckRadioState()
    {
        // Aquí debes implementar la lógica que mencionaste:
        // 1. Número de sintonización objetivo (random)
        // 2. Ruido de radio rota / Ruido de radio sintonizada

        // Por ejemplo:
        // float targetFrequency = GetComponentInParent<RadioComponent>().TargetFrequency;
        // if (Mathf.Abs(SintonizacionGlobal - targetFrequency) < 1.0f)
        // {
        //    // Radio bien sintonizada (silencia el ruido roto, activa el sonido de "funciona")
        // }
        // else
        // {
        //    // Radio mal sintonizada (activa el ruido de radio rota)
        // }
    }
}