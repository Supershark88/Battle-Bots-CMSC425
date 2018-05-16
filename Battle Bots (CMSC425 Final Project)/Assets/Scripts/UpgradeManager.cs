using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeManager : MonoBehaviour {

    private TextMeshProUGUI scrapText;
    private TextMeshProUGUI noScrapText;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI damageText;
    private TextMeshProUGUI speedText;
    private TextMeshProUGUI shooterLevelUpText;
    private TextMeshProUGUI shooterHealthText;
    private TextMeshProUGUI shooterDamageText;
    private TextMeshProUGUI shooterSpeedText;
    private TextMeshProUGUI shooterAccuracyText;
    private TextMeshProUGUI brawlerLevelUpText;
    private TextMeshProUGUI brawlerHealthText;
    private TextMeshProUGUI brawlerDamageText;
    private TextMeshProUGUI brawlerSpeedText;
    private TextMeshProUGUI engineerLevelUpText;
    private TextMeshProUGUI engineerHealthText;
    private TextMeshProUGUI engineerHealTimeText;
    private TextMeshProUGUI engineerSpeedText;
    private TextMeshProUGUI engineerHealText;

    private int scrap;
    private int playerHealth;
    private int playerDamage;
    private int playerSpeed;

    private int shooterLevel;
    private int shooterHealth;
    private int shooterDamage;
    private int shooterSpeed;
    private float shooterAccuracy;

    private int brawlerLevel;
    private int brawlerHealth;
    private int brawlerDamage;
    private int brawlerSpeed;

    private int engineerLevel;
    private int engineerHealth;
    private int engineerSpeed;
    private float engineerHealTime;
    private int engineerHeal;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        scrap = PlayerPrefs.GetInt("Scrap");
        scrapText = gameObject.transform.Find("Scrap").GetComponent<TextMeshProUGUI>();
        noScrapText = gameObject.transform.Find("NoScrap").GetComponent<TextMeshProUGUI>();
        noScrapText.text = "Spend 100 Scrap to upgrade";
        scrapText.text = "Scrap: " + scrap;

        // Player Upgrades
        playerHealth = PlayerPrefs.GetInt("PlayerHealth");
        playerDamage = PlayerPrefs.GetInt("PlayerDamage");
        playerSpeed = PlayerPrefs.GetInt("PlayerSpeed");

        healthText = gameObject.transform.Find("PlayerUpgrades").Find("PlayerHealth").Find("HealthValue").GetComponent<TextMeshProUGUI>();
        healthText.text = "" + playerHealth + "(+5)";
        damageText = gameObject.transform.Find("PlayerUpgrades").Find("PlayerDamage").Find("DamageValue").GetComponent<TextMeshProUGUI>();
        damageText.text = "" + playerDamage + "(+1)";
        speedText = gameObject.transform.Find("PlayerUpgrades").Find("PlayerSpeed").Find("SpeedValue").GetComponent<TextMeshProUGUI>();
        speedText.text = "" + playerSpeed + "(+1)";


        // Squad Upgrades
        shooterLevel = PlayerPrefs.GetInt("ShooterLevel");
        shooterHealth = 10 + 5 * shooterLevel;
        shooterDamage = 1 + shooterLevel;
        shooterSpeed = 3 + shooterLevel;
        shooterAccuracy = .50f + .05f * shooterLevel;

        shooterLevelUpText = gameObject.transform.Find("ShooterUpgrades").Find("ShooterLevel").Find("LevelValue").GetComponent<TextMeshProUGUI>();
        shooterLevelUpText.text = "" + shooterLevel;
        shooterHealthText = gameObject.transform.Find("ShooterUpgrades").Find("Health").GetComponent<TextMeshProUGUI>();
        shooterHealthText.text = "Health: " + shooterHealth;
        shooterDamageText = gameObject.transform.Find("ShooterUpgrades").Find("DMG").GetComponent<TextMeshProUGUI>();
        shooterDamageText.text = "DMG: " + shooterDamage;
        shooterSpeedText = gameObject.transform.Find("ShooterUpgrades").Find("SPD").GetComponent<TextMeshProUGUI>();
        shooterSpeedText.text = "SPD: " + shooterSpeed;
        shooterAccuracyText = gameObject.transform.Find("ShooterUpgrades").Find("ACC").GetComponent<TextMeshProUGUI>();
        shooterAccuracyText.text = "ACC: " + shooterAccuracy * 100 + "%";

        brawlerLevel = PlayerPrefs.GetInt("BrawlerLevel");
        brawlerHealth = 25 + 10 * brawlerLevel;
        brawlerDamage = 3 + 3 * brawlerLevel;
        brawlerSpeed = 2 + 1 * brawlerLevel; 

        brawlerLevelUpText = gameObject.transform.Find("BrawlerUpgrades").Find("BrawlerLevel").Find("LevelValue").GetComponent<TextMeshProUGUI>();
        brawlerLevelUpText.text = "" + brawlerLevel;
        brawlerHealthText = gameObject.transform.Find("BrawlerUpgrades").Find("Health").GetComponent<TextMeshProUGUI>();
        brawlerHealthText.text = "Health: " + brawlerHealth;
        brawlerDamageText = gameObject.transform.Find("BrawlerUpgrades").Find("DMG").GetComponent<TextMeshProUGUI>();
        brawlerDamageText.text = "DMG: " + brawlerDamage;
        brawlerSpeedText = gameObject.transform.Find("BrawlerUpgrades").Find("SPD").GetComponent<TextMeshProUGUI>();
        brawlerSpeedText.text = "SPD: " + brawlerSpeed;

        engineerLevel = PlayerPrefs.GetInt("EngineerLevel");
        engineerHealth = 20 + 5 * engineerLevel;
        engineerSpeed = 4 + engineerLevel;
        engineerHealTime = 8 - .5f * engineerLevel;
        engineerHeal = 5 + engineerLevel;

        engineerLevelUpText = gameObject.transform.Find("EngineerUpgrades").Find("EngineerLevel").Find("LevelValue").GetComponent<TextMeshProUGUI>();
        engineerLevelUpText.text = "" + engineerLevel;
        engineerHealthText = gameObject.transform.Find("EngineerUpgrades").Find("Health").GetComponent<TextMeshProUGUI>();
        engineerHealthText.text = "Health: " + engineerHealth;
        engineerSpeedText = gameObject.transform.Find("EngineerUpgrades").Find("SPD").GetComponent<TextMeshProUGUI>();
        engineerSpeedText.text = "SPD: " + engineerSpeed;
        engineerHealTimeText = gameObject.transform.Find("EngineerUpgrades").Find("HEALTime").GetComponent<TextMeshProUGUI>();
        engineerHealTimeText.text = "HEALTIME: " + engineerHealTime;
        engineerHealText = gameObject.transform.Find("EngineerUpgrades").Find("HEAL").GetComponent<TextMeshProUGUI>();
        engineerHealText.text = "HEAL: " + engineerHeal;
    }

    public void AddPlayerHealth()
    {
        if (scrap >= 100)
        {
            scrap = scrap - 100;
            playerHealth = playerHealth + 5;
            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("PlayerHealth", playerHealth);
            scrapText.text = "Scrap: " + scrap;
            healthText.text = playerHealth.ToString() + "(+5)";
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void AddPlayerDamage()
    {
        if (scrap >= 100)
        {
            scrap = scrap - 100;
            playerDamage = playerDamage + 1;
            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("PlayerDamage", playerDamage);
            scrapText.text = "Scrap: " + scrap;
            damageText.text = playerDamage.ToString() + "(+1)";
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void AddPlayerSpeed()
    {
        if (scrap >= 100)
        {
            scrap = scrap - 100;
            playerSpeed = playerSpeed + 1;
            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("PlayerSpeed", playerSpeed);
            scrapText.text = "Scrap: " + scrap;
            speedText.text = playerSpeed.ToString() + "(+1)";
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void levelUpShooter(){
        if(scrap >= 100)
        {
            scrap = scrap - 100;
            shooterLevel = shooterLevel + 1;

            shooterHealth = 10 + 5 * shooterLevel;
            shooterDamage = 1 + shooterLevel;
            shooterSpeed = 3 + shooterLevel;
            shooterAccuracy = .50f + .05f * shooterLevel;

            shooterHealthText.text = "Health: " + shooterHealth;
            shooterDamageText.text = "DMG: " + shooterDamage;
            shooterAccuracyText.text = "ACC: " + shooterAccuracy * 100 + "%";
            shooterSpeedText.text = "SPD: " + shooterSpeed;

            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("ShooterLevel", shooterLevel);
            scrapText.text = "Scrap: " + scrap;
            shooterLevelUpText.text = shooterLevel.ToString();
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void levelUpBrawler(){
        if(scrap >= 100)
        {
            scrap = scrap - 100;
            brawlerLevel = brawlerLevel + 1;

            brawlerHealth = 25 + 10 * brawlerLevel;
            brawlerDamage = 3 + 3 * brawlerLevel;
            brawlerSpeed = 2 + brawlerLevel;

            brawlerHealthText.text = "Health: " + brawlerHealth;
            brawlerDamageText.text = "DMG: " + brawlerDamage;
            brawlerSpeedText.text = "SPD: " + brawlerSpeed;

            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("BrawlerLevel", brawlerLevel);
            scrapText.text = "Scrap: " + scrap;
            brawlerLevelUpText.text = brawlerLevel.ToString();
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void levelUpEngineer(){
        if(scrap >= 100)
        {
            scrap = scrap - 100;
            engineerLevel = engineerLevel + 1;

            engineerHealth = 20 + 5 * engineerLevel;
            engineerSpeed = 4 + engineerLevel;
            engineerHealTime = 8 - .5f * engineerLevel;
            engineerHeal = 5 + engineerLevel;

            engineerHealthText.text = "Health: " + engineerHealth;
            engineerSpeedText.text = "SPD: " + engineerSpeed;
            engineerHealTimeText.text = "HEALTIME: " + engineerHealTime;
            engineerHealText.text = "HEAL: " + engineerHeal;

            PlayerPrefs.SetInt("Scrap", scrap);
            PlayerPrefs.SetInt("EngineerLevel", engineerLevel);
            scrapText.text = "Scrap: " + scrap;
            engineerLevelUpText.text = engineerLevel.ToString();
        }
        else
        {
            noScrapText.text = "Not Enough Scrap!";
        }
    }

    public void ContinueToGame()
    {
        int level = PlayerPrefs.GetInt("Level") + 1;
        SceneManager.LoadScene("Level " + level);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
