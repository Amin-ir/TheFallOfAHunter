using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closingTrapScript : MonoBehaviour {

	Animator anmtr;
	// Use this for initialization
	void Start () {
		anmtr = GetComponent<Animator>();
	}
	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.CompareTag("Player") || col.CompareTag("weapon"))
			anmtr.SetBool("triggered", true);

	}
}
