using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    // Boolean para dejar el enemigo de referencia quieto mientras hacemos el desarrollo
    public bool controladoPorSpawner = false;


    public Transform objetivo;
    public float velocidad = 1.0f;
    public float distanciaStop = 0.6f;

    void Update()
    {
        if (!controladoPorSpawner) return;

        if (!objetivo) return;

        Vector3 to = objetivo.position - transform.position;
        to.y = 0f;

        float d = to.magnitude;
        if (d <= distanciaStop) return;

        Vector3 dir = to / d;

        transform.position += dir * velocidad * Time.deltaTime;

        // Que mire hacia donde va 
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
