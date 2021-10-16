using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBlock
{
    float NextBlockY { get; set; }
    Vector2 Position { get; set; }
    void Destroy();
}

public class SimpleBlock : MonoBehaviour, IBlock
{
    public float NextBlockY { get; set; }
    public Vector2 Position { get => transform.position; set => transform.position = value; }
    public GameObject ScoreTrigger;
    // Start is called before the first frame update
    private void OnEnable()
    {
        ScoreTrigger.GetComponent<Collider2D>().enabled = true;
    }

    public void Destroy()
    {
        Destroy(transform.gameObject);
    }
}
