using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneFastenerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
			Time.timeScale = 1;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			Time.timeScale = 2;
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			Time.timeScale = 3;
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			Time.timeScale = 4;
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			Time.timeScale = 5;
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			Time.timeScale = 6;
		else if (Input.GetKeyDown(KeyCode.Alpha0))
			Time.timeScale = 0.5f;
	}
}
