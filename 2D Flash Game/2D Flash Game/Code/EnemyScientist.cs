using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScientist : EnemyController
{
    //Sets initial values and finds gameobjects.
    void Start()
    {
        enemyImage = transform.GetChild(0);
        enemySpeed = Random.Range(0.2f, 3f);
        cleanupList = GameObject.Find("Cleanup list").GetComponent<Transform>();
    }

    //Checks if child object was destroyed and destroys gameobject if true.
    void Update()
    {
        if (enemyImage == null)
        {
            GameObject enemyDied = Instantiate(deathPrefab, transform.position, deathPrefab.transform.rotation);
            enemyDied.transform.SetParent(cleanupList, false);
            Destroy(this.gameObject);
        }
        transform.Translate(Vector2.right * (Time.deltaTime * enemySpeed));
    }

    //Destroys this scientist when a cleanup collider is hit (stopped working I dont know why).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cleanup")
            Destroy(gameObject);
    }
}
