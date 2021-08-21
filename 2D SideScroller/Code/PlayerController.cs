using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0f;
	public float jumpSpeed = 6.0f;
	public float gravity = 9.8f;
    public GameObject jumpButton;
    public GameObject wandButton;
	public Text countText;
	public Text winText;
	public Text livesText;
	public Text keyText;
    public GameObject ball;
    public float ballSpeed = 600.0f;
    public GameObject Emitter;
    public float timeBetweenShots = 0.5f;
	public bool canJump;
	public bool jumpBuy;
    public bool wandBuy;
    public CharacterController controller;


    public Vector3 movedirection = Vector3.zero;
	private int count;
	private int lives;
    private float timestamp;
	private int Key;

    void Start ()
	{
		lives = 5;
		count = 0;
		livesText.text = "";
		SetCountText ();
		SetLivesText ();
		SetKeyText ();
		winText.text = "";
		LoadData();
	}
		
	void Update ()
    {
		SetCountText ();
		saveData ();
		if (Time.time >= timestamp && (Input.GetKey (KeyCode.LeftControl)))
        {
			GameObject bullet = Instantiate (ball, Emitter.transform.position, Quaternion.identity) as GameObject;
			bullet.GetComponent<Rigidbody> ().AddForce (transform.right * ballSpeed);
			bullet.transform.rotation = transform.rotation;
			timestamp = Time.time + timeBetweenShots;
		}
		if (Input.GetAxis("Horizontal") >= 0.01)
			transform.forward = new Vector3 (0f, 0f, 1f);
		else if (Input.GetAxis("Horizontal") <= -0.01)
			transform.forward = new Vector3 (0f, 0f, -1f);
		else if (Input.GetAxis("Vertical") <= -0.01)
			transform.forward = new Vector3 (1f, 0f, 0f);
		else if (Input.GetAxis("Vertical") >= 0.01)
			transform.forward = new Vector3 (-1f, 0f, 0f);
		 controller = GetComponent<CharacterController> ();
		movedirection.x = Input.GetAxis ("Horizontal") * speed;
		if (controller.isGrounded)
        {
			if (Input.GetButtonDown ("Jump"))
				movedirection.y = jumpSpeed;
				canJump = true;
		} 
		else if (controller.isGrounded == false && canJump == true)
        {
			if (Input.GetButtonDown ("Jump"))
            {
				movedirection.y = jumpSpeed;
				canJump = false;
            }
            movedirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            movedirection.y -= gravity * Time.deltaTime;
        }
		controller.Move (movedirection * Time.deltaTime);
	}
    //public void Jumpbuy() 
    //{
       // jumpBuy = !jumpBuy;
       // jumpButton.SetActive (false);
//	   }

    public void Wandbuy()
    {
        wandBuy = !wandBuy;
        wandButton.SetActive(false);
    }

	void saveData()
    {
		GlobalControl.Instance.Lives = lives;
		GlobalControl.Instance.Count = count;
	}

	void LoadData()
    {
		lives = GlobalControl.Instance.Lives;
		count = GlobalControl.Instance.Count;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("DeathZone"))
        {
			other.gameObject.SetActive (true);
			transform.position = new Vector3(0, 1.5f, 0);
			lives = lives - 1;
			SetLivesText ();

		}
		if (other.gameObject.CompareTag ("Exit"))
        {
			other.gameObject.SetActive (false);
			Key = Key + 1;
			SetKeyText ();
		}
		if (other.gameObject.CompareTag ("Pick Up"))
        {
			other.gameObject.SetActive (false);
			count = count + 1;

		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
			
	}

	void SetLivesText ()
	{
		livesText.text = "Lives: " + lives.ToString ();
		if (lives <= 0)
        {
			SceneManager.LoadScene ("Main Menu");
		}
	}
	public void SetKeyText () { keyText.text = "Key: " + Key.ToString ();
		if (Key >= 1)
        {
			saveData();
			int indexSC = SceneManager.GetActiveScene ().buildIndex;
			SceneManager.LoadScene(indexSC +1);		
	}
	
	}
}