using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Block")]
    public GameObject BlockPrefab;
    //Кол-во блоков на экране
    public int BlocksCount = 5;
    public int BlockMoveSpeed = 5;
    [Header("Coordinates")]
    public float UpSpawnBorderY;
    public float DownDestroyBorderY;
    public float MinimumSpawnX;
    public float MaximumSpawnX;

    Queue<IBlock> _blocks = new Queue<IBlock>();
    Vector2 CameraPos;
    // Start is called before the first frame update
    void Start()
    {
        if (BlockPrefab.GetComponent<IBlock>() == null)
            throw new System.ArgumentException("Отсутствет компонент IBlock");
        CameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Add new Block
        if(_blocks.Count < BlocksCount)
        {
            IBlock last;
            try
            {
                last = _blocks.Last();
            }
            catch
            {
                last = null;
            }
            if (_blocks.Count == 0 || last.Position.y < UpSpawnBorderY - last.NextBlockY)
                SpawnBlock(new Vector3(Random.Range(MinimumSpawnX, MaximumSpawnX), UpSpawnBorderY, 0));
        }
        //Destroy last Block
        if (_blocks.Count > 0 && _blocks.Peek().Position.y < CameraPos.y + DownDestroyBorderY)
            DestroyBlock();
        //Move Blocks
        foreach (var block in _blocks)
            block.Position += Vector2.down * BlockMoveSpeed * Time.deltaTime; 
    }

    void SpawnBlock(Vector3 position)
    {
        var block = Instantiate(BlockPrefab, position, Quaternion.identity).GetComponent<IBlock>();
        block.NextBlockY = (UpSpawnBorderY - DownDestroyBorderY) / BlocksCount;
        _blocks.Enqueue(block);
    }

    void DestroyBlock()
    {
        _blocks.Dequeue().Destroy();
    }

    private void OnDestroy()
    {
        foreach (var block in _blocks)
            block.Destroy();
    }
}
