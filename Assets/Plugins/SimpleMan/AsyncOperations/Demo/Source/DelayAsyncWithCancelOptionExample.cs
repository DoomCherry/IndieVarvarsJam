using SimpleMan.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMan.AsyncOperationsDemo
{
    public class DelayAsyncWithCancelOptionExample : MonoBehaviour
    {
        [SerializeField] private Button _spawnButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _prefab;

        [Range(0, 3)]
        [SerializeField] private float _delayTime = 1;

        private bool _isOperationCanceled;




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

        private async void SpawnButtonClicked()
        {
            _isOperationCanceled = false;

            SpawnObject();
            DisableSpawnButton();

            //Call async operation and get result of it. 
            EAsyncOperationResult result = await SafeAsync.DelayRealtime(_delayTime, () => _isOperationCanceled);

            //Always check the result of the async operation. If result is 'Canceled by system', it means 
            //that you are not in play mode anymore and you should finish this operation immidiate. Async is not 
            //really safe thing in Unity. 
            //If you ignore this rule, it can cause executing your code in editor mode.
            if (result == EAsyncOperationResult.CanceledBySystem)
                return;

            EnableSpawnButton();
        }

        private void CancelButtonClicked()
        {
            _isOperationCanceled = true;
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

