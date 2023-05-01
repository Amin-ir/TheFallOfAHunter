using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using UnityEngine.UI;

public class upgradeTableScripts : MonoBehaviour {

	public GameObject upgradePanel;
    bool active = false;
    private void Update()
    {
        if(active)
            if (CrossPlatformInputManager.GetButtonDown("Interaction"))
                upgradePanel.SetActive(true);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            active = true;
            FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
                .FirstOrDefault().GetComponent<Image>().color = Color.white;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = false;
            FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "Interaction")
                  .FirstOrDefault().GetComponent<Image>().color = Color.clear;
        }
        }
}
