using System;
using UnityEngine;

public class EnemyFlashlightKill : MonoBehaviour
{
    public void Morir()
    {
        Debug.Log("Enemigo muerto");
        Destroy(gameObject);
    }
}
