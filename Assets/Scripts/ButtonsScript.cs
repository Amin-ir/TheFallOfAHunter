using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine.UI;
public class ButtonsScript : MonoBehaviour {
	public GameObject confirmationForNewGame;
	public GameObject confirmationForSkipCuscene;
	public codexPage[] codexPages;
	public GameObject quote;
	int codexCounter = 0;
	GameObject codexObject;
	public void Start()
    {
		if(!PlayerPrefs.HasKey("DisplayTips") || PlayerPrefs.GetInt("DisplayTips") == 0)
			PlayerPrefs.SetInt("DisplayTips",1);
		if(FindObjectOfType<Toggle>() != null)
			FindObjectOfType<Toggle>().isOn = (PlayerPrefs.GetInt("DisplayTips") == 1);
		if (transform.Find("Codex") != null)
			codexObject = transform.Find("Codex").gameObject;
    }
	public void exit(){
		Application.Quit();
	}
	public void mainMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene("mainMenu");
	}
	public void continueGame(){
		Time.timeScale = 1f;
		if (PlayerPrefs.HasKey("level"))
			SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
		else newGame();
	}
	public void newGame() {
		if (PlayerPrefs.HasKey("level") && !confirmationForNewGame.activeInHierarchy)
		{
			confirmationForNewGame.SetActive(true);
			return;
		}
		SceneManager.LoadScene(2);
		CustomTools.StoreGameSavingParameters(0, 0, 0, 0, 0, 0, 0);
	}
	public void closeConfirmNewgameObject()
    {
		confirmationForNewGame.SetActive(false);
    }
	public void creator()
    {
		GetComponent<Animator>().Play("gotocreator");
    }
	public void mainOps()
    {
		GetComponent<Animator>().Play("gotomain");
    }
	public void pause()
    {
		FindObjectsOfType<AudioSource>().ToList().ForEach(audio => audio.Pause());
		transform.Find("pauseMenu").gameObject.SetActive(true);
		Time.timeScale = 0f;
    }
	public void resume()
    {
		FindObjectsOfType<AudioSource>().ToList().ForEach(audio => audio.Play());
		transform.Find("pauseMenu").gameObject.SetActive(false);
		Time.timeScale = 1f;
    }
	public void showCodex()
    {
		codexCounter = 0;
		codexObject.SetActive(true);
		fetchNextCodexPage();
    }
	public void fetchNextCodexPage() 
	{
		if(codexCounter != codexPages.Length - 1)
        {
			var nextCodex = codexPages[++codexCounter];
			loadCodexPage(nextCodex);
        }
		else
        {
			codexCounter = 0;
			fetchNextCodexPage();
        }
	}
	public void fetchPrevCodexPage() 
	{
		if (codexCounter != 0)
		{
			var prevCodex = codexPages[--codexCounter];
			loadCodexPage(prevCodex);
		}
		else
        {
			codexCounter = codexPages.Length - 1;
			fetchPrevCodexPage();
        }
	}
	public void loadCodexPage(codexPage cp)
    {
		codexObject.transform.Find("Text").GetComponent<Text>().text = cp.content;
		var UI_Image = codexObject.transform.Find("Image").GetComponent<Image>();
		if (cp.additionalImage != null)
		{
			UI_Image.sprite = cp.additionalImage;
			UI_Image.color = Color.white;
		}
		else UI_Image.color = Color.clear;
	}
	public void closeCodex()
    {
		codexObject.SetActive(false);
	}
	public void loadNextLevelInstantly(){
		Time.timeScale = 1f;
		CustomTools.FetchNextLevelInstantly();
	}
	public void ActivateSkipCutscenePanel(){
		Time.timeScale = 0f;
		confirmationForSkipCuscene.SetActive(true);
	}
	public void CloseConfirmationForSkipCutscene(){
		Time.timeScale = 1f;
		confirmationForSkipCuscene.SetActive(false);
	}
	public void ToggleDisplayingTips(){
		PlayerPrefs.SetInt("DisplayTips",-PlayerPrefs.GetInt("DisplayTips"));
	}
}
