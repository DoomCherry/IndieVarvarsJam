using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delay = 10;
    public GameObject prefab;


    private void Start()
    {
        this.RepeatForever(Spawn, delay);
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
