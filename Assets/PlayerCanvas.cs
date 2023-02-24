using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public GameObject hpIndicator;
    public GameObject hpPoint;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < hpIndicator.transform.childCount; i++)
        {
            Destroy(hpIndicator.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _player.hp; i++)
        {
            var point = Instantiate(hpPoint, hpIndicator.transform);
            point.SetActive(true);
        }
    }
}
