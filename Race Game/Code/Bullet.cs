using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject Prefab;
    public float Explosion = 1.5f;
    public float damage = 9;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 1, Color.red);
        RaycastHit hit; transform.position += transform.forward * Time.deltaTime * 10;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.45F))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, Explosion);
            int i = 0;

            GameObject temp = Instantiate(Prefab, transform.position, transform.rotation) as GameObject;
            Destroy(temp, 0.5f);
            while (i < hitColliders.Length)
            {
                hitColliders[i].SendMessage("DoDamage", damage, SendMessageOptions.DontRequireReceiver);
                i++;

                transform.parent = null;
                Destroy(gameObject);
            }
        }
    }
}
