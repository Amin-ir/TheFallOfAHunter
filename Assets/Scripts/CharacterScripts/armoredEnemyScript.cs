using System.Collections;
using UnityEngine;
using Assets.Scripts;
using UnityStandardAssets.CrossPlatformInput;

public class armoredEnemyScript : MonoBehaviour {
	public bool AllowedToMove = false, Stunned = false;
	public float stun, maxStun = 20f;
	float stunUI_InitWidth;
	public int secondsToPull = 5,secondsToUnstun = 5;
	public GameObject stunUI,xp; 
	enemyCommonScript _CommonProperties;
	Animator anmtr;
	Rigidbody2D _RigidBody;
	PlayerFightSystem player;
	// Use this for initialization
	void Start() {
		stunUI_InitWidth = stunUI.transform.localScale.x;
		stun = maxStun;
		_CommonProperties = GetComponent<enemyCommonScript>();
		_RigidBody = GetComponent<Rigidbody2D>();
		anmtr = GetComponent<Animator>();
		player = FindObjectOfType<PlayerFightSystem>();
	}
	// Update is called once per frame
	void Update() 
	{
		if (_CommonProperties.Awake && _CommonProperties.health > 0 && !Stunned)
			if (AllowedToMove)
				_RigidBody.velocity = new Vector2(_CommonProperties.Speed, 0);
			else StartCoroutine(smash(secondsToPull));
		else Stop();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !_CommonProperties.Awake)
			Move();
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
				stunUI.transform.localScale = new Vector3(stunUI_InitWidth * (stun / maxStun),
					stunUI.transform.localScale.y, stunUI.transform.localScale.z);
				if(stun <= 0)
                {
					stun = 0;
					Stunned = true;
					Stop();
					anmtr.SetBool("stunned", true);
					StartCoroutine(UnStun(secondsToUnstun));
				}
			}
			player.stamina += _CommonProperties.damageTaken;
		}
	}
	void OnTriggerStay2D(Collider2D col)
    {
		if (col.CompareTag("Player") && CrossPlatformInputManager.GetButtonDown("Interaction") && Stunned)
			if (player.stamina == player.maxStamina && player.playerStyle == Styles.idol)
			{
				player.gameObject.SetActive(false);
				anmtr.SetBool("finisher", true);
			}
	}
	IEnumerator smash(int i)
    {
		Stop();
		anmtr.SetBool("smash", true);
		yield return new WaitForSeconds(i);
		anmtr.SetBool("smash", false);
		Move();
	}
	IEnumerator UnStun(int i)
    {
		yield return new WaitForSeconds(i);
		stun = maxStun;
		Move();
		anmtr.SetBool("stunned", false);
	}
	public void death()
    {
		if (player.gameObject.activeSelf == false)
			player.gameObject.SetActive(true);
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
	}
	public void Stop()
    {
		_RigidBody.velocity = Vector2.zero;
		AllowedToMove = false;
    }
}
