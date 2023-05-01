using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour {

	Rigidbody2D rd;
	PlayerBehaviour plyr;
	public float arrowSpeed = 15f,arrowHeight = 5f;
	// Use this for initialization
	void Start () {
		plyr = FindObjectOfType<PlayerBehaviour> ();
		rd = GetComponent<Rigidbody2D> ();
		//rd.gravityScale = 0f;
		if (plyr.transform.position.x - transform.position.x > 0)
			rd.velocity = new Vector2 (arrowSpeed, arrowHeight);
		else
			rd.velocity = new Vector2 (arrowSpeed * -1, arrowHeight);
		if (rd.velocity.x < 0)
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Surface"))
			gameObject.SetActive (false);
	}
}
