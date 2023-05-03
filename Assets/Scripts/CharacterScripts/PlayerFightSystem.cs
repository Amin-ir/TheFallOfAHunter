using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

public class PlayerFightSystem : MonoBehaviour {
	
#pragma warning disable 0219
    public Styles playerStyle;
	public GameObject axeblood, swordblood,axeClone,axe,axeLogo,arrow;
	public GameObject rightShoulderArmor, leftShoulderArmor, foreArmArmor; 
	public Transform axeThrownPosition,shootArrowPosition;
    public Animator playerAnimator;
	PlayerBehaviour playerScr;
    enemyCommonScript[] _EnemiesInTheScene;
	public float maxHealth = 100f,stamina,maxStamina = 100f, currentHealth,healthBarInitialWidth,staminaBarInitialWidth;
	public float LFist = 1f, HFist = 2.5f, LAxe = 3f, HAxe = 5f,throwingAxeDamage = 7f, LSword = 7f, HSword = 9f,screenFadeRate = 5f,
	stormDamage = 30f,bombDamage = 20f,lightningDamage = 40f,arrowDamage = 5f; // L<name> : Light Attack && H<name> : Heavy Attack
	public bool hasAxe = true, dodge = false, AllowedToUseSword = false, mayPlayGetHitAnimation = true;
	public int arrowCount = 5;
	public SpriteRenderer axeSpriteRndrr;
    public Image currHealthUI,weaponIcon,currStaminaUI;
    public Sprite axeIcon, fistIcon, swordIcon;
	public Text arrowCountText;
	RectTransform theBarRectTransform, staminaRectTransform;
	public float RollForceX = 5f, RollForceY = -2f;
	//Initialization & retrieving PlayerPrefs values
    void Start () {
		
		playerScr = GetComponent<PlayerBehaviour>();
        theBarRectTransform = currHealthUI.GetComponent<RectTransform>();
		staminaRectTransform = currStaminaUI.GetComponent<RectTransform>();
		axeSpriteRndrr = axe.GetComponent<SpriteRenderer> ();
		_EnemiesInTheScene = Resources.FindObjectsOfTypeAll<enemyCommonScript>();
		playerAnimator = GetComponent<Animator>();

		GetPlayerPrefs();

		currentHealth = maxHealth;
		arrowCountText.text = arrowCount.ToString() + " X";
		playerStyle = Styles.idol;
        weaponIcon.sprite = fistIcon;
		SetAnimatorStyleParameter();
		stamina = 0f;
		healthBarInitialWidth = theBarRectTransform.rect.width;
		staminaBarInitialWidth = staminaRectTransform.rect.width;
	}

    void FixedUpdate () {
        if (currentHealth <= 0)
			StartCoroutine(death());
		if (stamina >= maxStamina)
		{
			stamina = maxStamina;
			FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "SpecialAttack").FirstOrDefault()
					.GetComponent<Image>().color = Color.white;
		}
		playerAnimator.SetBool("heavy",false);
		playerAnimator.SetBool("light",false);

        theBarRectTransform.sizeDelta = new Vector2((currentHealth/maxHealth) * healthBarInitialWidth, theBarRectTransform.sizeDelta.y);
		staminaRectTransform.sizeDelta = new Vector2 ((stamina/maxStamina) * staminaBarInitialWidth, staminaRectTransform.sizeDelta.y);

		if(CrossPlatformInputManager.GetButtonDown("Roll") && playerScr.allowToMove)
		{
			playerScr.allowToMove = false;
			playerAnimator.SetBool("dodge", true);
			RollForceX = Mathf.Abs(RollForceX) * (-Mathf.Sign(transform.localScale.x));
			playerScr._PlayerRigidBody.AddForce(new Vector2(RollForceX, RollForceY), ForceMode2D.Impulse);
		}


		if (CrossPlatformInputManager.GetButtonDown("Dodge"))
			playerAnimator.SetBool("dodgeDown", true);
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
			lightAttack();
		if (CrossPlatformInputManager.GetButtonDown("Fire2"))
			heavyAttack();

