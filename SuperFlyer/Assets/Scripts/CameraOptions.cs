using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptions : MonoBehaviour
{
    public float halfWidth = 3f;

    void Start()
    {
        Camera.main.orthographicSize = halfWidth / Camera.main.aspect;
    }
    private void Awake()
    {
        Camera.main.orthographicSize = halfWidth / Camera.main.aspect;
    }
}
