using UnityEngine;
using Assets.Scripts;
using UnityEngine.SceneManagement;
public class checkpointScript : MonoBehaviour {

	PlayerBehaviour player;
	public Sprite saved;
	SpriteRenderer checkpointSprite;
	Animator anmtr;
	public int id = 0;
	void Start () {
		checkpointSprite = GetComponent<SpriteRenderer> ();
		anmtr = GetComponent<Animator> ();
		player = FindObjectOfType<PlayerBehaviour> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Player")){
			checkpointSprite.sprite = saved;
			anmtr.SetBool ("saved", true);
			CustomTools.StoreGameSavingParameters(
				SceneManager.GetActiveScene().buildIndex,
				id,
				player.xp,
				player.armorUpgradeTemp,
				player.swordUpgradeTemp,
				player.axeUpgradeTemp,
				player.playerScr.arrowCount
				);
		}
	}
}
