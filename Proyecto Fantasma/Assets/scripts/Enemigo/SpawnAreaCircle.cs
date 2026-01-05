using UnityEngine;

public class SpawnAreaCircle : MonoBehaviour
{
    public float radio = 6f;
    public float alturaY = 0f; // offset vertical

    public Vector3 GetRandomPointOnEdge()
    {
        float ang = Random.Range(0f, Mathf.PI * 2f);
        Vector3 dir = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang));
        Vector3 p = transform.position + dir * radio;
        p.y = transform.position.y + alturaY;
        return p;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
