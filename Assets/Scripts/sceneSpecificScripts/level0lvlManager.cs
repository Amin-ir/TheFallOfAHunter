using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;


public class level0lvlManager : MonoBehaviour {

	public Camera playCam,mainCam;
	public GameObject player,playerUI;
	public Transform startPos, endPos;
	freeRunScript playerScr;
	public AudioClip[] audios;
	AudioSource adSrc;
	Animator anmtr;
	public string[] dialogues;
	int playAudioNumber = 0,dialogueCounter = 0;
	public Text DialogueBox;
	public Image profile1, profile2;
	public Sprite thirdProfile;
	public bool symmetricThirdProfile = false;
	void Start () {	
		anmtr = GetComponent<Animator>();
		if (player != null)
		{
			playerScr = player.GetComponent<freeRunScript>();
			playerScr.toMove = false;
		}
		adSrc = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if (player != null)
		{
			if (player.transform.position.x >= startPos.position.x)
				playCam.transform.parent = player.transform;
			if (player.transform.position.x >= endPos.position.x)
				playCam.transform.parent = null;
			if (player.transform.position.x >= endPos.position.x + 50)
			{
				playerScr.notMove();
				playerUI.SetActive(false);
				mainCam.gameObject.SetActive(true);
				playCam.gameObject.SetActive(false);
				GetComponent<Animator>().Play("takedown");
			}
		}
	}
	void startGame()
    {
		playerScr.toMove = true;
		mainCam.gameObject.SetActive(false);
		playCam.gameObject.SetActive(true);
		playerUI.SetActive(true);
		player.SetActive(true);
    }
	void fetchNextAudio()
    {
		adSrc.clip = audios[++playAudioNumber];
		adSrc.Play();
    }
	void fetchNextDialogue()
    {
		DialogueBox.gameObject.SetActive(true);
		DialogueBox.text = dialogues[dialogueCounter++];
    }
	void changeAvatar()
    {
		// This method activates profile 1 prior to profile 2 if both are inactive
		if(profile1.gameObject.activeInHierarchy)
        {
			profile2.gameObject.SetActive(true);
			profile1.gameObject.SetActive(false);
        } else
		{
			profile2.gameObject.SetActive(false);
			profile1.gameObject.SetActive(true);
		}
    }
	void DisableDialogueBox()
    {
		DialogueBox.text = "";
		profile1.gameObject.SetActive(false);
		profile2.gameObject.SetActive(false);
	}
	void changeOppositeAvatar()
    {
		profile2.sprite = thirdProfile;
		if(symmetricThirdProfile)
			profile2.transform.localScale = new Vector2(-profile2.transform.localScale.x, profile2.transform.localScale.y);
    }
	void loadNextSceneAfterCutscene()
    {
		var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
		if (nextLevel <= 13)
			CustomTools.StoreGameSavingParameters(
				nextLevel,
				0,
				PlayerPrefs.GetInt("xp"),
				PlayerPrefs.GetInt("armorUpgrade"),
				PlayerPrefs.GetInt("swordUpgrade"),
				PlayerPrefs.GetInt("axeUpgrade"),
				PlayerPrefs.GetInt("arrow")
				);
		else nextLevel = 1;
		SceneManager.LoadScene(nextLevel);
	}
}
