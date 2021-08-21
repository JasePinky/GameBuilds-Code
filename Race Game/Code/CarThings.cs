using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarThings : MonoBehaviour
{
    public GameController controller;

    public float speedPlayer1;
    public float speedPlayer2;
    private float topSpeed = 200;
    private float currentSpeed = 0;
    private float pitch = 0;

    public AudioClip engine;

    public Text speedPlayer1Text;
    public Text speedPlayer2Text;

    public bool flippedP1;
    public bool flippedP2;

    [Header("Health")]
    public GameObject player;
    public float hp = 100;

    [Header("SpecialStuff")]
	public bool gotPickup;
    public GameObject smoke;
    public bool gotSmoke;
    public Text smokeTimer;
    public GameObject smokeIcon;
    public bool gotBoost;
	public GameObject fireParticle;
    public GameObject boostIcon;

    [Header("ShootyPickup")]
    public float timeBetweenShots = 0.2f;
    public float ammo = 0;
    public float spray;
    private float timestamp;
    public GameObject projectile;
    public GameObject emitterLeft;
    public GameObject emitterRight;
    public GameObject gunIcon;
    public GameObject guns;
    public AudioSource shoot;
    public Text ammoText;
    public bool shootSide;
    public bool gotGun;
    
    [Header("CameraStuff")]
    public GameObject[] cameraMode;
    public int index = 0;

    public GameObject[] cameraModeP2;
    public int indexP2 = 0;

    public bool player1;
    public Rigidbody Car;

    public List<AxleInfo> axleInfos;
    public float currentMotorTorque;
    public float maxMotorTorque;
    public float maxSteeringAngle;


    void SetSpeedPlayer1Text()
    {
        speedPlayer1Text.text = "KM/H : " + speedPlayer1.ToString ();
    }

    void SetSpeedPlayer2Text()
    {
        speedPlayer2Text.text = "KM/H : " + speedPlayer2.ToString();
    }

    //OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("SmokePickup") && !player1 && !gotPickup)
        {
			gotPickup = true;
            gotSmoke = true;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }
		if (other.gameObject.CompareTag("BoostPickup") && !player1 && !gotPickup)
        {
			gotPickup = true;
            gotBoost = true;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }
		if (other.gameObject.CompareTag("GunPickup") && !player1 && !gotPickup)
        {
			gotPickup = true;
            gotGun = true;
            ammo = 200;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }

		if (other.gameObject.CompareTag("SmokePickup") && player1 && !gotPickup)
        {
			gotPickup = true;
            gotSmoke = true;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }
		if (other.gameObject.CompareTag("BoostPickup") && player1 && !gotPickup)
        {
			gotPickup = true;
            gotBoost = true;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }
		if (other.gameObject.CompareTag("GunPickup") && player1 && !gotPickup)
        {
			gotPickup = true;
            gotGun = true;
            ammo = 200;
            other.gameObject.SetActive(false);
            StartCoroutine(PickupRespawn(other.gameObject));
        }

        if (other.gameObject.CompareTag("Score") && !player1)
        {
            other.gameObject.SetActive(false);
            controller.player1.score = controller.player1.score + 10;
        }

        if (other.gameObject.CompareTag("Score") && player1)
        {
            other.gameObject.SetActive(false);
            controller.player2.score = controller.player2.score + 10;
        }

        if (other.gameObject.CompareTag("offRoadTrigger"))
        {
            SceneManager.LoadScene("OffRoad");
        }
        if (other.gameObject.CompareTag("onRoadTrigger"))
        {
            SceneManager.LoadScene("RoadRacing");
        }
        if (other.gameObject.CompareTag("onStuntTrigger"))
        {
            SceneManager.LoadScene("StuntRacing");
        }
    }

    void start()
    {
        SetSpeedPlayer1Text ();
        SetSpeedPlayer2Text ();
        currentMotorTorque = maxMotorTorque;
    }

    //Update
    void Update()
    {
        SetSpeedPlayer1Text ();
        speedPlayer1 = Mathf.Round(Car.velocity.magnitude * 3.6f);

        SetSpeedPlayer2Text ();
        speedPlayer2 = Mathf.Round(Car.velocity.magnitude * 3.6f);

        currentSpeed = Car.velocity.magnitude * 3.6f;
        pitch = currentSpeed / topSpeed + 0.44f;
        transform.GetComponent<AudioSource> ().pitch = pitch;

        if (gotSmoke)
        {
            smokeIcon.SetActive(true);
            gotBoost = false;
            gotGun = false;
        }
        else smokeIcon.SetActive(false);
        if (gotBoost)
        {
            boostIcon.SetActive(true);
            gotSmoke = false;
            gotGun = false;
        }
        else boostIcon.SetActive(false);
        if (gotGun)
        {
            gunIcon.SetActive(true);
            guns.SetActive(true);
            gotSmoke = false;
            gotBoost = false;
        }
        else
        {
            gunIcon.SetActive(false);
            guns.SetActive(false);
        }

        if (hp <= 0 && !player1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180));
            Car.angularVelocity = Vector3.zero;
            Car.velocity = Vector3.zero;
            controller.player2.score = controller.player2.score - 30;
            hp = 100;
        }
        if (hp <= 0 && player1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180));
            Car.angularVelocity = Vector3.zero;
            Car.velocity = Vector3.zero;
            controller.player2.score = controller.player2.score - 30;
            hp = 100;
        }

        //Pickups
        if (Input.GetKeyDown(KeyCode.T) && !player1 && gotSmoke)
        {
            smoke.SetActive(true);
            StartCoroutine("SmokeReady");
            gotSmoke = false;
			gotPickup = false;
        }
        if (Input.GetKeyDown(KeyCode.T) && !player1 && gotBoost)
        {
            maxMotorTorque = maxMotorTorque * 1.5f;
			fireParticle.SetActive (true);
            StartCoroutine("Boost");         
            gotBoost = false;
			gotPickup = false;
        }        
        ammoText.text = ammo.ToString();
        if (Time.time >= timestamp && (Input.GetKey(KeyCode.T)) && !player1 && gotGun)
        {
            ammo = ammo - 2;
            shoot.Play();
            GameObject bullet = Instantiate(projectile, emitterLeft.transform.position, emitterLeft.transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = emitterLeft.transform.forward * 200;
            bullet = Instantiate(projectile, emitterRight.transform.position, emitterRight.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = emitterRight.transform.forward * 200;
            timestamp = Time.time + timeBetweenShots;        
			if (ammo == 0) 
			{
				gotGun = false;
				gotPickup = false;
			}
			
        }

        if (Input.GetKeyDown(KeyCode.Keypad9) && player1 && gotSmoke)
        {
            smoke.SetActive(true);
            StartCoroutine("SmokeReady");
            gotSmoke = false;
			gotPickup = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9) && player1 && gotBoost)
        {
            maxMotorTorque = maxMotorTorque * 1.5f;
			fireParticle.SetActive (true);
            StartCoroutine("Boost");
            gotBoost = false;
			gotPickup = false;
        }
        if (Time.time >= timestamp && (Input.GetKey(KeyCode.Keypad9)) && player1 && gotGun)
        {
            ammo = ammo - 2;
            shoot.Play();
            GameObject bullet = Instantiate(projectile, emitterLeft.transform.position, emitterLeft.transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = emitterLeft.transform.forward * 200;
            bullet = Instantiate(projectile, emitterRight.transform.position, emitterRight.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = emitterRight.transform.forward * 200;
            timestamp = Time.time + timeBetweenShots;
			if (ammo == 0) 
			{
				gotGun = false;
				gotPickup = false;
			}
        }

		if (Input.GetKeyDown(KeyCode.G) && !player1 && currentSpeed < 10)
        {
            transform.rotation =  Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0));
            Car.angularVelocity = Vector3.zero;
            Car.velocity = Vector3.zero;
            controller.player1.score = controller.player1.score - 10;
            hp = 100;
        }

		if (Input.GetKeyDown(KeyCode.KeypadEnter) && player1 && currentSpeed < 10)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0));
            Car.angularVelocity = Vector3.zero;
            Car.velocity = Vector3.zero;
            controller.player2.score = controller.player2.score - 10;
            hp = 100;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Switched");
            index++;
        }
        if (index > cameraMode.Length - 1)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = cameraMode.Length - 1;
        }

        for (int i = 0; i < cameraMode.Length; i++)
        {
            if (index == i)
            {
                cameraMode[i].SetActive(true);
            }
            else
                cameraMode[i].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Debug.Log("SwitchedP2");
            indexP2++;
        }
        if (indexP2 > cameraModeP2.Length - 1)
        {
            indexP2 = 0;
        }
        else if (indexP2 < 0)
        {
            indexP2 = cameraModeP2.Length - 1;
        }

        for (int i = 0; i < cameraModeP2.Length; i++)
        {
            if (indexP2 == i)
            {
                cameraModeP2[i].SetActive(true);
            }
            else
                cameraModeP2[i].SetActive(false);
        }

        if (!player1 && transform.eulerAngles.z > 175 && transform.eulerAngles.z < 185 && !flippedP1)
        {
            controller.player1.score = controller.player1.score + 20;
            Debug.Log ("Flip");
            flippedP1 = true;            
        }
        if (!player1 && transform.eulerAngles.z > -15 && transform.eulerAngles.z < 15)
        {          
            flippedP1 = false;
        }

        if (player1 && transform.eulerAngles.z > 175 && transform.eulerAngles.z < 185 && !flippedP2)
        {
            controller.player2.score = controller.player2.score + 20;
            Debug.Log("Flip");
            flippedP2 = true;
        }
        if (player1 && transform.eulerAngles.z > -15 && transform.eulerAngles.z < 15)
        {
            flippedP2 = false;
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque;
        float steering = maxSteeringAngle;

        if (!player1 && controller.starting == true)
        {
            motor = maxMotorTorque * Input.GetAxis("Vertical2");
            steering = maxSteeringAngle * Input.GetAxis("Horizontal2");
        }

        else if (player1 && controller.starting == true)
        {
            motor = maxMotorTorque * Input.GetAxis("Vertical");
            steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        }

        else
        {
            motor = 0;
            steering = 0;
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void DoDamage(float damage)
    {
        hp -= damage;       
    }

    IEnumerator SmokeReady()
    {
        smokeTimer.text = "15";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "14";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "13";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "12";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "11";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "10";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "9";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "8";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "7";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "6";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "5";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "4";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "3";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "2";
        yield return new WaitForSeconds(1);
        smokeTimer.text = "1";
        yield return new WaitForSeconds(1);
        smoke.SetActive(false);
        smokeTimer.text = "Ready";
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(4);
        maxMotorTorque = currentMotorTorque;
		fireParticle.SetActive (false);
    }

    IEnumerator PickupRespawn(GameObject pickup)
    {
        yield return new WaitForSeconds(20);
        pickup.SetActive(true);
    }
}