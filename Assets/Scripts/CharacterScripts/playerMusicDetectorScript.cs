using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMusicDetectorScript : MonoBehaviour {

    public string targetObjectTag;
    AudioSource fightingMusic;

    void Start()
    {
        fightingMusic = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetObjectTag) && !fightingMusic.isPlaying)
            fightingMusic.Play();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetObjectTag) && !fightingMusic.isPlaying)
            fightingMusic.Play();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(targetObjectTag))
            fightingMusic.Stop();
    }
}
