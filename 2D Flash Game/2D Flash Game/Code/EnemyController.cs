using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController player;

    public GameObject[] enemies = new GameObject[2];

    protected GameObject bulletPrefab;
    public GameObject deathPrefab;
    public Rigidbody2D playerRigid;

    bool isSpawning = false;

    readonly float minWait = 2f;
    readonly float maxWait = 5f;
    float playerSpeed;
    protected int damage;
    protected float shootTimer = 3f;

    protected Transform playerPos;
    protected Transform enemyImage;
    protected Transform guardGun;
    protected Transform guardGunBarrel;
    protected Transform cleanupList;
    Transform groundSpawn;
    Transform airSpawn;
    Transform enemyList;

    protected float enemySpeed;

    //Finds gameobjects.
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        //airSpawn = GameObject.Find("AirSpawn").GetComponent<Transform>();
        enemyList = GameObject.Find("Enemy list").GetComponent<Transform>();
        groundSpawn = GameObject.Find("GroundSpawn").GetComponent<Transform>();
        cleanupList = GameObject.Find("Cleanup list").GetComponent<Transform>();
    }


    //Makes spawn positions follow the player and checks if enemies are allowed to spawn.
    void Update()
    {
        groundSpawn.transform.position = new Vector2(playerPos.position.x + 20, -4.6f);
        playerSpeed = playerRigid.velocity.x;

        if (!isSpawning && player.enemySpawn)
        {
            float timer = Random.Range((minWait / playerSpeed), (maxWait / playerSpeed));
            Invoke("SpawnEnemy", timer);
            isSpawning = true;
        }

        if (!player.enemySpawn)
        {
            DestroyAll();
            isSpawning = false;
        }
    }

    //Spawns Enemies.
    void SpawnEnemy()
    {
        if (Random.value > 0.1)
        {
            GameObject obj = Instantiate(enemies[0], new Vector2(groundSpawn.position.x, groundSpawn.position.y), transform.rotation);
            obj.transform.SetParent(enemyList, false);

            isSpawning = false;
        }
        else
        {
            GameObject obj = Instantiate(enemies[1], new Vector2(groundSpawn.position.x, groundSpawn.position.y), transform.rotation);
            obj.transform.SetParent(enemyList, false);

            isSpawning = false;
        }
    }

    //Destroys all Enemies that have spawned.
    public void DestroyAll()
    {
        foreach (Transform child in enemyList)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in cleanupList)
        {
            Destroy(child.gameObject);
        }
    }
}
