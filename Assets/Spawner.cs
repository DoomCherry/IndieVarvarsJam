using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ObjectInfo
{
    public GameObject gameObject;
    public float chance;
}

public class Spawner : MonoBehaviour
{
    public ObjectInfo[] prefabs;


    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(GetRandomObject(prefabs).gameObject, transform.position, Quaternion.identity);
    }

    private ObjectInfo GetRandomObject(ObjectInfo[] collection)
    {
        float maxChance = GetMaxChance(collection);
        float randomValue = UnityEngine.Random.Range(0, maxChance);
        float currentDownChance = 0;

        if (collection == null || collection.Length == 0)
            throw new System.ArgumentNullException($"{this}, {nameof(HallwayInfo)} collection is not contain items, or equal null.");

        for (int i = 0; i < collection.Length; i++)
        {
            if (randomValue >= currentDownChance && randomValue < currentDownChance + collection[i].chance)
                return collection[i];

            currentDownChance += collection[i].chance;
        }

        return collection.Last();
    }

    private float GetMaxChance(ObjectInfo[] collection)
    {
        float count = 0;

        for (int i = 0; i < collection.Length; i++)
        {
            count += collection[i].chance;
        }

        return count;
    }
}
