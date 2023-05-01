using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThrowingKnifeScript : MonoBehaviour {

	public enum type
    {
		knife,axe,wave
    }
	public type weaponType;
	Rigidbody2D rgd;
	Animator anmtr;
	PlayerFightSystem player;
	public float hspeed = 25f, vspeed = 15f, damage = 5f;
	bossFightScript boss;

	void Start () {
		rgd = GetComponent<Rigidbody2D>();
		player = FindObjectOfType<PlayerFightSystem>();
		boss = FindObjectOfType<bossFightScript>();
		if (GetComponent<Animator>() != null)
			anmtr = GetComponent<Animator>();
		if (boss.transform.localScale.x < 0)
		{
			hspeed *= -1;
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
		}

		if (weaponType == type.axe)
			rgd.velocity = new Vector2(hspeed, vspeed);
	}
	
	void Update () {
		if (weaponType != type.axe)
			rgd.velocity = new Vector2(hspeed, 0);
	}
	void OnBecameInvisible()
    {
		if (weaponType != type.axe)
			gameObject.SetActive(false);
    }
	void OnTriggerEnter2D(Collider2D col)
    {
		if (col.CompareTag("Surface"))
		{
			if (weaponType == type.axe)
			{
				anmtr.enabled = false;
				rgd.velocity = Vector2.zero;
				rgd.gravityScale = 0f;
			}
		}
		if (col.CompareTag("Player"))
			if(rgd.gravityScale == 0f && weaponType == type.axe)
			{
				player.hasAxe = true;
				player.axeSpriteRndrr.color = new Color(player.axeSpriteRndrr.color.r, player.axeSpriteRndrr.color.g, player.axeSpriteRndrr.color.b, 255f);
				gameObject.SetActive(false);
				player.axeLogo.SetActive(true);
			}
			else
				player.currentHealth -= damage;
	}
}
