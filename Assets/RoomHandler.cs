using NaughtyAttributes;
using System;
using UnityEngine;

public enum RoomType
{
    Full,
    LeftOff,
    RightOff,
    UpOff,
    DownOff,
}

[RequireComponent(typeof(HallwayHandler))]
public class RoomHandler : MonoBehaviour
{
    [Required]
    [SerializeField] private Transform _leftHallParent;

    [Required]
    [SerializeField] private Transform _rightHallParent;

    [Required]
    [SerializeField] private Transform _upHallParent;

    [Required]
    [SerializeField] private Transform _downHallParent;



    [Required]
    [SerializeField] private TriggerController _leftTrigger;

    [Required]
    [SerializeField] private TriggerController _rightTrigger;

    [Required]
    [SerializeField] private TriggerController _upTrigger;

    [Required]
    [SerializeField] private TriggerController _downTrigger;



    [SerializeField] private RoomType _roomType;

    private HallwayHandler _hallwayHandler;




    private HallwayHandler HallwayHandler
    {
        get => _hallwayHandler = _hallwayHandler ??= GetComponent<HallwayHandler>();
    }

    public RoomType RoomType
    {
        get => _roomType;
        set => _roomType = value;
    }

    public HallwayInfo UpHall { get; private set; }
    public HallwayInfo DownHall { get; private set; }
    public HallwayInfo RightHall { get; private set; }
    public HallwayInfo LeftHall { get; private set; }



    [Button]
    public void FillRoomWithHallways(bool includeUp = true, bool includeDown = true, bool includeRight = true, bool includeLeft = true)
    {
        switch (_roomType)
        {
            case RoomType.Full:
                if (includeUp) CreateUpHall(); else DeleteUpTrigger();
                if (includeDown) CreateDownHall(); else DeleteDownTrigger();
                if (includeLeft) CreateLeftHall(); else DeleteLeftTrigger();
                if (includeRight) CreateRightHall(); else DeleteRightTrigger();
                break;
            case RoomType.LeftOff:
                if (includeUp) CreateUpHall(); else DeleteUpTrigger();
                if (includeDown) CreateDownHall(); else DeleteDownTrigger();
                if (includeRight) CreateRightHall(); else DeleteRightTrigger();

                DeleteLeftTrigger();
                break;
            case RoomType.RightOff:
                if (includeUp) CreateUpHall(); else DeleteUpTrigger();
                if (includeDown) CreateDownHall(); else DeleteDownTrigger();
                if (includeLeft) CreateLeftHall(); else DeleteLeftTrigger();

                DeleteRightTrigger();
                break;
            case RoomType.UpOff:
                if (includeDown) CreateDownHall(); else DeleteDownTrigger();
                if (includeLeft) CreateLeftHall(); else DeleteLeftTrigger();
                if (includeRight) CreateRightHall(); else DeleteRightTrigger();

                DeleteUpTrigger();
                break;
            case RoomType.DownOff:
                if (includeUp) CreateUpHall(); else DeleteUpTrigger();
                if (includeLeft) CreateLeftHall(); else DeleteLeftTrigger();
                if (includeRight) CreateRightHall(); else DeleteRightTrigger();

                DeleteDownTrigger();
                break;
        }
    }

    [Button]
    public void ClearRoomWithHallways()
    {
        RemoveAllChildren(_upHallParent);
        RemoveAllChildren(_rightHallParent);
        RemoveAllChildren(_leftHallParent);
        RemoveAllChildren(_downHallParent);
    }

    public void InitializeUpTrigger(Action onTriggerEntered, Action onTriggerExited)
    {
        _upTrigger.OnTriggerEntered += onTriggerEntered;
        _upTrigger.OnTriggerEntered += onTriggerExited;
    }

    public void DeleteUpTrigger()
    {
        Destroy(_upTrigger.gameObject);
    }

    public void InitializeDownTrigger(Action onTriggerEntered, Action onTriggerExited)
    {
        _downTrigger.OnTriggerEntered += onTriggerEntered;
        _downTrigger.OnTriggerEntered += onTriggerExited;
    }

    public void DeleteDownTrigger()
    {
        Destroy(_downTrigger.gameObject);
    }

    public void InitializeRightTrigger(Action onTriggerEntered, Action onTriggerExited)
    {
        _rightTrigger.OnTriggerEntered += onTriggerEntered;
        _rightTrigger.OnTriggerEntered += onTriggerExited;
    }

    public void DeleteRightTrigger()
    {
        Destroy(_rightTrigger.gameObject);
    }

    public void InitializeLeftTrigger(Action onTriggerEntered, Action onTriggerExited)
    {
        _leftTrigger.OnTriggerEntered += onTriggerEntered;
        _leftTrigger.OnTriggerEntered += onTriggerExited;
    }

    public void DeleteLeftTrigger()
    {
        Destroy(_leftTrigger.gameObject);
    }

    private void CreateUpHall()
    {
        UpHall = HallwayHandler.GetRandomVerticalHall();
        Transform hall = Instantiate(UpHall.hallVariation, _upHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateDownHall()
    {
        DownHall = HallwayHandler.GetRandomVerticalHall();
        Transform hall = Instantiate(DownHall.hallVariation, _downHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateLeftHall()
    {
        LeftHall = HallwayHandler.GetRandomHorizontalHall();
        Transform hall = Instantiate(LeftHall.hallVariation, _leftHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateRightHall()
    {
        RightHall = HallwayHandler.GetRandomHorizontalHall();
        Transform hall = Instantiate(RightHall.hallVariation, _rightHallParent).transform;
        hall.localPosition = Vector3.zero;
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
