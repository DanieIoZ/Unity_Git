using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasItemsScaler : MonoBehaviour
{
    [SerializeField]
    RectTransform GameZone;
    [SerializeField]
    public RectTransform AdZone;
    // Start is called before the first frame update
    void Start()
    {
        GameZone.sizeDelta = AdZone.gameObject.activeSelf ? new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, gameObject.GetComponent<RectTransform>().sizeDelta.y - AdZone.sizeDelta.y) : gameObject.GetComponent<RectTransform>().sizeDelta;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
