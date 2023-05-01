using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using System.Collections.Generic;
using System.Collections;

public class levelEndScript : MonoBehaviour {

    blackCurtainScript blackCurtain;
    PlayerBehaviour playerScript;

    void Start()
    {
        if(FindObjectOfType<PlayerBehaviour>() != null)
            playerScript = FindObjectOfType<PlayerBehaviour>();
        blackCurtain = FindObjectOfType<blackCurtainScript>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            blackCurtain.unFade();
            StartCoroutine(saveAndLoadNextScene());    
        }
    }
    IEnumerator saveAndLoadNextScene()
    {
        yield return new WaitForSeconds(5);
        var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if(playerScript != null)
            CustomTools.StoreGameSavingParameters(
                nextLevel,
                0,
                playerScript.xp,
                playerScript.armorUpgradeTemp,
                playerScript.swordUpgradeTemp,
                playerScript.axeUpgradeTemp,
                playerScript.playerScr.arrowCount
                );
        SceneManager.LoadScene(nextLevel);
    }
}
