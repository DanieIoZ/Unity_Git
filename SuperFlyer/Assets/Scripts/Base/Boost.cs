using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public Rigidbody2D Player;
    public GameObject MobileControl;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("JUMP");
        Player.velocity = new Vector2(0, Speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.velocity.y < 1)
        {
            MobileControl.SetActive(true);
            Player.GetComponent<Movement>().enabled = true;
            this.enabled = false;
        }
    }
}
