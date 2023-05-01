using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeaponScript : MonoBehaviour
{

    PlayerFightSystem player;
    PlayerBehaviour playerBehave;
    enemyCommonScript enemyCommonScr;
    public float damageGiven = 2f;
    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerFightSystem>();
        playerBehave = player.GetComponent<PlayerBehaviour>();
        if(GetComponent<enemyCommonScript>() != null)
            enemyCommonScr = GetComponentInParent<enemyCommonScript>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.currentHealth -= damageGiven;
            if (player.mayPlayGetHitAnimation)
                playerBehave.playerAnimator.Play("getHit");
        }
    }
}

