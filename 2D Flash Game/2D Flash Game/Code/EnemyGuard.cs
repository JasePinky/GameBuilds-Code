using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuard : EnemyController
{
    Transform guard;
    bool canShoot = true;

    //Sets initial values and finds gameobjects.
    void Start()
    {
        enemySpeed = Random.Range(0.2f, 3f);

        guard = transform.GetChild(0);
        guardGun = transform.GetChild(1);
        guardGunBarrel = transform.GetChild(1).GetChild(0);        

        bulletPrefab = GameObject.Find("Enemy Bullet Prefab");
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        cleanupList = GameObject.Find("Cleanup list").GetComponent<Transform>();
    }

    //Makes guards face the player, their guns point at the player and lets them shoot with a timer.
    void Update()
    {
        if (guard != null)
        {
            Vector3 playerDir = guard.transform.position - playerPos.transform.position;
            playerDir.z = 0.0f;
            playerDir = playerDir.normalized;

            float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            guardGun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (playerPos.transform.position.x < transform.position.x)
            {
                guard.transform.localScale = new Vector2(-1, 1);
                guardGun.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                guard.transform.localScale = new Vector2(1, 1);
                guardGun.transform.localScale = new Vector2(-1, -1);
            }
        }
        else
        {
            GameObject enemyDied = Instantiate(deathPrefab, transform.position, deathPrefab.transform.rotation);
            enemyDied.transform.SetParent(cleanupList, false);
            Destroy(this.gameObject);
        }

        if (canShoot)
        {
            Invoke("ShootPlayer", shootTimer);
            canShoot = false;
        }

        transform.Translate(Vector2.right * (Time.deltaTime * enemySpeed));
    }

    //Intantiates bullets.
    void ShootPlayer()
    {
        GameObject bullet = Instantiate
                         (
                          bulletPrefab, 
                          new Vector2(guardGunBarrel.position.x, guardGunBarrel.position.y), 
                          guardGun.transform.rotation
                         );
        bullet.transform.SetParent(cleanupList, false);
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * -800);
        canShoot = true;      
    }

    //Destroys this guard when a cleanup collider is hit (stopped working I dont know why).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cleanup")
            Destroy(gameObject);
    }
}
