using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeScript : MonoBehaviour {

	public GameObject upperRope, lowerRope;
	public bool hasBeenCut = false;
	public Sprite cutUpperRopeSpr, cutLowerRopeSpr;
	Sprite upperRopeNonCutSprite, lowerRopeNonCutSprite;
	Vector3 positionToRespawn;
	// Use this for initialization
	void Start () {
		positionToRespawn = transform.position;
		lowerRopeNonCutSprite = lowerRope.GetComponent<SpriteRenderer>().sprite;
		upperRopeNonCutSprite = upperRope.GetComponent<SpriteRenderer>().sprite;
	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col)
    {
		if(col.CompareTag("weapon") && !hasBeenCut)
        {
			upperRope.GetComponent<SpriteRenderer>().sprite = cutUpperRopeSpr;
			lowerRope.GetComponent<SpriteRenderer>().sprite = cutLowerRopeSpr;
			transform.GetComponentInChildren<Rigidbody2D>().gravityScale = 10;
			lowerRope.transform.localScale = new Vector3(lowerRope.transform.localScale.x, lowerRope.transform.localScale.y * 2, lowerRope.transform.localScale.z);
			upperRope.transform.localScale = new Vector3(upperRope.transform.localScale.x, upperRope.transform.localScale.y * 2, upperRope.transform.localScale.z);
			hasBeenCut = true;
		}
    }
	void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.CompareTag("Surface"))
			{
				lowerRope.GetComponent<SpriteRenderer>().sprite = lowerRopeNonCutSprite;
				upperRope.GetComponent<SpriteRenderer>().sprite = upperRopeNonCutSprite;
				transform.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
				lowerRope.transform.localScale = new Vector3(lowerRope.transform.localScale.x, lowerRope.transform.localScale.y / 2, lowerRope.transform.localScale.z);
				upperRope.transform.localScale = new Vector3(upperRope.transform.localScale.x, upperRope.transform.localScale.y / 2, upperRope.transform.localScale.z);
				transform.position = positionToRespawn;
				hasBeenCut = false;
			}		
	}
}
