using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct RoomHandlersInfo
{
    public RoomHandler variation;
    public float chance;
}

public class MapHandler : MonoBehaviour
{
    [SerializeField] private RoomHandlersInfo[] _roomVariations;
    [SerializeField] private Vector2 _multPos = new Vector2(60, 50);
    [SerializeField] private int _startIndex = 0;

    private Dictionary<Vector2, RoomHandler> _map = new Dictionary<Vector2, RoomHandler>();




    private void Start()
    {
        _map.Add(Vector2.zero, Instantiate(_roomVariations[_startIndex].variation, Vector2.zero, Quaternion.identity, transform));
        _map[Vector2.zero].RoomType = RoomType.Full;
        _map[Vector2.zero].FillRoomWithHallways();

        InitUpRoom(Vector2.zero);
        InitDownRoom(Vector2.zero);
        InitRightRoom(Vector2.zero);
        InitLeftRoom(Vector2.zero);
    }

    private void CreateUpRoom(Vector2 currentKey)
    {
        Vector2 newKey = currentKey + Vector2.up;

        if (_map.ContainsKey(newKey))
            return;

        _map.Add(newKey, Instantiate(GetRandomRoom(), new Vector3(newKey.x * _multPos.x, 0, newKey.y * _multPos.y), Quaternion.identity, transform));
        _map[newKey].RoomType = RoomType.DownOff;
        _map[newKey].FillRoomWithHallways(
            !_map.ContainsKey(newKey + Vector2.up),
           !_map.ContainsKey(newKey + Vector2.down),
           !_map.ContainsKey(newKey + Vector2.right),
            !_map.ContainsKey(newKey + Vector2.left));

        InitUpRoom(newKey);
        InitLeftRoom(newKey);
        InitRightRoom(newKey);
    }

    private void CreateDownRoom(Vector2 currentKey)
    {
        Vector2 newKey = currentKey + Vector2.down;

        if (_map.ContainsKey(newKey))
            return;

        _map.Add(newKey, Instantiate(GetRandomRoom(), new Vector3(newKey.x * _multPos.x, 0, newKey.y * _multPos.y), Quaternion.identity, transform));
        _map[newKey].RoomType = RoomType.UpOff;
        _map[newKey].FillRoomWithHallways(
            !_map.ContainsKey(newKey + Vector2.up),
           !_map.ContainsKey(newKey + Vector2.down),
           !_map.ContainsKey(newKey + Vector2.right),
            !_map.ContainsKey(newKey + Vector2.left));

        InitDownRoom(newKey);
        InitLeftRoom(newKey);
        InitRightRoom(newKey);
    }

    private void CreateRightRoom(Vector2 currentKey)
    {
        Vector2 newKey = currentKey + Vector2.right;

        if (_map.ContainsKey(newKey))
            return;

        _map.Add(newKey, Instantiate(GetRandomRoom(), new Vector3(newKey.x * _multPos.x, 0, newKey.y * _multPos.y), Quaternion.identity, transform));
        _map[newKey].RoomType = RoomType.LeftOff;
        _map[newKey].FillRoomWithHallways(
            !_map.ContainsKey(newKey + Vector2.up),
           !_map.ContainsKey(newKey + Vector2.down),
           !_map.ContainsKey(newKey + Vector2.right),
            !_map.ContainsKey(newKey + Vector2.left));

        InitUpRoom(newKey);
        InitDownRoom(newKey);
        InitRightRoom(newKey);
    }

    private void CreateLeftRoom(Vector2 currentKey)
    {
        Vector2 newKey = currentKey + Vector2.left;

        if (_map.ContainsKey(newKey))
            return;

        _map.Add(newKey, Instantiate(GetRandomRoom(), new Vector3(newKey.x * _multPos.x, 0, newKey.y * _multPos.y), Quaternion.identity, transform));
        _map[newKey].RoomType = RoomType.RightOff;
        _map[newKey].FillRoomWithHallways(
            !_map.ContainsKey(newKey + Vector2.up),
           !_map.ContainsKey(newKey + Vector2.down),
           !_map.ContainsKey(newKey + Vector2.right),
            !_map.ContainsKey(newKey + Vector2.left));

        InitUpRoom(newKey);
        InitDownRoom(newKey);
        InitLeftRoom(newKey);
    }

