using UnityEngine;
using System.Collections;

public class cloneAxeScript : MonoBehaviour
{
	public Rigidbody2D rd;
	Animator anmtr;
	public float throwForceX = 15f,throwForceY = 0f;
	PlayerFightSystem player;
	// Use this for initialization
	void Start ()
	{
		player = FindObjectOfType<PlayerFightSystem> ();
		anmtr = GetComponent<Animator> ();
		rd = GetComponent<Rigidbody2D> ();
		if (player.transform.localScale.x < 0)
			throwForceX *= -1;
		rd.velocity = new Vector2 (throwForceX, throwForceY);
		if (rd.velocity.x < 0)
			transform.localScale = new Vector2 (transform.localScale.x * -1, transform.localScale.y);
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Surface")) {
			anmtr.SetBool ("axeOnSurface", true);
			rd.velocity = Vector2.zero;
			rd.gravityScale = 0f;
			}
		if (col.CompareTag ("Player") && rd.gravityScale == 0f) {
			player.hasAxe = true;
			player.axeSpriteRndrr.color = new Color(player.axeSpriteRndrr.color.r ,player.axeSpriteRndrr.color.g ,player.axeSpriteRndrr.color.b,255f);
			gameObject.SetActive (false);
			player.axeLogo.SetActive (true);
		}
	}
}