using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFieldOfViewScript : MonoBehaviour
{
    void Start()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float targetAspect = 16f / 9f;
        float _fieldOfView = Camera.main.fieldOfView;
        float diff = targetAspect / aspectRatio;
        Camera.main.fieldOfView = _fieldOfView * diff;
    }
}
