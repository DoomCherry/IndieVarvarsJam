using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HallwayType
{
    horizontal,
    vertical
}

[RequireComponent(typeof(TilesManager))]
public class HallwayHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _horizontalWallTiles;
    [SerializeField] private GameObject[] _verticalWallTiles;
    [SerializeField] private GameObject[] _wayTiles;
    [SerializeField] private TileType _defaultTileType;

    private HallwayType _hallwayType;
    private TilesManager _tilesManager;




    public HallwayType HallwayType
    {
        get
        {
            return _hallwayType;
        }

        set
        {
            _hallwayType = value;

            if (_hallwayType == HallwayType.horizontal)
                OffVerticalWalls();
            else
                OffHorizontalWalls();
        }
    }
    public TilesManager TilesManager
    {
        get => _tilesManager = _tilesManager ??= GetComponent<TilesManager>();
    }



    [Button]
    public void OnVerticalWalls()
    {
        for (int i = 0; i < _verticalWallTiles.Length; i++)
        {
            _verticalWallTiles[i].SetActive(true);
        }
    }

    [Button]
    public void OnHorizontalWalls()
    {
        for (int i = 0; i < _horizontalWallTiles.Length; i++)
        {
            _horizontalWallTiles[i].SetActive(true);
        }
    }

    [Button]
    public void OffVerticalWalls()
    {
        for (int i = 0; i < _verticalWallTiles.Length; i++)
        {
            _verticalWallTiles[i].SetActive(false);
        }
    }

    [Button]
    public void OffHorizontalWalls()
    {
        for (int i = 0; i < _horizontalWallTiles.Length; i++)
        {
            _horizontalWallTiles[i].SetActive(false);
        }
    }

    [Button]
    public void SetWallToDefaule()
    {
        SetWayTo(_defaultTileType);
    }

    public void SetWayTo(TileType tileType)
    {
        ClearAllWayTypes();

        for (int i = 0; i < _wayTiles.Length; i++)
        {
            Instantiate(TilesManager.TileCollections[tileType].GetRandomWayView(), _wayTiles[i].transform);
        }
    }

    [Button]
    public void ClearAllWayTypes()
    {
        for (int i = 0; i < _wayTiles.Length; i++)
        {
            RemoveAllChildren(_wayTiles[i].transform);
        }
    }

    private void RemoveAllChildren(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
