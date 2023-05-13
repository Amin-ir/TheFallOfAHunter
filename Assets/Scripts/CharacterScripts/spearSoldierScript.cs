using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

public class spearSoldierScript : MonoBehaviour {

	public bool Stunned = false, AllowedToMove = false;
	public int SecondsToRevive = 3;
	Rigidbody2D _RigidBody;
	Animator _Animator;
	enemyCommonScript _CommonProperties;
	PlayerFightSystem _Player;
    List<float> PlayerSpecialAttacksDamages = new List<float>();

	public GameObject xp;
	GameObject detector;
	void Start() {
		detector = transform.Find("detector").gameObject;
		_RigidBody = GetComponent<Rigidbody2D>();
		_CommonProperties = GetComponent<enemyCommonScript>();
		_Animator = GetComponent<Animator>();
		_Player = FindObjectOfType<PlayerFightSystem>();
		PlayerSpecialAttacksDamages = new List<float> { _Player.bombDamage, _Player.stormDamage, _Player.lightningDamage};
	}

	void Update()
	{
		if (_CommonProperties.Awake && _CommonProperties.health > 0 && !Stunned)
		{
			var DistanceToPlayer = Mathf.Abs(transform.position.x - _Player.transform.position.x);

			if (DistanceToPlayer > _CommonProperties.DistanceToAttack)
				if (AllowedToMove)
					_RigidBody.velocity = new Vector2(_CommonProperties.Speed, 0);
				else Stop();
			else
				_Animator.SetBool("attack", true);

		}
		else Stop();
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
			{
				_Animator.SetBool("awake", true);
				detector.SetActive(false);
			}
		if (col.CompareTag("weapon"))
		{
			if (!Stunned)
				if (PlayerSpecialAttacksDamages.Contains(_CommonProperties.damageTaken))
					StartCoroutine(GetStunned(SecondsToRevive));
				else
				{
					_Player.stamina += _CommonProperties.damageTaken;
					_Animator.SetBool("defense", true);
					_RigidBody.velocity = Vector2.zero;
					_CommonProperties.Awake = false;
				}
			else
			{
				_Player.stamina += _CommonProperties.damageTaken;
				if (_Player.stamina >= _Player.maxStamina)
					FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "SpecialAttack")
						.FirstOrDefault().gameObject.SetActive(true);
				if (_CommonProperties.health < 0)
					Die();
			}
		}
	}
	IEnumerator GetStunned(int i)
	{
		AllowedToMove = false;
		Stunned = true;
		_Animator.SetBool("stunned", true);
		yield return new WaitForSeconds(i);
		Move();
	}
	void Die()
    {
		Stop();
		_CommonProperties.health = 0;
		_Animator.Play("die");
	}
	void Deactive()
	{
		xp.transform.parent = null;
		xp.SetActive(true);
		gameObject.SetActive(false);
	}
	public void Move()
	{
		_CommonProperties.Awake = true;
		AllowedToMove = true;
		Stunned = false;
		_Animator.SetBool("attack", false);
		_Animator.SetBool("defense", false);
		_Animator.SetBool("stunned", false);
    }
	void Stop()
    {
		_RigidBody.velocity = Vector2.zero;
	}
}
