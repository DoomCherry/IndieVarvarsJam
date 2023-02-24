using SimpleMan.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMan.AsyncOperationsDemo
{
    public class DelayExample : MonoBehaviour
    {
        [SerializeField] private Button _spawnButton;
        [SerializeField] private GameObject _prefab;

        [Range(0, 3)]
        [SerializeField] private float _delayTime = 1;




        private void OnEnable()
        {
            _spawnButton.onClick.AddListener(SpawnButtonClicked);
        }

        private void OnDisable()
        {
            _spawnButton.onClick.RemoveListener(SpawnButtonClicked);
        }

        private void SpawnButtonClicked()
        {
            //This method allows you to run delay coroutine. 'Enable button' method will be
            //invoked after delay, but code below will be executed normally in current frame.
            this.Delay(_delayTime, EnableSpawnButton);

            SpawnObject();
            DisableSpawnButton();
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

