using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPoint : MonoBehaviour {
    public float addingHealth = 10f;
    PlayerFightSystem playerScr;
	// Use this for initialization
	void Start () {
        playerScr = FindObjectOfType<PlayerFightSystem>();
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && playerScr.currentHealth != playerScr.maxHealth)
        {
            if (playerScr.currentHealth + addingHealth < playerScr.maxHealth)
                playerScr.currentHealth += addingHealth;
            else playerScr.currentHealth = playerScr.maxHealth;
            gameObject.SetActive(false);
            playerScr.axeblood.SetActive(false);
            playerScr.swordblood.SetActive(false);
        }
    }           
}
