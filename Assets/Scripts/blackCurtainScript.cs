using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class blackCurtainScript : MonoBehaviour {

	public int secondsWaitBeforeFade = 5;
	public float fadeRate = 5f;
	public bool allowedToDeactive = false;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(fade(secondsWaitBeforeFade));
	}
	void Update()
    {

    }
	IEnumerator fade(int secondsToWait)
    {
		yield return new WaitForSeconds(secondsToWait);
		GetComponent<Image>().CrossFadeAlpha(0, fadeRate, false);
		if (allowedToDeactive)
			StartCoroutine(deactive(5));
    }
	IEnumerator deactive(int secondsToWait)
    {
		yield return new WaitForSeconds(secondsToWait);
		gameObject.SetActive(false);
    }
	public void unFade()
    {
		GetComponent<Image>().CrossFadeAlpha(1, fadeRate, false);
    }
}
