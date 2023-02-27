using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DrakonAwaker : MonoBehaviour
{
    public Drakon drakon;
    public Stone stone;
    public GameObject canvas;
    private Player _player;


    public BoxCollider Trigger => GetComponent<BoxCollider>();


    private void Start()
    {
        _player = FindObjectOfType<Player>();
        Trigger.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _player.gameObject)
        {
            this.enabled = false;
            canvas.SetActive(true);

            Destroy(stone.gameObject);

            drakon.gameObject.SetActive(true);
        }
    }
}
