using SimpleMan.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMan.AsyncOperationsDemo
{
    public class DelayAsyncExample : MonoBehaviour
    {
        [SerializeField] private Button _uiButton;
        [SerializeField] private GameObject _prefab;

        [Range(0, 3)]
        [SerializeField] private float _delayTime = 1;




        private void OnEnable()
        {
            _uiButton.onClick.AddListener(ButtonClicked);
        }

        private void OnDisable()
        {
            _uiButton.onClick.RemoveListener(ButtonClicked);
        }

        private async void ButtonClicked()
        {
            SpawnObject();
            DisableButton();

            //Call async operation and get result of it. 
            EAsyncOperationResult asyncOperationResult = await SafeAsync.DelayRealtime(_delayTime, null);

            //Always check the result of the async operation. If result is 'Canceled by system', it means 
            //that you are not in play mode anymore and you should finish this operation immidiate. Async is not 
            //really safe thing in Unity. 
            //If you ignore this rule, it can cause executing your code in editor mode.
            if (asyncOperationResult == EAsyncOperationResult.CanceledBySystem)
                return;

            EnableButton();
        }

        private void EnableButton()
        {
            _uiButton.interactable = true;
        }

        private void DisableButton()
        {
            _uiButton.interactable = false;
        }

        private void SpawnObject()
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}

