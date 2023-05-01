using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpScr : MonoBehaviour {
    public int addingXP = 10;
    PlayerBehaviour playerScr;
	// Use this for initialization
	void Start () {
        playerScr = FindObjectOfType<PlayerBehaviour>();
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerScr.xp += addingXP;
            gameObject.SetActive(false);
        }
    }           
}
