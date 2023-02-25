using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private Collider _collider;




    private Collider Collider
    {
        get => _collider = _collider ??= GetComponent<Collider>();
    }



    public event Action OnTriggerEntered;
    public event Action OnTriggerExited;




    private void Start()
    {
        Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        char[] binary = Convert.ToString(_layerMask.value, 2).ToCharArray().Reverse().ToArray();

        if(binary[other.gameObject.layer] == '1')
        {
            OnTriggerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        char[] binary = Convert.ToString(_layerMask.value, 2).ToCharArray().Reverse().ToArray();

        if (binary[other.gameObject.layer] == '1')
        {
            OnTriggerExited?.Invoke();
        }
    }
}
