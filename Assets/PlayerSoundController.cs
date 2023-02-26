using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _leftStepVariation;
    [SerializeField] private AudioSource[] _rightStepVariation;
    [SerializeField] private AudioSource[] _eatingVariation;
    [SerializeField] private AudioSource[] _drinkingVariation;
    [SerializeField] private AudioSource[] _coinVariationVariation;
    [SerializeField] private AudioSource[] _takeSomethingVariation;
    [SerializeField] private AudioSource[] _bombTickVariation;
    [SerializeField] private float _stepsDelay = 0.5f;
    [SerializeField] private Player _playerRef;

    private Vector3 _lastPosition;
    private int _stepCount = 0;

    private void Start()
    {
        this.RepeatForever(PlayRandomStep, _stepsDelay);
        _lastPosition = transform.position;

        _playerRef.OnDrinkSomething += PlayRandomDrink;
        _playerRef.OnEatSomething += PlayRandomEat;
        _playerRef.OnTakeGoldSomething += PlayRandomGold;
        _playerRef.OnTakeSomething += PlayRandomTake;
    }

    private void PlayRandomStep()
    {
        if (_lastPosition != transform.position)
        {
            if (_stepCount % 2 == 0)
                PlayRandomRightStep();
            else
                PlayRandomLeftStep();

            _lastPosition = transform.position;
            _stepCount++;
        }
    }

    private void PlayRandomRightStep()
    {
        PlayRandom(_rightStepVariation);
    }

    private void PlayRandomLeftStep()
    {
        PlayRandom(_leftStepVariation);
    }

    private void PlayRandomEat()
    {
        PlayRandom(_eatingVariation);
    }

    private void PlayRandomDrink()
    {
        PlayRandom(_drinkingVariation);
    }

    private void PlayRandomGold()
    {
        PlayRandom(_coinVariationVariation);
    }

    private void PlayRandomTake()
    {
        PlayRandom(_takeSomethingVariation);
    }

    private void PlayRandom(AudioSource[] collection)
    {
        int rndValue = Random.Range(0, collection.Length);
        collection[rndValue].Play();
    }
}
