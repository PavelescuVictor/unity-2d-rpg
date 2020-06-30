using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject enemy;
    public GameObject spawnPoint;
    public bool spawnOnce;
    public float spawnRate;
    public float nextSpawn;

    [SerializeField]
    private bool spawned;
    private Vector2 whereToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCharacter"))
        {
            if (spawnOnce == true && spawned == false)
            {
                whereToSpawn = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
                Instantiate(enemy, whereToSpawn, Quaternion.identity);
                spawned = true;
            }
            else if (spawnOnce == false)
            {
                if (Time.time > nextSpawn)
                {
                    nextSpawn = Time.time + spawnRate;
                    whereToSpawn = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
                    Instantiate(enemy, whereToSpawn, Quaternion.identity);
                }
            }
        };
    }
}
