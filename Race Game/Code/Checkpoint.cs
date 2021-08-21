using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public bool finalCheckPoint;
    public int index;
    public GameController gameController;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        gameController.UpdatePlayerCheckpoint(other.tag, index, finalCheckPoint);

        if (other.tag == "Respawn")
        {
            spawnPoint.position = new Vector3(transform.position.x, transform.position.y, spawnPoint.position.z);
        }
    }        
}
