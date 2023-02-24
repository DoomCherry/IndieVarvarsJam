using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _wayTiles;
    [SerializeField] private GameObject[] _horizontalWallTiles;
    [SerializeField] private GameObject[] _verticalWallTiles;
    [SerializeField] private GameObject[] _leftDownCorners;
    [SerializeField] private GameObject[] _rightDownCorners;
    [SerializeField] private GameObject[] _leftUpCorners;
    [SerializeField] private GameObject[] _rightUpCorners;

    [SerializeField] private TileType _defaultTileType;
    private TilesManager _tilesManager;




    public TilesManager TilesManager
    {
        get => _tilesManager = _tilesManager ??= GetComponent<TilesManager>();
    }




    [Button]
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

    [Button]
    public void SetWallTo(TileType tileType)
    {
        ClearAllWallTypes();

        for (int i = 0; i < _wayTiles.Length; i++)
        {
            Instantiate(TilesManager.TileCollections[tileType].GetRandomWayView(), _wayTiles[i].transform);
        }
    }

    [Button]
    public void ClearAllWallTypes()
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
