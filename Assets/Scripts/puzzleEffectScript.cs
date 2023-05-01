using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class puzzleEffectScript : MonoBehaviour
{
    public bool triggered;
    public PuzzleType type;
    public GameObject objectToMove;
    public float rate = 5f;
    public GameObject destObj;
    Vector3 start,dest,init_start;
    Animator anmtr;
    // Use this for initialization
    void Start()
    {
        if(type == PuzzleType.fallingSprites || type == PuzzleType.spinningSaw)
        {
            start = objectToMove.transform.position;
            dest = destObj.transform.position;
            init_start = start;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (triggered)
            switch (type)
            {
                case PuzzleType.gate:
                    StartCoroutine(LerpPosition(destObj.transform.position, rate));
                    triggered = false;
                    break;
                case PuzzleType.fallingSprites:
                    objectToMove.transform.position = Vector3.Lerp(transform.position, init_start, rate);
                    break;
                case PuzzleType.spinningSaw:
                    if (anmtr != null)
                        anmtr.enabled = false;
                    break;
            }
        if (!triggered)
            switch (type)
            {
                case PuzzleType.fallingSprites:
                    backAndForth();
                    break;
                case PuzzleType.spinningSaw:
                    backAndForth();
                    break;
            }
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        GetComponent<AudioSource>().enabled = true;
        float time = 0;
        Vector3 startPosition = objectToMove.transform.position;
        while (time < duration)
        {
            objectToMove.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objectToMove.transform.position = targetPosition;
    }
    void swap(ref Vector3 start, ref Vector3 destObj)
    {
        Vector3 temp;
        temp = start;
        start = destObj;
        destObj = temp;
    }
    void backAndForth()
    {
        if (objectToMove.transform.position == dest)
            swap(ref start, ref dest);
        objectToMove.transform.position = Vector3.MoveTowards(transform.position, dest, rate);
    }
}
