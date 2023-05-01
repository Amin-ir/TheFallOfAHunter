using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
    public float ForwardView = 30f;
    public GameObject player;
    public float camMoveRate = 5f;
    Vector3 vecForward;
	void Start () {
        transform.position = new Vector3(player.transform.position.x + ForwardView, player.transform.position.y, transform.position.z);
        vecForward = new Vector3(player.transform.position.x + ForwardView, player.transform.position.y, transform.position.z);
    }
	void FixedUpdate () {
        vecForward = new Vector3(player.transform.position.x + ForwardView, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, vecForward, camMoveRate * Time.deltaTime);
        if (Mathf.Sign(ForwardView) * Mathf.Sign(player.transform.localScale.x) < 0)
            ForwardView *= -1;

    }
    
}
