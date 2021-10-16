using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public GameObject CoinPrefab;
    public Transform UpPoint;
    public Transform DownPoint;
    public Transform LeftXPoint;
    public Transform RightXPoint;

    public Vector2 GetCoinPos()
    {
        return new Vector2(Random.Range(LeftXPoint.position.x, RightXPoint.position.x), Random.Range(DownPoint.position.y, UpPoint.position.y));
    }
}
