using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Upgrades upgrades;
    UIController UI;

    Vector2 initLaunchForce = new Vector2(4.5f, 4.8f);
    Vector2 launchDir;
    Vector2 playerSpeed;
    private Vector2 playerSpawn = new Vector2(-3f, -4.5f);
    private Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);

    public bool died = false;
    public bool enemySpawn = false;
    bool launched = false;
    bool preLaunched = false;
    bool floorDeath = false;
    bool playerReady = false;
    bool enemyHit = false; 
    bool playerHit = false;
    bool doOnce = true;
    bool doOnce2 = true;  

    Rigidbody2D rigidBody;
    SpriteRenderer playerColour;

    Transform revolverPivot;
    Transform cleanupList;
    Transform revolverBarrel;
    public GameObject grenadeParticle;
    public GameObject muzzleParticle;
    public GameObject playerParticle;
    public GameObject revolverParticle;
    GameObject bulletPrefab;
    private Transform revolverObject;

    // Variables for deleting and moving prefabs.
    private GameObject _Instance1;
    private GameObject _Instance2;

    public int maxBullets = 6;
    [HideInInspector]
    public int currentBullets;
    [HideInInspector]
    public float recoilForce = -500f;
    [HideInInspector]
    public float maxHealth = 100f;
    [HideInInspector]
    public float droppedMoney = 50f;
    [HideInInspector]
    public float launchForce = 20f;
    [HideInInspector]
    public float playerDrag = 0.3f;
    [HideInInspector]
    public float money = 0f;
    [HideInInspector]
    public float bounceForce = 2.3f;
    [HideInInspector]
    public float killCount;
    float speedFix;
    float currentHealth;

    void Start()
    {
        launchDir = new Vector2(launchForce, (launchForce + 3));
        currentHealth = maxHealth;
        currentBullets = maxBullets;
        upgrades = gameObject.GetComponent<Upgrades>();
        UI = GameObject.Find("Canvas").GetComponent<UIController>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerColour = GetComponent<SpriteRenderer>();

        revolverPivot = GameObject.Find("RevolverPivot").GetComponent<Transform>();
        revolverObject = GameObject.Find("Revolver").GetComponent<Transform>(); 
        
        bulletPrefab = GameObject.Find("Player Bullet Prefab");

        revolverObject = GameObject.Find("Revolver").GetComponent<Transform>();
        cleanupList = GameObject.Find("Cleanup list").GetComponent<Transform>();
        revolverPivot = GameObject.Find("RevolverPivot").GetComponent<Transform>();
        revolverBarrel = GameObject.Find("Revolver Barrel").GetComponent<Transform>();              

        ResetPlayer();
        playerReady = true;
    }

    void Update()
    {
        //inverts players speed on y axis so OnTriggerEnter function can use speedFix to compare y speed.
        playerSpeed = rigidBody.velocity;
        if(playerSpeed.y < 0)
            speedFix = -playerSpeed.y;
        else
            speedFix = playerSpeed.y;

        
        if (!preLaunched && Input.GetMouseButtonDown(0) && playerReady)
        {
            StartCoroutine(Launch());
            preLaunched = true;
            Debug.Log("Launched");
        }

        if (launched)
            AirControl();

        if (preLaunched)
            enemySpawn = true;
        if (!preLaunched)
            enemySpawn = false;

        if (currentHealth < 0.1f && !floorDeath && doOnce)
        {
            doOnce = false;
            UI.SendMessage("SetHealth", currentHealth);
            launched = false;
            rigidBody.drag = 1;
            rigidBody.AddForce(Vector3.left * playerSpeed.x * 1.2f, ForceMode2D.Impulse);
            StartCoroutine(PlayerDeath());
        }

        if (currentHealth < 0.1f && floorDeath && doOnce)
        {
            doOnce = false;
            UI.SendMessage("SetHealth", currentHealth);
            rigidBody.drag = 1;
            rigidBody.AddForce(Vector3.left * playerSpeed.x * 1.2f, ForceMode2D.Impulse);
            StartCoroutine(PlayerFloorDeath());
        }
        if (playerSpeed.y == 0 && playerSpeed.x == 0 && launched)
            currentHealth = 0;

        if (playerSpeed.y == 0 && launched)
        {
            doOnce2 = false;
            StartCoroutine(CheckDeath());
        }

        if (Input.GetKeyUp("r") && launched == true)
        {
            currentHealth = 0;
        }      
    }

    //Finds mouse position and points player towards it.
    //Lets player shoot towards mouse position causing force to be added backwards.
    void AirControl()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - gameObject.transform.position;
        mouseDir.z = 0.0f;
        mouseDir = mouseDir.normalized;

        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        revolverPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
     
        Vector2 forward = transform.TransformDirection(Vector2.right);
        Vector2 mousePos2 = mousePos - transform.position;
        if (Vector2.Dot(mousePos2, forward) < 0)
            revolverObject.transform.localScale = new Vector2(0.85f, -0.85f);
        if (Vector2.Dot(mousePos2, forward) > 0)
            revolverObject.transform.localScale = new Vector2(0.85f, 0.85f);

        if (currentBullets > 0 && Input.GetMouseButtonDown(0))
        {
            rigidBody.AddForce(mouseDir * recoilForce);
            currentBullets--;
            PlayerShoot();
            StartCoroutine(MuzzleParticle());
        }
    }

    void PlayerShoot()
    {
        GameObject bullet = Instantiate
                         (
                          bulletPrefab,
                          new Vector2(revolverBarrel.position.x, revolverBarrel.position.y),
                          revolverPivot.transform.rotation
                         );
        bullet.transform.SetParent(cleanupList, false);
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * 1400);
    }

    //Resets player back to spawn position.
    void ResetPlayer()
    {
        StopAllCoroutines();

        launched = false;
        preLaunched = false;
        floorDeath = false;
        playerReady = false;
        doOnce = true;

        currentHealth = maxHealth;
        rigidBody.velocity = new Vector2(0, 0);
        grenadeParticle.SetActive(false);     
        transform.localScale = new Vector2(0.5f, 0.5f);
        revolverObject.transform.localScale = new Vector2(0.85f, 0.85f);
        rigidBody.angularVelocity = 0f;       
        gameObject.transform.SetPositionAndRotation(playerSpawn, spawnRot);
        revolverPivot.rotation = Quaternion.AngleAxis(0f, Vector3.forward);

    }

    public void TakeDamage(int damage)
    {
        playerHit = true;
        if(playerHit)
        {
            StartCoroutine(PlayerHit());
            playerHit = false;
        }
        currentHealth = currentHealth - damage;
        UI.SendMessage("SetHealth", currentHealth);
    }

    //Used for ingame button to let the player ready up before starting the game.
    public void Ready()
    {
        died = false;
        playerReady = true;
        upgrades.SetUpgrades();

        currentHealth = maxHealth;
        currentBullets = maxBullets;
        rigidBody.drag = playerDrag;

        launchDir = new Vector2(launchForce, (launchForce + 3));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && !died && !enemyHit && playerSpeed.x < speedFix)
        {
            enemyHit = true;
            StartCoroutine(EnemyHit());
            if(playerSpeed.y < 0)
                rigidBody.AddForce(Vector3.up * -playerSpeed.y * (bounceForce * 0.8f), ForceMode2D.Impulse);
            if(playerSpeed.y > 0)
                rigidBody.AddForce(Vector3.up * playerSpeed.y * bounceForce, ForceMode2D.Impulse);
            rigidBody.AddForce(Vector3.right * 0.3f, ForceMode2D.Impulse);
            Destroy(other.gameObject);
            money += droppedMoney;
            killCount++;
        }

        if (other.tag == "Floor" && launched && playerSpeed.x > 0.2f && playerSpeed.x < speedFix)
        {
            SendMessage("TakeDamage", 30);
            rigidBody.drag = rigidBody.drag * 1.2f;
            rigidBody.AddForce(Vector3.up * -playerSpeed.y * (bounceForce * 0.6f), ForceMode2D.Impulse);
            rigidBody.AddForce(Vector3.left * 0.3f, ForceMode2D.Impulse);
        }

        if (other.tag == "Enemy" && !died && !enemyHit && playerSpeed.x > speedFix)
        {
            enemyHit = true;
            StartCoroutine(EnemyHit());
            rigidBody.AddForce(Vector3.up * playerSpeed.x * (bounceForce * 0.8f), ForceMode2D.Impulse);
            rigidBody.AddForce(Vector3.right * 0.3f, ForceMode2D.Impulse);
            Destroy(other.gameObject);
            money += droppedMoney;
            killCount++;
        }

        if (other.tag == "Floor" && launched && playerSpeed.x > 0.2f && playerSpeed.x > speedFix)
        {
            SendMessage("TakeDamage", 30);
            rigidBody.drag = rigidBody.drag * 1.2f;
            rigidBody.AddForce(Vector3.up * playerSpeed.x * (bounceForce * 0.6f), ForceMode2D.Impulse);
            rigidBody.AddForce(Vector3.left * 0.3f, ForceMode2D.Impulse);
        }

        if (other.tag == "Floor" && launched && playerSpeed.x < 0.2f)
        {
            floorDeath = true;
            died = true;
            launched = false;
            currentHealth = 0f;
            transform.localScale = new Vector2(0.5f, 0.1f);
        }

        if (other.tag == "Floor" && launched && playerSpeed.y > 0.2f)
        {
            died = true;
            launched = false;
            rigidBody.drag = 50;
            currentHealth = 0f;
        }
    }

    IEnumerator PlayerHit()
    {
        playerColour.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        playerColour.color = Color.white;
    }

    IEnumerator Launch()
    {
        rigidBody.AddForce(initLaunchForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        rigidBody.AddForce(launchDir, ForceMode2D.Impulse);
        grenadeParticle.SetActive(true);
        launched = true;
    }

    IEnumerator CheckDeath()
    {
        yield return new WaitForSeconds(1f);
        if (playerSpeed.y == 0 && launched)
            currentHealth = 0;
        doOnce2 = true;
    }

    //Timer for resetting the player after the player dies.
    IEnumerator PlayerDeath()
    {
        died = true;
        transform.localScale = new Vector2(0, 0);
        revolverObject.localScale = new Vector2(0, 0);
        _Instance1 = Instantiate(playerParticle, transform.position, transform.rotation);
        _Instance2 = Instantiate(revolverParticle, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        Destroy(_Instance1);
        Destroy(_Instance2);
        UI.SendMessage("PlayerDeath", 10, SendMessageOptions.DontRequireReceiver);
        ResetPlayer();      
    }

    //Timer for when the player died by hitting the floor.
    IEnumerator PlayerFloorDeath()
    {
        died = true;
        _Instance2 = Instantiate(revolverParticle, transform.position, transform.rotation);
        revolverObject.localScale = new Vector2(0, 0);
        yield return new WaitForSeconds(3f);     
        Destroy(_Instance2);
        UI.SendMessage("PlayerDeath", 10, SendMessageOptions.DontRequireReceiver);
        ResetPlayer();       
    }

    IEnumerator MuzzleParticle()
    {
        muzzleParticle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleParticle.SetActive(false);
    }

    IEnumerator EnemyHit()
    { 
        yield return new WaitForSeconds(0.1f);
        enemyHit = false;
    }
}
