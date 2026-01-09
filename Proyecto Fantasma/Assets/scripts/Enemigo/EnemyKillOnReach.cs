using UnityEngine;

public class EnemyKillOnReach : MonoBehaviour
{
    public Transform objetivo;
    public float distanciaMatar = 1f;
    public GameOverVR gameOver;

    private bool yaMato;

    private void Start()
    {
        distanciaMatar = 1f;
    }

    void Update()
    {
        if (yaMato) return;
        if (!objetivo || !gameOver) return;

        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = objetivo.position; b.y = 0f;

        float d = Vector3.Distance(a, b);
        //if (d < 2f) Debug.Log("Distancia al jugador: " + d.ToString("F2"));

        if (d <= distanciaMatar)
        {
            yaMato = true;
            //Debug.Log(">>> Llamando a GameOver.Morir()");
            gameOver.Morir();
        }
    }
}
