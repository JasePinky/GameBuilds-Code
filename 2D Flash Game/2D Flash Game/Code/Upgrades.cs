using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    protected PlayerController player;

    int origBullet;
    float origHealth;
    float origmoney;
    float origLaunch;
    float origRecoil;
    float origDrag;

    protected Text bulletLevelText;
    protected Text bulletCostText;
    protected Button bulletUpgradeButton;
    protected bool canBuyBullet = false;
    protected float bulletUpgradeCost = 5000f;
    public int bulletUpgradeLevel;

    protected Text healthLevelText;
    protected Text healthCostText;
    protected Button healthUpgradeButton;
    protected bool canBuyHealth = false;
    protected float healthUpgradeCost = 200f;
    float healthUpgradeAmount = 20f;
    float newHealth;
    public int healthUpgradeLevel;

    protected Text moneyLevelText;
    protected Text moneyCostText;
    protected Button moneyUpgradeButton;
    protected bool canBuyMoney = false;
    protected float moneyUpgradeCost = 100f;
    float moneyUpgradeAmount = 10f;
    float newDroppedMoney;
    public int moneyUpgradeLevel;

    protected Text launchLevelText;
    protected Text launchCostText;
    protected Button launchUpgradeButton;
    protected bool canBuyLaunch = false;
    protected float launchUpgradeCost = 700f;
    float launchUpgradeAmount = 2f;
    float newLaunchForce;
    public int launchUpgradeLevel;

    protected Text recoilLevelText;
    protected Text recoilCostText;
    protected Button recoilUpgradeButton;
    protected bool canBuyRecoil = false;
    protected float recoilUpgradeCost = 300f;
    float recoilUpgradeAmount = -50f;
    float newRecoilForce;
    public int recoilUpgradeLevel;

    protected Text dragLevelText;
    protected Text dragCostText;
    protected Button dragUpgradeButton;
    protected bool canBuyDrag = false;
    protected float dragUpgradeCost = 2000f;
    float dragUpgradeAmount = -0.06f;
    float newPlayerDrag;
    public int dragUpgradeLevel;

    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();

        origBullet = player.maxBullets;
        origHealth = player.maxHealth;
        origmoney = player.droppedMoney;
        origLaunch = player.launchForce;
        origRecoil = player.recoilForce;
        origDrag = player.playerDrag;

        SetInitUpgrades();
    }

    void Update()
    {
        BounceForceBalance();
    }

    //Sets upgrade levels during start.
    void SetInitUpgrades()
    {
        for (int i = bulletUpgradeLevel; i > 0; i--)
        {
            bulletUpgradeCost = bulletUpgradeCost * 2f;
            bulletUpgradeCost = Mathf.Round(bulletUpgradeCost);
            player.maxBullets++;
        }

        for (int i = healthUpgradeLevel; i > 0; i--)
        {
            healthUpgradeCost = healthUpgradeCost * 1.1f;
            healthUpgradeCost = Mathf.Round(healthUpgradeCost);
            newHealth = player.maxHealth + healthUpgradeAmount;
            player.maxHealth = newHealth;
        }

        for (int i = moneyUpgradeLevel; i > 0; i--)
        {
            moneyUpgradeCost = moneyUpgradeCost * 1.1f;
            moneyUpgradeCost = Mathf.Round(moneyUpgradeCost);
            newDroppedMoney = player.droppedMoney + moneyUpgradeAmount;
            player.droppedMoney = newDroppedMoney;
        }

        for (int i = launchUpgradeLevel; i > 0; i--)
        {
            launchUpgradeCost = launchUpgradeCost * 1.5f;
            launchUpgradeCost = Mathf.Round(launchUpgradeCost);
            newLaunchForce = player.launchForce + launchUpgradeAmount;
            player.launchForce = newLaunchForce;
        }

        for (int i = recoilUpgradeLevel; i > 0; i--)
        {
            recoilUpgradeCost = recoilUpgradeCost * 1.4f;
            recoilUpgradeCost = Mathf.Round(recoilUpgradeCost);
            recoilUpgradeAmount = recoilUpgradeAmount * 0.95f;
            newRecoilForce = player.recoilForce + recoilUpgradeAmount;
            player.recoilForce = newRecoilForce;
        }

        if (dragUpgradeLevel < 5)
        {
            for (int i = dragUpgradeLevel; i > 0; i--)
            {
                dragUpgradeCost = dragUpgradeCost * 2f;
                dragUpgradeCost = Mathf.Round(dragUpgradeCost);
                newPlayerDrag = player.playerDrag + dragUpgradeAmount;
                player.playerDrag = newPlayerDrag;
            }
        }
        else if (dragUpgradeLevel == 5)
        {
            player.playerDrag = 0;
            canBuyDrag = false;
        }
    }

    //Sets upgrade levels when the player is ready.
    public void SetUpgrades()
    {
        player.maxBullets = origBullet;
        player.maxHealth = origHealth;
        player.droppedMoney = origmoney;
        player.launchForce = origLaunch;
        player.recoilForce = origRecoil;
        player.playerDrag = origDrag;

        for (int i = bulletUpgradeLevel; i > 0; i--)
        {
            player.maxBullets++;
        }

        for (int i = healthUpgradeLevel; i > 0; i--)
        {
            newHealth = player.maxHealth + healthUpgradeAmount;
            player.maxHealth = newHealth;
        }

        for (int i = moneyUpgradeLevel; i > 0; i--)
        {
            newDroppedMoney = player.droppedMoney + moneyUpgradeAmount;
            player.droppedMoney = newDroppedMoney;
        }

        for (int i = launchUpgradeLevel; i > 0; i--)
        {
            newLaunchForce = player.launchForce + launchUpgradeAmount;
            player.launchForce = newLaunchForce;
        }

        for (int i = recoilUpgradeLevel; i > 0; i--)
        {
            recoilUpgradeAmount = recoilUpgradeAmount * 0.95f;
            newRecoilForce = player.recoilForce + recoilUpgradeAmount;
            player.recoilForce = newRecoilForce;
        }

        if (dragUpgradeLevel < 5)
        {
            for (int i = dragUpgradeLevel; i > 0; i--)
            {
                newPlayerDrag = player.playerDrag + dragUpgradeAmount;
                player.playerDrag = newPlayerDrag;
            }
        }
        else if (dragUpgradeLevel == 5)
        {
            player.playerDrag = 0;
            canBuyDrag = false;
        }
    }

    //Functions for setting levels from UI class.
    public void BulletLevel(int level)
    {
        bulletUpgradeLevel = level;
    }
    public void HealthLevel(int level)
    {
        healthUpgradeLevel = level;
    }
    public void MoneyLevel(int level)
    {
        moneyUpgradeLevel = level;
    }
    public void LaunchLevel(int level)
    {
        launchUpgradeLevel = level;
    }
    public void RecoilLevel(int level)
    {
        recoilUpgradeLevel = level;
    }
    public void DragLevel(int level)
    {
        dragUpgradeLevel = level;
    }

    //Change bounce force based on dragUpgradeLevel to stop infinitly increasing bounciness
    void BounceForceBalance()
    {
        if (dragUpgradeLevel == 1)
            player.bounceForce = 2.2f;

        if (dragUpgradeLevel == 2)
            player.bounceForce = 2.18f;

        if (dragUpgradeLevel == 3)
            player.bounceForce = 2.1f;

        if (dragUpgradeLevel == 4)
            player.bounceForce = 2.07f;

        if (dragUpgradeLevel == 5)
            player.bounceForce = 2f;
    }
}
