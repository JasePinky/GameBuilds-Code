using UnityEngine;

public class EnemyDeleter : MonoBehaviour
{
    Transform playerPos;

    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = new Vector2(playerPos.position.x - 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
