using UnityEngine;
using System.Collections;

public class GlobalControl : MonoBehaviour {

	public static GlobalControl Instance;
	public int Lives;
	public int Count;
    //public bool jumpBuy;
    //public PlayerController control;

	void Awake ()   
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
    //void Update()
    //{
       // if (control.controller.isGrounded == false && control.canJump == true && jumpBuy == true)
       // {
         //   if (Input.GetButtonDown("Jump"))
       //     {
       //         control.movedirection.y = control.jumpSpeed;
       //         control.canJump = false;
       //     }
      //  }

   // }
}