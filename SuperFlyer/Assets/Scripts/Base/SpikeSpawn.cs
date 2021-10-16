using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawn : MonoBehaviour
{
    public GameObject SpikePrefab;

    float Spike_HalfWidth;

    public int SpikeCount;

    public float SpikeDistance = 0.5f;

    #region Border Transforms
    public Transform RightBorder_Upper;
    //public Transform RightBorder_UpperRight;
    public Transform RightBorder_Down;
    //public Transform RightBorder_DownRight;
    //public Transform LeftBorder_UpperLeft;
    public Transform LeftBorder_Upper;
    //public Transform LeftBorder_DownLeft;
    public Transform LeftBorder_Down;
    #endregion

    /// <summary>
    /// B = Border
    /// R = Right 
    /// L = Left
    /// U = Upper
    /// D = Down
    /// </summary>

    Vector2 RB_Up
    { get { return RightBorder_Upper.position; } set { RightBorder_Upper.position = value; } }
    //Vector2 RB_UR
    //{ get { return RightBorder_UpperRight.position; } set { RightBorder_UpperRight.position = value; } }
    Vector2 RB_Down
    { get { return RightBorder_Down.position; } set { RightBorder_Down.position = value; } }
    //Vector2 RB_DR
    //{ get { return RightBorder_DownRight.position; } set { RightBorder_DownRight.position = value; } }

    //Vector2 LB_UL
    //{ get { return LeftBorder_UpperLeft.position; } set { LeftBorder_UpperLeft.position = value; } }
    Vector2 LB_Up
    { get { return LeftBorder_Upper.position; } set { LeftBorder_Upper.position = value; } }
    //Vector2 LB_DL
    //{ get { return LeftBorder_DownLeft.position; } set { LeftBorder_DownLeft.position = value; } }
    Vector2 LB_Down
    { get { return LeftBorder_Down.position; } set { LeftBorder_Down.position = value; } }

    float CameraLeft;
    float CameraRight;

    ObjectPool Spikes;

    delegate void EnableMethods();
    EnableMethods EM = () => Debug.Log("Enable");

    void Start()
    {
        CreatePool();
        EM = SpawnSpikes;
        Debug.Log("SPAWNSPIKES ADDED");
        Spike_HalfWidth = 0.25f; //SpikePrefab.GetComponentInChildren<Renderer>().bounds.size.x / 2;
        Debug.Log(Spike_HalfWidth);
        SpawnSpikes();
    }

    public void CreatePool()
    {
        CameraRight = FindObjectOfType<CameraOptions>().halfWidth;
        CameraLeft = -CameraRight;

        Spikes = new ObjectPool(SpikePrefab, SpikeCount);
        Spikes.InstantiatePool(gameObject.transform, true);
    }

    private void OnEnable()
    {
        EM();
        Debug.Log("OnEnable " + gameObject.name);
    }

    public void SpawnSpikes()
    {
        Debug.Log("SS start");
        for (int i = 0; i < SpikeCount; i++)
        {
            int Side = Random.Range(0, 3);
            switch (Side)
            {
                case 0:
                    CasualGameplay.SetPos(Spikes.Objects[i], new Vector2(RoundToFraction(Random.Range(CameraLeft + Spike_HalfWidth, LB_Down.x), 0.5f), LB_Down.y), Quaternion.identity);
                    break;
                case 1:
                    CasualGameplay.SetPos(Spikes.Objects[i], new Vector2(RoundToFraction(Random.Range(RB_Down.x, CameraRight - Spike_HalfWidth), 0.5f), RB_Down.y), Quaternion.identity);
                    break;
                case 2:
                    CasualGameplay.SetPos(Spikes.Objects[i], new Vector2(RoundToFraction(Random.Range(CameraLeft + Spike_HalfWidth, LB_Up.x), 0.5f), LB_Up.y), Quaternion.Euler(0,0,180));
                    break;
                case 3:
                    CasualGameplay.SetPos(Spikes.Objects[i], new Vector2(RoundToFraction(Random.Range(RB_Up.x, CameraRight - Spike_HalfWidth), 0.5f), RB_Up.y), Quaternion.Euler(0, 0, 180));
                    break;
            }
        }
        Debug.Log("SPAWNED");
    }

    //Vector2 SpawnSingleSpike()
    //{
    //    Vector2[] Pos = new Vector2[2]
    //       {
    //            new Vector2(Random.Range(CameraLeft, LB_DR.x), LB_DR.y),
    //            new Vector2(Random.Range(RB_DL.x, CameraRight), RB_DL.y)
    //       };
    //    Vector2 NewPos = Pos[Random.Range(0, 1)];
    //    return new Vector2(RoundToFraction(NewPos.x, 0.5f), NewPos.y);
    //}

    float RoundToFraction(float value, float fraction)
    {
        return Mathf.Round(value / fraction) * fraction;
    }
}
