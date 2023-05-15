using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class puzzleScript : MonoBehaviour {

	public GameObject clone;
	PlayerFightSystem player;
	Animator anmtr;
	bool pulled = false, AllowedToInteract = false;
    public string animationName;
    puzzleEffectScript effectedPuzzle;
	// Use this for initialization
	void Start () {
        effectedPuzzle = GetComponentInChildren<puzzleEffectScript>();
		player = FindObjectOfType<PlayerFightSystem>();
		anmtr = GetComponent<Animator> ();
		clone = gameObject.transform.Find ("clone").gameObject;
    }
    private void Update()
    {
        if (AllowedToInteract && 
            !pulled &&
            CrossPlatformInputManager.GetButtonDown("Interaction") && 
            CheckIfPlayerIsStill())
        {
            player.gameObject.SetActive(false);
            clone.gameObject.SetActive(true);
            anmtr.Play(animationName);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            {
                FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
                    .FirstOrDefault().GetComponent<Image>().color = Color.white;
                AllowedToInteract = true;
            }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
                  .FirstOrDefault().GetComponent<Image>().color = Color.clear;
            AllowedToInteract = false;
        }
    }
    void endPulling()
    {
		pulled = true;
        clone.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        player.GetComponent<PlayerBehaviour>().allowToMove = true;
        player.SetAnimatorStyleParameter();
        if(effectedPuzzle != null)
            effectedPuzzle.triggered = true;
        if(effectedPuzzle.GetComponent<enemyWeaponScript>() != null)
            effectedPuzzle.GetComponent<enemyWeaponScript>().damageGiven = 0;
    }
    bool CheckIfPlayerIsStill()
    {
        if (player.playerStyle == Assets.Scripts.Styles.idol 
            && player.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            return true;
        else return false;
    }
}
