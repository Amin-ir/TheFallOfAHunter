using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipUIDisplayScript : MonoBehaviour
{
    public void CloseTipWindow()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
}