    private void InitUpRoom(Vector2 key)
    {
        _map[key].InitializeUpTrigger(OnUpEnter(key), OnUpExit(key));
        _map[key].InitializeRightTrigger(OnRightEnter(key), OnRightExit(key));
        _map[key].InitializeLeftTrigger(OnLeftEnter(key), OnLeftExit(key));
    }

    private void InitDownRoom(Vector2 key)
    {
        _map[key].InitializeRightTrigger(OnRightEnter(key), OnRightExit(key));
        _map[key].InitializeLeftTrigger(OnLeftEnter(key), OnLeftExit(key));
        _map[key].InitializeDownTrigger(OnDownEnter(key), OnDownExit(key));
    }

    private void InitLeftRoom(Vector2 key)
    {
        _map[key].InitializeLeftTrigger(OnLeftEnter(key), OnLeftExit(key));
        _map[key].InitializeUpTrigger(OnUpEnter(key), OnUpExit(key));
        _map[key].InitializeDownTrigger(OnDownEnter(key), OnDownExit(key));
    }

    private void InitRightRoom(Vector2 key)
    {
        _map[key].InitializeRightTrigger(OnRightEnter(key), OnRightExit(key));
        _map[key].InitializeUpTrigger(OnUpEnter(key), OnUpExit(key));
        _map[key].InitializeDownTrigger(OnDownEnter(key), OnDownExit(key));
    }

    private Action OnUpEnter(Vector2 key)
    {
        void OnEnter()
        {
            CreateUpRoom(key);
            _map[key].DeleteUpTrigger();
        }

        return OnEnter;
    }

    private Action OnDownEnter(Vector2 key)
    {
        void OnEnter()
        {
            CreateDownRoom(key);
            _map[key].DeleteDownTrigger();
        }

        return OnEnter;
    }

    private Action OnLeftEnter(Vector2 key)
    {
        void OnEnter()
        {
            CreateLeftRoom(key);
            _map[key].DeleteLeftTrigger();
        }

        return OnEnter;
    }

    private Action OnRightEnter(Vector2 key)
    {
        void OnEnter()
        {
            CreateRightRoom(key);
            _map[key].DeleteRightTrigger();
        }

        return OnEnter;
    }

    private Action OnUpExit(Vector2 key)
    {
        void OnExit()
        {

        }

        return OnExit;
    }

    private Action OnDownExit(Vector2 key)
    {
        void OnExit()
        {

        }

        return OnExit;
    }

    private Action OnLeftExit(Vector2 key)
    {
        void OnExit()
        {

        }

        return OnExit;
    }

    private Action OnRightExit(Vector2 key)
    {
        void OnExit()
        {

        }

        return OnExit;
    }

    private RoomHandler GetRandomRoom()
    {
        float maxChance = GetMaxChance(_roomVariations);
        float randomValue = UnityEngine.Random.Range(0, maxChance);
        float currentDownChance = 0;

        if (_roomVariations == null || _roomVariations.Length == 0)
            throw new ArgumentNullException($"{this}, {nameof(HallwayInfo)} collection is not contain items, or equal null.");

        for (int i = 0; i < _roomVariations.Length; i++)
        {
            if (randomValue >= currentDownChance && randomValue < currentDownChance + _roomVariations[i].chance)
                return _roomVariations[i].variation;

            currentDownChance += _roomVariations[i].chance;
        }

        return _roomVariations.Last().variation;
    }

    private float GetMaxChance(RoomHandlersInfo[] collection)
    {
        float count = 0;

        for (int i = 0; i < collection.Length; i++)
        {
            count += collection[i].chance;
        }

        return count;
    }
}
