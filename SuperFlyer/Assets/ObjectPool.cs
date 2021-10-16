using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool
{
    public GameObject ObjectPrefab;

    public List<GameObject> Objects = new List<GameObject>();

    public int MaxCount;

    public ObjectPool(GameObject _objectprefab, int Count)
    {
        ObjectPrefab = _objectprefab;
        MaxCount = Count;
    }

    public void InstantiatePool(bool Active = false)
    {
        for (int i = 0; i < MaxCount; i++)
        {
            Spawn(Active);
        }
    }

    public void InstantiatePool(Transform Parent, bool Active = false)
    {
        for (int i = 0; i < MaxCount; i++)
        {
            Spawn(Parent, Active);
        }
    }
    public GameObject Spawn(Transform Parent, bool Active = false)
    {
        Objects.Add(Object.Instantiate(ObjectPrefab, Parent));
        Objects.Last().SetActive(Active);
        return Objects.Last();
    }
    public GameObject Spawn(bool Active = false)
    {
        Objects.Add(Object.Instantiate(ObjectPrefab));
        Objects.Last().SetActive(Active);
        return Objects.Last();
    }
    public void Despawn(int Index)
    {
        Objects.Remove(Objects[Index]);
        Object.Destroy(Objects[Index]);
    }
    public void Despawn(GameObject Item)
    {
        Objects.Remove(Item);
        Object.Destroy(Item);
    }

    public GameObject GetFirstInactive(bool ExpandPool)
    {
        for (int i = 0; i < Objects.Count(); i++)
        {
            if (!Objects[i].activeInHierarchy)
                return Objects[i];
        }
        if (ExpandPool)
        {
            MaxCount++;
            return Spawn();
        }
        return null;
    }
}
