using SimpleMan.AsyncOperations;
using SimpleMan.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMan.AsyncOperationsDemo
{
    public class DelayWithCancelOptionExample : MonoBehaviour
    {
        [SerializeField] private Button _spawnButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _prefab;

        [Range(0, 3)]
        [SerializeField] private float _delayTime = 1;

        //You need to cache your coroutine to have ability to stop it
        private Coroutine _delayRoutine;




        private void OnEnable()
        {
            _spawnButton.onClick.AddListener(SpawnButtonClicked);
            _cancelButton.onClick.AddListener(CancelButtonClicked);
        }

        private void OnDisable()
        {
            _spawnButton.onClick.RemoveListener(SpawnButtonClicked);
            _cancelButton.onClick.RemoveListener(CancelButtonClicked);
        }

        private void SpawnButtonClicked()
        {
            //Use 'Delay' fuction by the same way as in previous example, but with caching
            //returned 'Coroutine' class into the field
            _delayRoutine = this.Delay(_delayTime, EnableSpawnButton);

            SpawnObject();
            DisableSpawnButton();
        }

        private void CancelButtonClicked()
        {
            //Make sure that coroutine class exist (is not null)
            //and stop this coroutine
            if (_delayRoutine.Exist())
                StopCoroutine(_delayRoutine);

            //The button need to be enabled manually, because the 
            //coroutine has been canceled, and it will never call 'On complete' 
            //delegate
            EnableSpawnButton();
        }

        private void EnableSpawnButton()
        {
            _spawnButton.interactable = true;
        }

        private void DisableSpawnButton()
        {
            _spawnButton.interactable = false;
        }

        private void SpawnObject()
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}

