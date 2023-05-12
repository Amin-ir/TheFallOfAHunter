using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class tipScript : MonoBehaviour {
    public int indexAtCodex;
    bool alreadyDisplayed = false;
    Text tipsText;
    Image tipsImage;
    GameObject tipsGameObject;
    private void Start()
    {
        tipsGameObject = FindObjectOfType<Canvas>().transform.Find("tipsUI").gameObject;
        tipsImage = tipsGameObject.transform.Find("Image").GetComponent<Image>();
        tipsText = tipsGameObject.GetComponentInChildren<Text>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && !alreadyDisplayed)
        {
            var fetchedCodex = FindObjectOfType<Canvas>().GetComponent<ButtonsScript>().codexPages[indexAtCodex];
            tipsText.text = fetchedCodex.content;
            if (fetchedCodex.additionalImage != null)
            {
                tipsImage.gameObject.SetActive(true);
                tipsImage.sprite = fetchedCodex.additionalImage;
            }
            else tipsImage.gameObject.SetActive(false);
            alreadyDisplayed = true;
            Time.timeScale = 0f;
            tipsGameObject.SetActive(true);
        }
    }
}
