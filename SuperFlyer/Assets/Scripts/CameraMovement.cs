using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject Cam;

    [Tooltip("Объект-персонаж")]
    GameObject Player;

    [Header("Camera Moving")]
    [Tooltip("Автоматическое движение камеры вверх")]
    public bool AutoMove;
    public float CamSpeed;
    [Range(0, 1)]
    [Tooltip("Высота, после которой камера ускоряется и следует за персонажем")]
    public float CameraFastMoveBorder;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Movement>().gameObject;
        Cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cam.GetComponent<Camera>().WorldToScreenPoint(Player.transform.position, Camera.MonoOrStereoscopicEye.Mono).y > Cam.GetComponent<Camera>().pixelHeight * CameraFastMoveBorder)
        {

            Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y + (Player.transform.position).y - Camera.main.ScreenToWorldPoint(new Vector3(0,Cam.GetComponent<Camera>().pixelHeight * CameraFastMoveBorder,0)).y, Cam.transform.position.z);
        }
        if (AutoMove)
            Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y + CamSpeed * Time.deltaTime, Cam.transform.position.z);
    }

}
