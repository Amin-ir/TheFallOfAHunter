using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpScr : MonoBehaviour {
    public int addingXP = 10;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var playerScr = FindObjectOfType<PlayerBehaviour>();
            playerScr.xp += addingXP;
            gameObject.SetActive(false);
        }
    }           
}
