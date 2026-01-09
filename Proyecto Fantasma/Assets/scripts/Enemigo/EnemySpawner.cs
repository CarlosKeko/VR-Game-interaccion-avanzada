using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpawnAreaCircle area;
    public GameObject enemyPrefab;
    public Transform objetivo;

    [Header("Game Over")]
    public GameOverVR gameOver;

    [Header("Spawn")]
    public float intervalo = 7f;
    public int maxEnemigos = 8;

    [Header("Control")]
    public bool spawnear = false;

    [Header("Suelo")]
    public LayerMask groundMask;          
    public float raycastHeight = 10f;     
    public float extraUp = 0.02f;         // para que no se “meta” en el suelo

    private float t;
    private readonly List<GameObject> vivos = new();

    void Update()
    {
        vivos.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (!spawnear) return;
        if (vivos.Count >= maxEnemigos) return;

        t += Time.deltaTime;
        if (t >= intervalo)
        {
            t = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (!area || !enemyPrefab || !objetivo) return;

        // Punto en el borde (X/Z)
        Vector3 pos = area.GetRandomPointOnEdge();

        // Raycast hacia abajo para encontrar el suelo real
        Vector3 origen = pos + Vector3.up * raycastHeight;
        if (Physics.Raycast(origen, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
        {
            pos = hit.point;
        }

        GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity);
        vivos.Add(e);

        // Apoyar al suelo usando collider (corrige pivots raros)
        PlaceOnGround(e, pos.y, extraUp);

        // Movimiento hacia el objetivo
        var mover = e.GetComponentInChildren<EnemyMoveToTarget>(true);
        if (mover)
        {
            mover.objetivo = objetivo;
            mover.controladoPorSpawner = true;
        }

        var kill = e.GetComponentInChildren<EnemyKillOnReach>(true);
        if (kill)
        {
            kill.objetivo = objetivo;

            if (gameOver == null) gameOver = Object.FindFirstObjectByType<GameOverVR>();
            kill.gameOver = gameOver;
        }
        else
        {
            Debug.LogWarning("El enemigo instanciado NO tiene EnemyKillOnReach en raíz ni en hijos: " + e.name);
        }

    }

    void PlaceOnGround(GameObject enemy, float groundY, float up)
    {
        Collider col = enemy.GetComponentInChildren<Collider>();
        if (col == null)
        {
            // Sin collider: lo dejamos justo encima del suelo
            Vector3 p = enemy.transform.position;
            p.y = groundY + up;
            enemy.transform.position = p;
            return;
        }

        // Queremos que el punto más bajo del collider toque groundY
        float bottomY = col.bounds.min.y;
        float delta = (groundY + up) - bottomY;
        enemy.transform.position += Vector3.up * delta;
    }

    public void SetSpawning(bool on)
    {
        spawnear = on;
        t = 0f;
    }

    public void DespawnAll()
    {
        foreach (var e in vivos)
            if (e) Destroy(e);
        vivos.Clear();
    }

    public void SpawnNow()
    {
        // Respeta el límite
        vivos.RemoveAll(e => e == null || !e.activeInHierarchy);
        if (vivos.Count >= maxEnemigos) return;

        Spawn();
    }
}
