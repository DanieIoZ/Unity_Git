using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using Lean.Touch;

public class Movement : MonoBehaviour
{
    #region Mobile Controls
    public bool Mobile;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    #endregion

    #region Speed Parameters
    [Header("Speed parameters")]
    [Tooltip("Vertical speed when tapping (Recommended = 2)")]
    public float VerticalSpeed;
    [Tooltip("Horizontal speed when tapping (Recommended = 1)")]
    public float HorizontalSpeed;
    [Tooltip("Multiplayer of speed for AddForce when tapping (Recommended = 100)")]
    public float SpeedMultiplyer;
    #endregion

    #region Swipes
    [Header("Swipes")]
    [Tooltip("Velocity of horizontal swiping (Recommended = 15)")]
    public float HorizontalSwipeVelocity;
    [Tooltip("Velocity of vertical swiping (Recommended = 15)")]
    public float VerticalSwipeVelocity;
    [Tooltip("Linear drag of Rigidbody2D when swiping (Recommended = 4)")]
    public float SwipeDrag;
    #endregion

    #region Additional Attributes
    int HorizontalOrientation;
    Rigidbody2D Rigid;
    bool SwipedHorizontal;
    bool SwipedVertical;
    #endregion

    #region Base Methods
    void Start()
    {
        dragDistance = Screen.height * 0.05f;
        HorizontalOrientation = 1;
        Rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //if (Mobile)
        //{
            
        //}
        //else
        //{
        //    if (Input.anyKeyDown)
        //    {
        //        if (Input.GetAxis("Jump") > 0)
        //        {
        //            Debug.Log("JUMP");
        //            Jump();
        //            return;
        //        }
        //        if (Input.GetAxis("Swipe Horizontal") != 0)
        //        {
        //            if (Input.GetAxis("Swipe Horizontal") > 0)
        //            {
        //                SwipeHorizontal(1);
        //            }
        //            else
        //            {
        //                Debug.Log("Swipe Hor Negative");
        //                SwipeHorizontal(-1);
        //            }
        //            return;
        //        }
        //        if (Input.GetAxis("Swipe Vertical") > 0)
        //        {
        //            SwipeVertical();
        //            return;
        //        }

        //    }
        //}
        
        if (SwipedVertical && Mathf.Abs(Rigid.velocity.y) < 1)
        {
            SwipedVertical = false;
        }
        if (SwipedHorizontal && Mathf.Abs(Rigid.velocity.x) < 2)
        {
            Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            SwipedHorizontal = false;
        }
        if (!(SwipedHorizontal | SwipedVertical))
        {
            Rigid.drag = 0;
        }
    }
    #endregion

    public void Jump()
    {
        Rigid.velocity = new Vector2(0, 0);
        Rigid.AddForce(new Vector2(HorizontalSpeed * HorizontalOrientation, VerticalSpeed) * SpeedMultiplyer);
    }
    public void SwipeHorizontal(int Direction)
    {
        HorizontalOrientation = Direction;
        Rigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        Rigid.drag = SwipeDrag;
        SwipedHorizontal = true;
        Rigid.velocity = (new Vector2(HorizontalSwipeVelocity * HorizontalOrientation, 0));
        InversePicHorizontal(HorizontalOrientation);
    }
    public void SwipeVertical()
    {
        Debug.Log("Swipe Vertical");
        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rigid.drag = SwipeDrag;
        Rigid.velocity = (new Vector2(Rigid.velocity.x, VerticalSwipeVelocity));
        SwipedVertical = true;
    }

    #region Methods when touching the border
    /// <summary>
    ///  1 - Right
    /// -1 - Left
    /// </summary>
    /// <param name="Inversion"></param>
    private void InversePicHorizontal(int Inversion)
    {
        transform.localScale = new Vector3(Inversion * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.y);
    }
    private void InversePicHorizontal()
    {
        HorizontalOrientation *= -1;
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
            InversePicHorizontal();
    }
    #endregion
}
