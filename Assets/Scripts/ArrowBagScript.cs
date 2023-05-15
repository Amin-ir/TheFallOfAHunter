using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBagScript : MonoBehaviour {
    public int ArrowToGive = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            var player = FindObjectOfType<PlayerFightSystem>();
            player.arrowCount += ArrowToGive;
            player.UpdateArrowCounterUI();
            gameObject.SetActive(false);
        }
    }
}
