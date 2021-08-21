using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	Vector3 pos;
	public float LerpSpeed = 2;

	void Update () {
		pos.z = offset.z;
		pos.y = Mathf.Lerp (pos.y,target.position.y+offset.y,Time.deltaTime*LerpSpeed);
		pos.x = target.position.x;
		transform.position = pos;
	}
}
