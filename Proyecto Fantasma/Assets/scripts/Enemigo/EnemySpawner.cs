using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpawnAreaCircle area;
    public GameObject enemyPrefab;
    public Transform objetivo;

    [Header("Spawn")]
    public float intervalo = 2f;
    public int maxEnemigos = 8;

    [Header("Control")]
    public bool spawnear = false; //por defecto NO spawnea

    private float t;
    private readonly List<GameObject> vivos = new();

    void Update()
    {
        // Limpieza
        vivos.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (!spawnear) return;              // si está apagado, no hace nada
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

        Vector3 pos = area.GetRandomPointOnEdge();
        GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity);
        vivos.Add(e);

        var mover = e.GetComponent<EnemyMoveToTarget>();
        if (mover)
        {

            mover.objetivo = objetivo;
            mover.controladoPorSpawner = true;
        }
    }

    // Métodos para que la radio controle el spawner
    public void SetSpawning(bool on)
    {
        spawnear = on;
        t = 0f; // reinicia el temporizador (opcional)
    }

    public void DespawnAll()
    {
        foreach (var e in vivos)
            if (e) Destroy(e);
        vivos.Clear();
    }
}
