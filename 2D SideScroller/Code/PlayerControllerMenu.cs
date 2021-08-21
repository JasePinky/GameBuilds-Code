using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerControllerMenu : MonoBehaviour
{

    public float speed = 6.0f;
    public float jumpSpeed = 6.0f;
    public float gravity = 9.8f;
    public Text countText;
    public Text winText;
    public Text livesText;
    public GameObject winScreen;
    public GameObject outsideScreen;
    public GameObject ball;
    public float ballSpeed = 600.0f;
    public GameObject Emitter;
    public float timeBetweenShots = 0.5f;

    private Vector3 movedirection = Vector3.zero;
    private int count;
    private int lives;
    private float timestamp;

    void Start()
    {
        lives = 5;
        count = 0;
        livesText.text = "";
        SetCountText();
        SetLivesText();
        winText.text = "";
    }

    void Update()
    {
        if (Time.time >= timestamp && (Input.GetKeyDown(KeyCode.LeftControl))) {
            GameObject bullet = Instantiate(ball, Emitter.transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.right * ballSpeed);
            timestamp = Time.time + timeBetweenShots;
        }
        if (Input.GetKeyDown(KeyCode.D))
            transform.forward = new Vector3(0f, 0f, 1f);
        else if (Input.GetKeyDown(KeyCode.A))
            transform.forward = new Vector3(0f, 0f, -1f);
        else if (Input.GetKeyDown(KeyCode.S))
            transform.forward = new Vector3(1f, 0f, 0f);
        else if (Input.GetKeyDown(KeyCode.W))
            transform.forward = new Vector3(-1f, 0f, 0f);
        CharacterController controller = GetComponent<CharacterController>();
        movedirection.x = Input.GetAxis("Horizontal") * speed;
        if (controller.isGrounded)
        {

            if (Input.GetButton("Jump"))
                movedirection.y = jumpSpeed;
        }
        movedirection.y -= gravity * Time.deltaTime;
        controller.Move(movedirection * Time.deltaTime);
        if (lives <= 0)
        {
        }
        if (count >= 9)
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            other.gameObject.SetActive(true);
            transform.position = new Vector3(0, 1.5f, 0);
            lives = lives - 1;
            SetLivesText();

        }
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 1)
        {
            winText.text = "Would you like to start?";
            winScreen.SetActive(true);

        }

    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void NextLevel()
    {
        if (count >= 1)
        {
            outsideScreen.SetActive(true);
            SceneManager.LoadScene("Outside1");
        }
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene("Main Menu");
    }
}