using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Full,
    LeftOff,
    RightOff,
    UpOff,
    DownOff
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




    [Button]
    public void FillRoomWithHallways()
    {
        switch (_roomType)
        {
            case RoomType.Full:
               CreateUpHall();
                CreateDownHall();
                CreateLeftHall();
                CreateRightHall();
                break;
            case RoomType.LeftOff:
                CreateUpHall();
                CreateDownHall();
                CreateRightHall();
                break;
            case RoomType.RightOff:
                CreateUpHall();
                CreateDownHall();
                CreateLeftHall();
                break;
            case RoomType.UpOff:
                CreateDownHall();
                CreateLeftHall();
                CreateRightHall();
                break;
            case RoomType.DownOff:
                CreateUpHall();
                CreateLeftHall();
                CreateRightHall();
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

    private void CreateUpHall()
    {
        Transform hall = Instantiate(HallwayHandler.GetRandomVerticalHall(), _upHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateDownHall()
    {
        Transform hall = Instantiate(HallwayHandler.GetRandomVerticalHall(), _downHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateLeftHall()
    {
        Transform hall = Instantiate(HallwayHandler.GetRandomHorizontalHall(), _leftHallParent).transform;
        hall.localPosition = Vector3.zero;
    }

    private void CreateRightHall()
    {
        Transform hall = Instantiate(HallwayHandler.GetRandomHorizontalHall(), _rightHallParent).transform;
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
