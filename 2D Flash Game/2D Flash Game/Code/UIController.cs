using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Upgrades
{
    Vector2 playerPos;

    GameObject playerObj;

    Text moneyText;
    Text distanceText;
    Image bulletImage;
    Image healthImage;
    Text killCountText;
    Text distanceScoreText;
    Text distanceHighScoreText;

    RectTransform newHighScore;
    RectTransform upgradeScreen;
    RectTransform resultsScreen;
    RectTransform bulletCountParent;

    private Image _Instance3;

    int oldCurrentBullets = 0;
    int currentBullets;

    float distance;
    float distanceHighScore = 0f;
    float killCount;
    float uiHealth;
    readonly float spacing = 10f;

    bool playerDied = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        playerObj = GameObject.Find("Player");

        dragCostText = GameObject.Find("DragCostText").GetComponent<Text>();
        dragLevelText = GameObject.Find("DragLevelText").GetComponent<Text>();

        moneyCostText = GameObject.Find("MoneyCostText").GetComponent<Text>();
        moneyLevelText = GameObject.Find("MoneyLevelText").GetComponent<Text>();

        healthCostText = GameObject.Find("HealthCostText").GetComponent<Text>();
        healthLevelText = GameObject.Find("HealthLevelText").GetComponent<Text>();

        recoilCostText = GameObject.Find("RecoilCostText").GetComponent<Text>();
        recoilLevelText = GameObject.Find("RecoilLevelText").GetComponent<Text>();

        launchCostText = GameObject.Find("LaunchCostText").GetComponent<Text>();
        launchLevelText = GameObject.Find("LaunchLevelText").GetComponent<Text>();

        bulletCostText = GameObject.Find("BulletsCostText").GetComponent<Text>();
        bulletLevelText = GameObject.Find("BulletsLevelText").GetComponent<Text>();

        dragUpgradeButton = GameObject.Find("DragUpgradeButton").GetComponent<Button>();
        moneyUpgradeButton = GameObject.Find("MoneyUpgradeButton").GetComponent<Button>();
        bulletUpgradeButton = GameObject.Find("BulletUpgradeButton").GetComponent<Button>();
        healthUpgradeButton = GameObject.Find("HealthUpgradeButton").GetComponent<Button>();
        launchUpgradeButton = GameObject.Find("LaunchUpgradeButton").GetComponent<Button>();
        recoilUpgradeButton = GameObject.Find("RecoilUpgradeButton").GetComponent<Button>();

        bulletImage = GameObject.Find("BulletImage").GetComponent<Image>();
        healthImage = GameObject.Find("Health").GetComponent<Image>();

        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        killCountText = GameObject.Find("Kill Score").GetComponent<Text>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        distanceScoreText = GameObject.Find("Distance Score").GetComponent<Text>();
        distanceHighScoreText = GameObject.Find("Distance HighScore").GetComponent<Text>();

        newHighScore = GameObject.Find("New HighScore").GetComponent<RectTransform>();
        resultsScreen = GameObject.Find("Result Screen").GetComponent<RectTransform>();
        upgradeScreen = GameObject.Find("Upgrade Screen").GetComponent<RectTransform>();
        bulletCountParent = GameObject.Find("Bullet Counter").GetComponent<RectTransform>();
    }

    void Update()
    {
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        distance = playerPos.x;
        distance = Mathf.Round(distance);

        //Checking if player has enough money to buy upgrades.
        if (player.money >= bulletUpgradeCost)
            canBuyBullet = true;
        else
            canBuyBullet = false;

        if (player.money >= healthUpgradeCost)
            canBuyHealth = true;
        else
            canBuyHealth = false;

        if (player.money >= moneyUpgradeCost)
            canBuyMoney = true;
        else
            canBuyMoney = false;

        if (player.money >= launchUpgradeCost)
            canBuyLaunch = true;
        else
            canBuyLaunch = false;

        if (player.money >= recoilUpgradeCost)
            canBuyRecoil = true;
        else
            canBuyRecoil = false;

        if (player.money >= dragUpgradeCost && dragUpgradeLevel < 5)
            canBuyDrag = true;
        else
            canBuyDrag = false;

        UpdateVariables();
        SetUI();
    }

    void UpdateVariables()
    {
        currentBullets = player.currentBullets;
        killCount = player.killCount;
        playerDied = player.died;
    }

    void PlayerDeath()
    {
        resultsScreen.localScale = new Vector2(1, 1);
        if (distance > distanceHighScore)
        {
            distanceHighScore = distance;
            newHighScore.transform.localScale = new Vector2(1, 1);
        }
        else if (distance < distanceHighScore)
            newHighScore.transform.localScale = new Vector2(0, 0);
    } 
    
    public void SetHealth(float health)
    {
        uiHealth = health;
        healthImage.fillAmount = uiHealth / player.maxHealth;
    }

    void SetUI()
    {
        //Ingame bullet counter, spawns images in a row. Always has the same amount displayed as the player can shoot.
        if (currentBullets != oldCurrentBullets)
        {
            foreach (Transform child in bulletCountParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            oldCurrentBullets = currentBullets;

            for (int i = currentBullets; i > 0; i--)
            {
                _Instance3 = Instantiate(bulletImage, new Vector2(0, 0), transform.rotation);
                float xPos = spacing * i;
                xPos = xPos + 40;
                _Instance3.rectTransform.SetPositionAndRotation(new Vector2(xPos, 0), bulletImage.rectTransform.rotation);
                _Instance3.rectTransform.SetParent(bulletCountParent, false);
            }
        }

        moneyText.text = player.money.ToString();

        if (distance < 0f)
            distance = 0f;
        distanceText.text = distance.ToString();

        if (distance > distanceHighScore && !playerDied)
        {
            distanceHighScore = distance;
            newHighScore.transform.localScale = new Vector2(1, 1);
        }

        if (distance < distanceHighScore && !playerDied)
            newHighScore.transform.localScale = new Vector2(0, 0);

        if (!playerDied)
            distanceScoreText.text = "Distance: " + distance.ToString();

        killCountText.text = "Kills: " + killCount.ToString();
        distanceHighScoreText.text = "HighScore: " + distanceHighScore.ToString();

        bulletLevelText.text = "Level: " + bulletUpgradeLevel.ToString();
        bulletCostText.text = "$$ " + bulletUpgradeCost.ToString();

        healthLevelText.text = "Level: " + healthUpgradeLevel.ToString();
        healthCostText.text = "$$ " + healthUpgradeCost.ToString();

        moneyLevelText.text = "Level: " + moneyUpgradeLevel.ToString();
        moneyCostText.text = "$$ " + moneyUpgradeCost.ToString();

        launchLevelText.text = "Level: " + launchUpgradeLevel.ToString();
        launchCostText.text = "$$ " + launchUpgradeCost.ToString();

        recoilLevelText.text = "Level: " + recoilUpgradeLevel.ToString();
        recoilCostText.text = "$$ " + recoilUpgradeCost.ToString();

        if (dragUpgradeLevel < 5)
        {
            dragLevelText.text = "Level: " + dragUpgradeLevel.ToString();
            dragCostText.text = "$$ " + dragUpgradeCost.ToString();
        }
        else if (dragUpgradeLevel == 5)
        {
            dragLevelText.text = "Level: MAX!";
            dragCostText.text = "$$ MAXED";
        }

        if (canBuyBullet)
            bulletUpgradeButton.interactable = true;
        else
            bulletUpgradeButton.interactable = false;

        if (canBuyHealth)
            healthUpgradeButton.interactable = true;
        else
            healthUpgradeButton.interactable = false;

        if (canBuyMoney)
            moneyUpgradeButton.interactable = true;
        else
            moneyUpgradeButton.interactable = false;

        if (canBuyLaunch)
            launchUpgradeButton.interactable = true;
        else
            launchUpgradeButton.interactable = false;

        if (canBuyRecoil)
            recoilUpgradeButton.interactable = true;
        else
            recoilUpgradeButton.interactable = false;

        if (canBuyDrag)
            dragUpgradeButton.interactable = true;
        else
            dragUpgradeButton.interactable = false;
    }

    //All the public voids for button interactions.
    public void PlayerReady()
    {
        upgradeScreen.localScale = new Vector2(0, 0);
        resultsScreen.localScale = new Vector2(0, 0);
        SetHealth(player.maxHealth);
        //bulletUpgradeLevel = bulletUpgradeLevel;
        //healthUpgradeLevel = healthUpgradeLevel;
        //launchUpgradeLevel = launchUpgradeLevel;
        //recoilUpgradeLevel = recoilUpgradeLevel;
        //moneyUpgradeLevel = moneyUpgradeLevel;
        //dragUpgradeLevel = dragUpgradeLevel;
        //UpdateLevels();
    }

    public void Next()
    {
        upgradeScreen.localScale = new Vector2(1, 1);
        resultsScreen.localScale = new Vector2(0, 0);
    }

    public void BulletUpgrade()
    {
        player.money = player.money - bulletUpgradeCost;
        bulletUpgradeCost = bulletUpgradeCost * 2f;
        bulletUpgradeCost = Mathf.Round(bulletUpgradeCost);
        bulletUpgradeLevel++;
        playerObj.SendMessage("BulletLevel", bulletUpgradeLevel, SendMessageOptions.DontRequireReceiver);
    }

    public void HealthUpgrade()
    {
        player.money = player.money - healthUpgradeCost;
        healthUpgradeCost = healthUpgradeCost * 1.1f;
        healthUpgradeCost = Mathf.Round(healthUpgradeCost);
        healthUpgradeLevel++;
        playerObj.SendMessage("HealthLevel", healthUpgradeLevel, SendMessageOptions.DontRequireReceiver);
    }

    public void MoneyUpgrade()
    {
        player.money = player.money - moneyUpgradeCost;
        moneyUpgradeCost = moneyUpgradeCost * 1.1f;
        moneyUpgradeCost = Mathf.Round(moneyUpgradeCost);
        moneyUpgradeLevel++;
        playerObj.SendMessage("MoneyLevel", moneyUpgradeLevel, SendMessageOptions.DontRequireReceiver);
    }

    public void LaunchUpgrade()
    {
        player.money = player.money - launchUpgradeCost;
        launchUpgradeCost = launchUpgradeCost * 1.5f;
        launchUpgradeCost = Mathf.Round(launchUpgradeCost);
        launchUpgradeLevel++;
        playerObj.SendMessage("LaunchLevel", launchUpgradeLevel, SendMessageOptions.DontRequireReceiver);
    }

    public void RecoilUpgrade()
    {
        player.money = player.money - recoilUpgradeCost;
        recoilUpgradeCost = recoilUpgradeCost * 1.4f;
        recoilUpgradeCost = Mathf.Round(recoilUpgradeCost);
        recoilUpgradeLevel++;
        playerObj.SendMessage("RecoilLevel", recoilUpgradeLevel, SendMessageOptions.DontRequireReceiver);
    }

    public void AirDragUpgrade()
    {
        if (dragUpgradeLevel < 5)
        {
            player.money = player.money - dragUpgradeCost;
            dragUpgradeCost = dragUpgradeCost * 2f;
            dragUpgradeCost = Mathf.Round(dragUpgradeCost);
            dragUpgradeLevel++;
            playerObj.SendMessage("DragLevel", dragUpgradeLevel, SendMessageOptions.DontRequireReceiver);
        }
        else if (dragUpgradeLevel == 5)
        {
            canBuyDrag = false;
        }
    }
}
