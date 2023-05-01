using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class enemyCommonScript : MonoBehaviour {

	public float health,maxHealth = 10f,damageTaken = 0f;
	public GameObject currentHealthUI;
	public bool GetThrown = false, Awake = false;
	public float Speed = 5f, DistanceToAttack = 35f;
	PlayerBehaviour _Player;
	float healthUIInitWidth;

	void Start () {
		Awake = false;
		GetThrown = false;
		if(currentHealthUI != null)
			healthUIInitWidth = currentHealthUI.transform.localScale.x;
        health = maxHealth;
		_Player = FindObjectOfType<PlayerBehaviour>();
	}

	void Update () {
		if (currentHealthUI != null)
		{
			currentHealthUI.transform.localScale = new Vector3(health / maxHealth * healthUIInitWidth, currentHealthUI.transform.localScale.y, currentHealthUI.transform.localScale.z);
			if (health <= 0)
				health = 0;
		}
        if (Awake)
        {
			var TrueDirection = Mathf.Sign(_Player.transform.position.x - transform.position.x);
			Speed = Mathf.Abs(Speed) * TrueDirection;
			transform.localScale = new Vector2(TrueDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y);
		}
	}
}
