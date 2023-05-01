using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class upgradePanelScript : MonoBehaviour {
    PlayerFightSystem player;
    PlayerBehaviour playerScr;
    int axeUpgrade = 0, swordUpgrade = 0, armorUpgrade = 0;
    public Text CurrentXp,currentArmor, currentSword, currentAxe;
    public Text currentArmorLevel, currentSwordLevel,currentAxeLevel, notEnoughXp;

    public Image AxeGreenBar, SwordGreenBar, ArmorBar;

    void Start()
    {
        if (PlayerPrefs.HasKey("armorUpgrade"))
            armorUpgrade = PlayerPrefs.GetInt("armorUpgrade");
        if (PlayerPrefs.HasKey("axeUpgrade"))
            axeUpgrade = PlayerPrefs.GetInt("axeUpgrade");
        if (PlayerPrefs.HasKey("swordUpgrade"))
            swordUpgrade = PlayerPrefs.GetInt("swordUpgrade");
        player = FindObjectOfType<PlayerFightSystem>(); 
        playerScr = player.gameObject.GetComponent<PlayerBehaviour>();

        displayInfo();
    }
    void displayInfo()
    {
        CurrentXp.text = playerScr.xp.ToString();
        currentArmor.text = (player.maxHealth).ToString();
        currentAxe.text = (player.HAxe + player.LAxe).ToString();
        currentSword.text = (player.HSword + player.LSword).ToString();
        currentArmorLevel.text = armorUpgrade.ToString();
        currentAxeLevel.text = axeUpgrade.ToString();
        currentSwordLevel.text = swordUpgrade.ToString();
        foreach (var item in new[] { currentArmorLevel, currentSwordLevel, currentAxeLevel } )
        {
            var level = Convert.ToInt32(item.text);
            item.transform.Find("nextValue").GetComponent<Text>().text = (level + 1).ToString();
            item.transform.Find("cost").GetComponent<Text>().text = (CustomTools.upgrades[level + 1] * 1000).ToString();
            item.transform.Find("additionText").GetComponent<Text>().text = (CustomTools.upgrades[level + 1] * 100).ToString() + "%";
        }
    }
    public void upgradeArmor()
    {
        var price = Convert.ToDouble(currentArmorLevel.transform.Find("cost").GetComponent<Text>().text);
        if (checkEnoughXP((float)price, playerScr.xp) == 1)
        {
            playerScr.xp -= (int)price;
            playerScr.armorUpgradeTemp++;
            armorUpgrade++;
            var reward = ((price / 1000) + 1);
            player.maxHealth = (float) (reward * CustomTools.init_maxHealth);
            player.currentHealth = player.maxHealth;
            player.LFist = (float)(reward * CustomTools.init_LFist);
            player.HFist = (float)(reward * CustomTools.init_HFist);
            player.matchArmorWithUpgrades();
            displayInfo();
        }
    }
    public void upgradeSword()
    {
        var price = Convert.ToDouble(currentSwordLevel.transform.Find("cost").GetComponent<Text>().text);
        if (checkEnoughXP((float)price, playerScr.xp) == 1)
        {
            playerScr.xp -= (int)price;
            playerScr.swordUpgradeTemp++;
            swordUpgrade++;
            var reward = ((price / 1000) + 1);
            player.LSword = (float)(reward * CustomTools.init_LSword);
            player.HSword = (float)(reward * CustomTools.init_HSword);
            displayInfo();
        }
    }
    public void upgradeAxe()
    {
        var price = Convert.ToDouble(currentAxeLevel.transform.Find("cost").GetComponent<Text>().text);
        if (checkEnoughXP((float)price, playerScr.xp) == 1)
        {
            playerScr.xp -= (int)price;
            playerScr.axeUpgradeTemp++;
            axeUpgrade++;
            var reward = ((price / 1000) + 1);
            player.LAxe = (float)(reward * CustomTools.init_LAxe);
            player.HAxe = (float)(reward * CustomTools.init_HAxe);
            player.throwingAxeDamage = (float)(reward * CustomTools.init_throwingAxe);
            displayInfo();
        }
    }
    public void done()
    {
        displayInfo();
        notEnoughXp.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    int checkEnoughXP(float price, float xp)
    {
        if (xp < price)
        {
            notEnoughXp.gameObject.SetActive(true);
            return 0;
        }
        else return 1;
    }
}
