using UnityEngine;
using System.Collections;

public class ProjectileShooter : MonoBehaviour {

	public GameObject ball;
	public float ballSpeed = -600.0f;


	void Start () {
		InvokeRepeating ("LaunchBall", 0f, 4f);
	}
	

	void LaunchBall () {
		GameObject bullet = Instantiate (ball, transform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody> ().AddForce (transform.forward * ballSpeed);
	}
}
