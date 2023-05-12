using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeScript : MonoBehaviour {

	public GameObject upperRope, lowerRope;
	public bool hasBeenCut = false;
	public Sprite cutUpperRopeSpr, cutLowerRopeSpr;
	Sprite upperRopeNonCutSprite, lowerRopeNonCutSprite;
	GameObject ObjectToClone;
	void Start () {
		ObjectToClone = Resources.Load<GameObject>("3rdAngerObstacles");
		lowerRopeNonCutSprite = lowerRope.GetComponent<SpriteRenderer>().sprite;
		upperRopeNonCutSprite = upperRope.GetComponent<SpriteRenderer>().sprite;
	}
	void OnTriggerEnter2D(Collider2D col)
    {
		if(col.CompareTag("weapon") && !hasBeenCut)
        {
			upperRope.GetComponent<SpriteRenderer>().sprite = cutUpperRopeSpr;
			lowerRope.GetComponent<SpriteRenderer>().sprite = cutLowerRopeSpr;
			transform.GetComponentInChildren<Rigidbody2D>().gravityScale = 10;
			GetComponent<CircleCollider2D>().isTrigger = false;
			lowerRope.transform.localScale = new Vector3(lowerRope.transform.localScale.x, lowerRope.transform.localScale.y * 2, lowerRope.transform.localScale.z);
			upperRope.transform.localScale = new Vector3(upperRope.transform.localScale.x, upperRope.transform.localScale.y * 2, upperRope.transform.localScale.z);
			hasBeenCut = true;
		}
		else if (col.gameObject.CompareTag("Surface"))
			CloneSpikeyBall();	
    }
	public void CloneSpikeyBall(){
		var ObjectToDeactive = GetParentRecursively(3); 
		var _clone = Instantiate(ObjectToClone, ObjectToDeactive.transform.position, Quaternion.identity) as GameObject;
		ObjectToDeactive.SetActive(false);		
	}
	GameObject GetParentRecursively(int depth){
		GameObject target = gameObject;
		for(int i = 0; i < depth; i++)
			target = target.transform.parent.gameObject;
		return target;
	}
}
