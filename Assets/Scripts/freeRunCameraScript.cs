using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeRunCameraScript : MonoBehaviour {

	public Transform cameraStartFollowingPlayerPoint, cameraStopFollowingPlayerPoint;

	freeRunScript player;
	bool cameraFollowPlayer = false;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<freeRunScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!cameraFollowPlayer)
        {
			if(player.transform.position.x > cameraStartFollowingPlayerPoint.position.x)
            {
				cameraFollowPlayer = true;
				Camera.main.transform.parent = player.transform;
            }
        }
		else
        {
			if (player.transform.position.x > cameraStopFollowingPlayerPoint.position.x)
				Camera.main.transform.parent = null;
        }
	}
}
