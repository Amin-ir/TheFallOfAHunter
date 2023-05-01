using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerArrowScript : MonoBehaviour {

	Rigidbody2D rd;
	public float arrowSpeedX = 20f,arrowSpeedY = 0f;
	PlayerFightSystem player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerFightSystem>();
		rd = GetComponent<Rigidbody2D>();  
		if (player.transform.localScale.x > 0)
			rd.velocity = new Vector2(arrowSpeedX, arrowSpeedY);
		else { 
			rd.velocity = new Vector2(arrowSpeedX * -1, arrowSpeedY);
		}
		if (player.transform.localScale.x > 0)
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("enemy") || other.CompareTag("Surface"))
			gameObject.SetActive(false);
	}
}
