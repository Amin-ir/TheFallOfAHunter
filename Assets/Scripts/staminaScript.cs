using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class staminaScript : MonoBehaviour
{
    public float addingStamina = 50f;
    PlayerFightSystem playerScr;
    // Use this for initialization
    void Start()
    {
        playerScr = FindObjectOfType<PlayerFightSystem>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerScr.stamina += addingStamina;
            gameObject.SetActive(false);    
        }
    }
}
