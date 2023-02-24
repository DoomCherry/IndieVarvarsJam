using SimpleMan.AsyncOperations;
using SimpleMan.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMan.AsyncOperationsDemo
{
    public class CustomTimeScaleExample : MonoBehaviour
    {
        [SerializeField] private Button _spawnButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _prefab;

        [Range(0, 3)]
        [SerializeField] private float _delayTime = 1;

        [Range(0, 3)]
        [SerializeField] private float _customTimeScale = 1f;

        //You need to cache your coroutine to have ability to stop it
        private Coroutine _routine;




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
            //Use this method to set custom time scale for coroutine
            _routine = this.RepeatForeverCustomTimeScale(SpawnObject, () => _customTimeScale, _delayTime);

            DisableSpawnButton();
            EnableCancelButton();
        }

        private void CancelButtonClicked()
        {
            //Make sure that coroutine class exist (is not null)
            //and stop this coroutine
            if (_routine.Exist())
                StopCoroutine(_routine);

            //Enable spawn button again
            DisableCancelButton();
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

        private void EnableCancelButton()
        {
            _cancelButton.interactable = true;
        }

        private void DisableCancelButton()
        {
            _cancelButton.interactable = false;
        }

        private void SpawnObject()
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}

