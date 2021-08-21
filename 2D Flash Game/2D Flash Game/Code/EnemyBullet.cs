using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBullet : MonoBehaviour
{
    private UnityAction playertHit;
    GameObject hitObject;

    void Start()
    {
        playertHit += SendDamage;
        playertHit += DestroySelf;
    }

    void SendDamage()
    {
        hitObject.SendMessage("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    //Checks if bullets hit the player, sends damage if true.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitObject = collision.gameObject;
            playertHit();
        }
        if (collision.tag == "Cleanup")
            Destroy(gameObject);
    }
}