		if (CrossPlatformInputManager.GetButtonDown("Throw"))
			throwables();
		if (CrossPlatformInputManager.GetButtonDown("Fire3") && stamina == maxStamina)
		{
			staminaAttacks();
			foreach (var item in _EnemiesInTheScene)
				item.GetThrown = true;
		}
    }
	IEnumerator waitFunction(int wait = 2){
		playerScr.allowToMove = false;
		yield return new WaitForSeconds(wait);
		playerScr.allowToMove = true;
	}
    IEnumerator death()
    {
        playerScr.allowToMove = false;
        playerScr.playerAnimator.Play("Die");
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		FindObjectOfType<blackCurtainScript>().unFade();
        yield return new WaitForSeconds(7);
		FindObjectOfType<ButtonsScript>().continueGame();
    }
	void GetPlayerPrefs()
	{
		if (PlayerPrefs.HasKey("armorUpgrade"))
		{
			var level = PlayerPrefs.GetInt("armorUpgrade");
			var IncreasingPercentage = CustomTools.upgrades[level] + 1;
			playerScr.armorUpgradeTemp = level;
			maxHealth = CustomTools.init_maxHealth * IncreasingPercentage;
			LFist = CustomTools.init_LFist * IncreasingPercentage;
			HFist = CustomTools.init_HFist * IncreasingPercentage;
			matchArmorWithUpgrades();
		}
		if (PlayerPrefs.HasKey("axeUpgrade"))
		{
			var level = PlayerPrefs.GetInt("axeUpgrade");
			var IncreasingPercentage = CustomTools.upgrades[level] + 1;
			playerScr.axeUpgradeTemp = level;
			LAxe = CustomTools.init_LAxe * IncreasingPercentage;
			HAxe = CustomTools.init_HAxe * IncreasingPercentage;
		}
		if (PlayerPrefs.HasKey("swordUpgrade"))
		{
			var level = PlayerPrefs.GetInt("swordUpgrade");
			var IncreasingPercentage = CustomTools.upgrades[level] + 1;
			playerScr.swordUpgradeTemp = level;
			LSword = CustomTools.init_LSword * IncreasingPercentage;
			HSword = CustomTools.init_HSword * IncreasingPercentage;
		}
		if (PlayerPrefs.HasKey("arrow"))
			arrowCount = PlayerPrefs.GetInt("arrow");
		else arrowCount = 0;
	}
	public void endAnim()
	{
		playerAnimator.SetBool ("staminaAttack", false);
		Time.timeScale = 1f;
		playerScr.allowToMove = true;
	}
	public void throwAxe()
	{
		mayPlayGetHitAnimation = false;
		if (_EnemiesInTheScene != null)
			foreach (var item in _EnemiesInTheScene)
				item.damageTaken = throwingAxeDamage;
		playerStyle = Styles.idol;
		weaponIcon.sprite = fistIcon;
		SetAnimatorStyleParameter();
		GameObject clone = Instantiate (axeClone, axeThrownPosition.position, Quaternion.identity) as GameObject;
		playerAnimator.SetBool ("throwAxe", false);
		axeSpriteRndrr.color = new Color(axeSpriteRndrr.color.r, axeSpriteRndrr.color.g, axeSpriteRndrr.color.b, 0);
	}
	public void dodgeDone()
	{
		if (playerScr._PlayerRigidBody.velocity == Vector2.zero)
		{
			playerScr.idol = true;
			playerScr.allowToMove = true;
			mayPlayGetHitAnimation = true;
			playerAnimator.SetBool("dodge", false);
			playerAnimator.SetBool("dodgeDown", false);
		}
	}
	public void shootArrow()
    {
		mayPlayGetHitAnimation = false;
		GameObject clone = Instantiate(arrow, shootArrowPosition.position, Quaternion.identity) as GameObject;
		StartCoroutine(waitFunction(1));
    }
	public void setStyle()
    {
		mayPlayGetHitAnimation = false;
		playerScr._PlayerRigidBody.velocity = Vector2.zero;
		playerScr.playerAnimator.SetBool("idol", true);

		if (playerStyle != Styles.sword)
			playerStyle++;
		else playerStyle = Styles.idol;

        switch (playerStyle)
        {
            case Styles.idol:
				weaponIcon.sprite = fistIcon;
                break;
            case Styles.axe:
				if (hasAxe)
				{
					weaponIcon.sprite = axeIcon;
				}
				else setStyle();
				break;
            case Styles.sword:
				if (AllowedToUseSword)
				{
					weaponIcon.sprite = swordIcon;
				}
				else setStyle();
				break;
        }
		SetAnimatorStyleParameter();
		StartCoroutine(waitFunction());

    }
	public void throwables()
    {
		mayPlayGetHitAnimation = false;
		switch (playerStyle)
		{
			case Styles.idol:
				if (arrowCount > 0)
				{
					arrowCount--;
					arrowCountText.text = arrowCount.ToString() + " X";
					playerAnimator.Play("shootBow");
					if (_EnemiesInTheScene != null)
						foreach (var item in _EnemiesInTheScene)
							item.damageTaken = arrowDamage;
				}
				break;
			case Styles.axe:
				if (hasAxe)
				{
					playerAnimator.SetBool("throwAxe", true);
					hasAxe = false;
					axeLogo.SetActive(false);
				}
				break;
		}
	}
	public void staminaAttacks()
    {
		mayPlayGetHitAnimation = false;
		stamina = 0f;
		Time.timeScale = 0.3f;
		float damage = 0f;
		if (_EnemiesInTheScene != null)
		{
			switch (playerStyle)
			{
				case Styles.axe:
					damage = stormDamage;
					break;
				case Styles.idol:
					damage = bombDamage;
					break;
				case Styles.sword:
					damage = lightningDamage;
					break;
			}
			foreach (var item in _EnemiesInTheScene)
				item.damageTaken = damage;
		}
		playerAnimator.SetBool("staminaAttack", true);
		playerScr.allowToMove = false;
		FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "SpecialAttack")
			.FirstOrDefault().GetComponent<Image>().color = Color.clear;
	}
	public void heavyAttack()
    {
		mayPlayGetHitAnimation = false;
		playerAnimator.SetBool("heavy", true);
		float damage = 0f;
		if (_EnemiesInTheScene != null)
		{
			switch (playerStyle)
			{
				case Styles.idol:
					damage = HFist;
					break;
				case Styles.axe:
					damage = HAxe;
					break;
				case Styles.sword:
					damage = HSword;
					break;
			}
			foreach (var item in _EnemiesInTheScene)
				item.damageTaken = damage;
		}
		StartCoroutine(waitFunction());
	}
	public void lightAttack()
	{
		mayPlayGetHitAnimation = false;
		playerAnimator.SetBool("light", true);
		float damage = 0f;
		if (_EnemiesInTheScene != null)
		{
			switch (playerStyle)
			{
				case Styles.idol:
					damage = LFist;
					break;
				case Styles.axe:
					damage = LAxe;
					break;
				case Styles.sword:
					damage = LSword;
					break;
			}
			foreach (var item in _EnemiesInTheScene)
				item.damageTaken = damage;
		}
	}
	public void matchArmorWithUpgrades()
	{
		switch (playerScr.armorUpgradeTemp)
		{
			case 1:
				leftShoulderArmor.SetActive(true);
				break;
			case 2:
				leftShoulderArmor.SetActive(true);
				rightShoulderArmor.SetActive(true);
				break;
			case 3:
				leftShoulderArmor.SetActive(true);
				rightShoulderArmor.SetActive(true);
				foreArmArmor.SetActive(true);
				break;
		}
	}
	public void SetAnimatorStyleParameter()
	{
		switch (playerStyle)
		{
			case Styles.idol:
				playerAnimator.SetInteger("Style", 1);
				break;
			case Styles.axe:
				playerAnimator.SetInteger("Style", 2);
				break;
			case Styles.sword:
				playerAnimator.SetInteger("Style", 3);
				break;
		}
	}
}
