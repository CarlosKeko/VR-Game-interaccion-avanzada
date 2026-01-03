using UnityEngine;
using UnityEngine.InputSystem;

public class BotonRepararMando : MonoBehaviour
{
    // Aquí asignaremos el botón A (Derecha) o X (Izquierda)
    public InputActionProperty botonA_X;

    public bool EstaPresionado()
    {
        print(botonA_X.action.ReadValue<float>());

        return botonA_X.action.ReadValue<float>() > 0.1f;
    }
}