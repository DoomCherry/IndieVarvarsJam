using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HallwayType
{
    horizontal,
    vertical
}

[Serializable]
public struct HallwayInfo
{
    public TileType _hallType;
    public GameObject hallVariation;
    public float chanceToDrop;
}

public class HallwayHandler : MonoBehaviour
{
    [SerializeField] private HallwayInfo[] _horizontalHall;
    [SerializeField] private HallwayInfo[] _verticalHall;



   

    public GameObject GetRandomVerticalHall()
    {
        return GetRandomHall(_verticalHall);
    }

    public GameObject GetRandomHorizontalHall()
    {
        return GetRandomHall(_horizontalHall);
    }

    private GameObject GetRandomHall(HallwayInfo[] collection)
    {
        float maxChance = GetMaxChance(collection);
        float randomValue = UnityEngine.Random.Range(0, maxChance);
        float currentDownChance = 0;

        if (collection == null || collection.Length == 0)
            throw new ArgumentNullException($"{this}, {nameof(HallwayInfo)} collection is not contain items, or equal null.");

        for (int i = 0; i < collection.Length; i++)
        {
            if (randomValue >= currentDownChance && randomValue < collection[i].chanceToDrop)
                return collection[i].hallVariation;
        }

        return collection.Last().hallVariation;
    }

    private float GetMaxChance(HallwayInfo[] collection)
    {
        float count = 0;

        for (int i = 0; i < collection.Length; i++)
        {
            count += collection[i].chanceToDrop;
        }

        return count;
    }
}
