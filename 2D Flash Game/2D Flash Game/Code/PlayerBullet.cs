using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    PlayerController player;

    public GameObject explosionParticles;
    Transform cleanupList;

    float hitRadius = 2f;
    bool doOnce = true;

    //Finds gameojects.
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        cleanupList = GameObject.Find("Cleanup list").GetComponent<Transform>();
    }   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }

    List<GameObject> objInArea = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            objInArea.Add(collision.gameObject);
            Debug.Log("Enemy hit");
        }
        else
        {
            if (doOnce)
            {
                GameObject explosion = Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
                explosion.transform.SetParent(cleanupList, false);
                Destroy(gameObject);
                foreach (GameObject obj in objInArea)
                {
                    player.money = player.money + player.droppedMoney;
                    Destroy(obj);
                }

                doOnce = false;
            }
        }
    }
}