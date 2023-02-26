using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    public int _activateCountOnAwake = 3;

    private void Awake()
    {
        GameObject[] children = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }

        GameObject[] activateChildren = children.OrderBy(x => Random.Range(0, transform.childCount)).Take(3).ToArray();

        for (int i = 0; i < activateChildren.Length; i++)
        {
            activateChildren[i].SetActive(true);
        }
    }
}
