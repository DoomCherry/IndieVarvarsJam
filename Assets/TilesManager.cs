using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HallType
{
    River,
    Sand,
    Mushroom,
    GarbagePath,
    Smoke,
    Hotel,

    //It's in end
    Simple,
}

[Serializable]
public struct ViewInfo
{
    public GameObject view;
    [MinValue(0), MaxValue(1)]
    public float chanse;
}

[Serializable]
public class TileInfo
{
    public ViewInfo[] wayViewVariations;
    public ViewInfo[] horizontalBlockViewVariations;
    public ViewInfo[] verticalBlockViewVariations;
    public ViewInfo[] leftDownCornerViewVariations;
    public ViewInfo[] rightDownCornerViewVariations;
    public ViewInfo[] leftUpCornerViewVariations;
    public ViewInfo[] rightUpCornerViewVariations;
    public ViewInfo[] upTransitionViewVariations;
    public ViewInfo[] downTransitionViewVariations;
    public ViewInfo[] leftTransitionViewVariations;
    public ViewInfo[] rightTransitionViewVariations;



    public GameObject GetRandomWayView()
    {
        return GetRandomView(wayViewVariations);
    }

    public GameObject GetRandomHorizontalBlockView()
    {
        return GetRandomView(horizontalBlockViewVariations);
    }

    public GameObject GetRandomVerticalBlockView()
    {
        return GetRandomView(verticalBlockViewVariations);
    }

    public GameObject GetRandomLeftDownBlockCornerView()
    {
        return GetRandomView(leftDownCornerViewVariations);
    }

    public GameObject GetRandomRightDownBlockCornerView()
    {
        return GetRandomView(rightDownCornerViewVariations);
    }

    public GameObject GetRandomLeftUpBlockCornerView()
    {
        return GetRandomView(leftUpCornerViewVariations);
    }

    public GameObject GetRandomRightUpBlockCornerView()
    {
        return GetRandomView(rightUpCornerViewVariations);
    }

    public GameObject GetRandomUpTransitionView()
    {
        return GetRandomView(upTransitionViewVariations);
    }

    public GameObject GetRandomDownTransitionView()
    {
        return GetRandomView(downTransitionViewVariations);
    }

    public GameObject GetRandomLeftTransitionView()
    {
        return GetRandomView(leftTransitionViewVariations);
    }

    public GameObject GetRandomRightTransitionView()
    {
        return GetRandomView(rightTransitionViewVariations);
    }

    private GameObject GetRandomView(ViewInfo[] collection)
    {
        float maxChance = MaxChance(collection);
        float randomValue = UnityEngine.Random.Range(0, maxChance);

        float value = 0;
        for (int i = 0; i < collection.Length; i++)
        {
            if (randomValue >= value && randomValue < collection[i].chanse)
                return collection[i].view;

            value += collection[i].chanse;
        }

        return collection.Last().view;
    }

    private float MaxChance(ViewInfo[] collection)
    {
        return collection.Sum(n => n.chanse);
    }
}

[Serializable]
public struct TileKeyPair
{
    [SerializeField] public TileInfo value;
    public HallType key;
}

public class TilesManager : MonoBehaviour
{
    [SerializeField] private List<TileKeyPair> _tileKeyPairs = new List<TileKeyPair>();
    private Dictionary<HallType, TileInfo> _tileCollections = new Dictionary<HallType, TileInfo>();




    public Dictionary<HallType, TileInfo> TileCollections
    {
        get
        {
            ValidateTileCollections();
            return _tileCollections;
        }
    }




    private void OnValidate()
    {
        ValidateTileCollections();
    }

    private void ValidateTileCollections()
    {
        _tileCollections.Clear();

        if (_tileKeyPairs.Count == 0)
            return;

        ChangeLastToDefault();

        for (int i = 0; i < _tileKeyPairs.Count; i++)
        {
            if (_tileCollections.TryGetValue(_tileKeyPairs[i].key, out TileInfo tileInfo))
            {
                _tileKeyPairs.RemoveAt(i);
                i--;
                continue;
            }

            _tileCollections.Add(_tileKeyPairs[i].key, _tileKeyPairs[i].value);
        }




        void ChangeLastToDefault()
        {
            if (_tileKeyPairs.Count <= 1)
                return;

            TileKeyPair lastPair = _tileKeyPairs.Last();
            lastPair.key = FindNotUseTileType();
            _tileKeyPairs[_tileKeyPairs.Count - 1] = lastPair;
        }




        HallType FindNotUseTileType()
        {
            for (int i = 0; i < (int)HallType.Simple; i++)
            {
                if (_tileKeyPairs.Where(n => n.key == (HallType)i).Count() == 0)
                    return (HallType)i;
            }

            return HallType.Simple;
        }
    }
}
