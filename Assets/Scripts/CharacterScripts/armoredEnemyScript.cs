using System.Collections;
using UnityEngine;
using Assets.Scripts;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using UnityEngine.UI;

public class armoredEnemyScript : MonoBehaviour {
	public bool AllowedToMove = false, Stunned = false;
	public float stun, maxStun = 20f, distanceToAttack;
	float stunUI_InitWidth;
	float distanceToPlayer;
	public GameObject stunUI,xp; 
	enemyCommonScript _CommonProperties;
	Animator anmtr;
	Rigidbody2D _RigidBody;
	PlayerFightSystem player;
	GameObject detector;
	void Start() {
		detector = transform.Find("detector").gameObject;
		stunUI_InitWidth = stunUI.transform.localScale.x;
		stun = maxStun;
		_CommonProperties = GetComponent<enemyCommonScript>();
		_RigidBody = GetComponent<Rigidbody2D>();
		anmtr = GetComponent<Animator>();
		player = FindObjectOfType<PlayerFightSystem>();
	}
	void Update() 
	{

		if (_CommonProperties.Awake && _CommonProperties.health > 0 && !Stunned)
		{
			distanceToPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
			AllowedToMove = !(distanceToPlayer <= distanceToAttack);
			if (AllowedToMove)
			{
				anmtr.SetBool("smash", false);
				_RigidBody.velocity = new Vector2(_CommonProperties.Speed, 0);
			}
			else {
				Stop();
				anmtr.SetBool("smash", true); 
			}
		}
		else Stop();

		if(CrossPlatformInputManager.GetButtonDown("Interaction") &&
			player.stamina == player.maxStamina && player.playerStyle == Styles.idol)
		{
				player.gameObject.SetActive(false);
				anmtr.SetBool("finisher", true);
		}


	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !_CommonProperties.Awake)
		{
			Move();
			detector.SetActive(false);
		}
		if (other.gameObject.CompareTag("weapon"))
        {
			if (Stunned)
			{
				_CommonProperties.health -= _CommonProperties.damageTaken;
				if (_CommonProperties.health <= 0)
                {
					_CommonProperties.health = 0;
					anmtr.Play("die");
                }
			}
			else
			{
				stun -= _CommonProperties.damageTaken;
				if(stun <= 0)
                {
					stun = 0;
					Stunned = true;
					Stop();
					anmtr.SetBool("stunned", true);
				}
				SetStunUI();
			}
			player.stamina += _CommonProperties.damageTaken;
		}
	}
	void OnTriggerStay2D(Collider2D col)
    {
		if (col.CompareTag("Player") && Stunned && player.stamina == player.maxStamina)
			FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
				.FirstOrDefault().GetComponent<Image>().color = Color.white;
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
			FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
				.FirstOrDefault().GetComponent<Image>().color = Color.clear;
	}
	public void SetStunUI()
	{
		stunUI.transform.localScale = new Vector3(stunUI_InitWidth * (stun / maxStun),
			stunUI.transform.localScale.y, stunUI.transform.localScale.z);
	}
	public void death()
    {
		if (!player.gameObject.activeSelf)
		{ 
			player.gameObject.SetActive(true);
			player.SetAnimatorStyleParameter();
			FindObjectOfType<PlayerBehaviour>().allowToMove = true;
		}
		Stop();
		gameObject.SetActive(false);
		xp.transform.parent = null;
		xp.SetActive(true);
    }
	public void Move()
    {
		AllowedToMove = true;
		_CommonProperties.Awake = true;
		Stunned = false;
		anmtr.SetBool("awake", true);
		anmtr.SetBool("stunned", false);
	}
	public void Stop()
    {
		anmtr.SetBool("smash", false);
		_RigidBody.velocity = Vector2.zero;
		AllowedToMove = false;
    }
	public void RefillStunnabilityBar()
	{
		stun = maxStun;
		SetStunUI();
	}
}
