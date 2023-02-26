using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    public int _activateCountMaxOnAwake = 3;
    public int _activateCountMinOnAwake = 0;

    private void Awake()
    {
        int activateCount = Random.Range(_activateCountMinOnAwake, _activateCountMaxOnAwake + 1);

        GameObject[] children = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }

        GameObject[] activateChildren = children.OrderBy(x => Random.Range(0, transform.childCount)).Take(activateCount).ToArray();

        for (int i = 0; i < activateChildren.Length; i++)
        {
            activateChildren[i].SetActive(true);
        }
    }
}
