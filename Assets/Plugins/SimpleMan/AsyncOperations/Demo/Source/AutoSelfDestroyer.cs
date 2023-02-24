using SimpleMan.AsyncOperations;
using UnityEngine;

namespace SimpleMan.AsyncOperationsDemo
{
    public class AutoSelfDestroyer : MonoBehaviour
    {
        [Range(1, 5)]
        [SerializeField] private float _timeToDestroy = 2;




        private void Start()
        {
            //You can use lambda expressions and send the parameters using this constuction:
            //() => YourMethod(someParameter);
            //() => YourMethod(firstParameter, secondParameter);
            this.Delay(_timeToDestroy, () => Destroy(gameObject));
        }
    }
}

