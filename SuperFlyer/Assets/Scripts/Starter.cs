using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class Starter : MonoBehaviour
{
    private void Start()
    {
        LeanTouch.OnFingerTap += StartPlaying;
    }
    public void StartPlaying(LeanFinger finger)
    {
        GetComponent<CameraMovement>().enabled = true;
        GetComponent<Boost>().enabled = true;
        LeanTouch.OnFingerTap -= StartPlaying;
        this.enabled = false;
    }
}
