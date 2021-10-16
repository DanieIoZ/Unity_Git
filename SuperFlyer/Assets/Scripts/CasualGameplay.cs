using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CasualGameplay : MonoBehaviour
{
    #region Prefabs
    [Header("Block")]
    public GameObject BlockPrefab;
    [Header("Coins")]
    public GameObject CoinPrefab;
    #endregion
    #region PointObjects
    [Header("Coordinates")]
    //Green point - point of spawning obstacles
    public GameObject SpawnPoint;
    //Orange point - point of distance between 2 obstacles
    public GameObject DistancePoint;
    //Red point - point of destroying obstacles
    public GameObject DestroyPoint;
    //Gray points - minX and maxX
    public GameObject LeftXPoint;
    public GameObject RightXPoint;
    #endregion
    #region Coins
    [Tooltip("Спавнятся ли монетки")]
    public bool CoinSpawn;
    [Tooltip("Шанс спавна монеток")]
    [Range(0, 1)]
    public float CoinSpawnChance;
    #endregion
    #region Vectors
    Vector2 _SpawnPoint { get { return SpawnPoint.transform.position; } set { SpawnPoint.transform.position = value; } }
    Vector2 _DistancePoint { get { return DistancePoint.transform.position; } set { DistancePoint.transform.position = value; } }
    Vector2 _DestroyPoint { get { return DestroyPoint.transform.position; } set { DestroyPoint.transform.position = value; } }
    Vector2 _LeftXPoint { get { return LeftXPoint.transform.position; } set { LeftXPoint.transform.position = value; } }
    Vector2 _RightXPoint { get { return RightXPoint.transform.position; } set { RightXPoint.transform.position = value; } }
    #endregion
    #region Pools
    ObjectPool _blocks;
    public int BlockCount;

    ObjectPool _coins;
    public int CoinCount;
    #endregion
    [Header("Indexes")]
    [SerializeField]
    int _dbi = 0;
    int DownBlockIndex
    {
        get
        {
            return _dbi;
        }
        set
        {
            _dbi = value >= _blocks.MaxCount ? value % _blocks.MaxCount : value;
        }
    }
    [SerializeField]
    int _ubi = 0;
    int UpBlockIndex
    {
        get
        {
            return _ubi;
        }
        set
        {
            _ubi = value >= _blocks.MaxCount ? value % _blocks.MaxCount : value;
        }
    }

    [SerializeField]
    int _dci = 0;
    int DownCoinIndex
    {
        get
        {
            return _dci;
        }
        set
        {
            _dci = value >= _coins.MaxCount ? value % _coins.MaxCount : value;
        }
    }
    [SerializeField]
    int _uci = 0;
    int UpCoinIndex
    {
        get
        {
            return _uci;
        }
        set
        {
            _uci = value >= _coins.MaxCount ? value % _coins.MaxCount : value;
        }
    }

    #region BaseMethods
    void Start()
    {
        _blocks = new ObjectPool(BlockPrefab, BlockCount);
        _coins = new ObjectPool(CoinPrefab, CoinCount);

        _blocks.InstantiatePool();
        _coins.InstantiatePool();

        SetPosAndActive(_blocks.Objects[0], new Vector2(Random.Range(_LeftXPoint.x, _RightXPoint.x), _SpawnPoint.y), true);
    }


    void Update()
    {
        if (_blocks.Objects[UpBlockIndex].transform.position.y < _DistancePoint.y)
        {
            if (CoinSpawn && Random.Range(0f, 1f) < CoinSpawnChance)
            {
                Debug.Log("CoinSpawn");
                SetPosAndActive(_coins.Objects[UpCoinIndex], GetComponent<CoinSpawn>().GetCoinPos(),true);
                UpCoinIndex++;
            }
            UpBlockIndex++;
            SetPosAndActive(_blocks.Objects[UpBlockIndex],new Vector2(Random.Range(_LeftXPoint.x, _RightXPoint.x), _SpawnPoint.y), true);
        }

        //Deactivate last Block
        if (_blocks.Objects[DownBlockIndex].transform.position.y < _DestroyPoint.y)
        {
            _blocks.Objects[DownBlockIndex].SetActive(false);
            DownBlockIndex++;
        }
        if (CoinSpawn && _coins.Objects[DownCoinIndex].transform.position.y < _DestroyPoint.y)
        {
            _coins.Objects[DownCoinIndex].SetActive(false);
            DownCoinIndex++;
        }
    }
    #endregion
    #region Position Methods
    public static void SetPosAndActive(GameObject Object, Vector2 Pos, bool Active)
    {
        Object.SetActive(Active);
        Object.transform.position = Pos;
    }
    public static void SetPosAndActive(GameObject Object, Vector2 Pos, Quaternion Rotation, bool Active)
    {
        Object.SetActive(Active);
        Object.transform.rotation = Rotation;
        Object.transform.position = Pos;
    }
    public static void SetPos(GameObject Object, Vector2 Pos, Quaternion Rotation)
    {
        Object.transform.rotation = Rotation;
        Object.transform.position = Pos;
    }

    #endregion
}
