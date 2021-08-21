using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public Transform Player;
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 5;
    public float gravity = 10;
    public float health = 10;
    public GameObject ball;
    public float ballSpeed = -900.0f;
    public GameObject Emitter;
    public float timeBetweenShots = 0.5f;
    public float delay = 1;
    public bool canShoot = false;

    void Start()
    {
        Vector3 position = new Vector3(0, 0, 0);
        StartCoroutine("ShootDelay");
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        Gravity();
        transform.LookAt(Player);

        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {

            transform.position += transform.forward * MoveSpeed * Time.deltaTime;



            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                //Here Call any function U want Like Shoot at here or something
            }
        }
        Health();
        Shoot();
    }

    void Gravity()
    {
        CharacterController controller = GetComponent<CharacterController>();
        Vector3 gravityVector = new Vector3(0, -gravity, 0);
        controller.Move(gravityVector * Time.deltaTime);
    }
    void Health()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider bullet)
    {
        if (bullet.gameObject.tag == ("DeathZone"))
        {
            health--;
        }
    }
    void Shoot() {
        if (canShoot == true)
        {
            GameObject bullet = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * ballSpeed);
            bullet.transform.rotation = transform.rotation;
            canShoot = false;
        }
}

    IEnumerator ShootDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            {
                canShoot = true;
            }
        }
    }
    //credits : kevin v/d Bij
}
