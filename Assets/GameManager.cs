using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Hero Parameters")]
    [SerializeField] private float _hp;
    [SerializeField] private float _hungryLevel;
    [SerializeField] private float _thirthLevel;
    [SerializeField] private float _restLevel;
    [SerializeField] private int _gold;
    [SerializeField] private float _maxTime = 9999;

    [Space(10)]

    [Header("Gameplay")]
    [SerializeField] private float _nextStepTimeMult = 0.75f;

    [MaxValue(0)]
    [SerializeField] private float _maxHungryPerStep = -1;

    [MaxValue(0)]
    [SerializeField] private float _maxThirstPerStep = -1;

    [MaxValue(0)]
    [SerializeField] private float _maxRestPerStep = -1;

    [MinValue(0)]
    [SerializeField] private float _maxHpPerStep = 1;

    [MinValue(0), MaxValue(1)]
    [SerializeField] private float _minimalRestMult = 0.25f;

    [Space(3)]
    [Header("Hotel")]
    [SerializeField] private float _hotelMoneyPay = 100;
    [SerializeField] private float _hotelDamage;

    [Space(3)]
    [Header("Sands")]
    [SerializeField] private float _sandThirstAdd;

    [Space(3)]
    [Header("Smoke")]
    [SerializeField] private float _smokeHungryAdd;

    [Space(3)]
    [Header("River")]
    [SerializeField] private float _riverHungryAdd;

    [Space(3)]
    [Header("Fair")]
    [SerializeField] private float _fairHungryAdd;

    [Space(3)]
    [Header("Mushroom")]
    [SerializeField] private float _mushroomHungryAdd;
    [SerializeField] private float _mushroomDamage;

    [MinValue(0), MaxValue(1)]
    [SerializeField] private float _mushroomPositiveChance = 0.5f;

    private float _maxHp;
    private float _maxHungryLevel;
    private float _maxThirthLevel;
    private float _maxRestLevel;
    private float _currentMaxTime;
    private float _currentTime;
    private int _currentStepNumber = 0;

    public static GameManager self;



    public float CurrentTimeProportion
    {
        get => _currentTime / _currentMaxTime;
    }

    public float CurrentHpProportion
    {
        get => _hp / _maxHp;
    }

    public float CurrentRestProportion
    {
        get => _restLevel / _maxRestLevel;
    }

    public float CurrentHungryProportion
    {
        get => _hungryLevel / _maxHungryLevel;
    }

    public float CurrentThirstProportion
    {
        get => _thirthLevel / _maxThirthLevel;
    }

    public float GoldCount => _gold;



    private void Awake()
    {
        _currentTime = _maxTime;
        _maxHp = _hp;
        _maxHungryLevel = _hungryLevel;
        _maxThirthLevel = _thirthLevel;
        _maxRestLevel = _restLevel;
        _currentMaxTime = _maxTime;

        if (self == null)
            self = this;
    }

    private void FixedUpdate()
    {
        _currentTime -= Time.fixedDeltaTime * GetRestTimeMult();



        float GetRestTimeMult()
        {
            return _minimalRestMult + Mathf.Lerp(0, 1 - _minimalRestMult, CurrentRestProportion);
        }
    }

    public void GoToNextPath(HallType tileType)
    {
        _currentStepNumber++;

        RefreshTime();
        ApplyEffectFromPath(tileType);
        ApplyEffectForPath();
    }

    private void ApplyEffectForPath()
    {
        ChangeHungryLevel(_maxHungryPerStep);
        ChangeThirthLevel(_maxThirstPerStep);

        ChangeRestLevel(_maxRestPerStep * Mathf.Lerp(-1, 1, CurrentHungryProportion));
        TakeDamage(_maxHpPerStep * Mathf.Lerp(1, 0, CurrentThirstProportion));
    }

    private void ApplyEffectFromPath(HallType tileType)
    {
        switch (tileType)
        {
            case HallType.River:
                ChangeThirthLevel(_sandThirstAdd);
                break;

            case HallType.Sand:
                ChangeThirthLevel(_sandThirstAdd);
                break;

            case HallType.Mushroom:
                TryMushroom();
                break;

            case HallType.GarbagePath:
                ChangeHungryLevel(_fairHungryAdd);
                break;

            case HallType.Smoke:
                ChangeHungryLevel(_smokeHungryAdd);
                break;

            case HallType.Hotel:
                TryHotel();
                break;

            case HallType.Simple:
                break;
        }


        void TryHotel()
        {
            if (_gold > _hotelMoneyPay)
                FullRegenerate();
            else
                TakeDamage(_hotelDamage);
        }

        void TryMushroom()
        {
            float randomValue = Random.Range(0, 1);

            if (randomValue >= _mushroomPositiveChance)
                TakeMushroomNegativeEfect();
            else
                TakeMushroomPositiveEffect();
        }
    }

    public void FullRegenerate()
    {
        _restLevel = _maxRestLevel;
        _hp = _maxHp;
        _hungryLevel = _maxHungryLevel;
        _thirthLevel = _maxThirthLevel;
    }

    public void TakeMushroomPositiveEffect()
    {
        TakeDamage(-_mushroomDamage);
        ChangeHungryLevel(_mushroomHungryAdd);
    }

    public void TakeMushroomNegativeEfect()
    {
        TakeDamage(_mushroomDamage);
        ChangeHungryLevel(-_mushroomHungryAdd);
    }

    public void AddGold(int addition)
    {
        _gold += addition;
        _gold = Mathf.Clamp(_gold, 0, 999);
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage * GetRestMult();
        _hp = Mathf.Clamp(_hp, 0, _maxHp);



        float GetRestMult()
        {
            float restMult = 1;

            if (damage > 0)
                restMult = 1 - CurrentRestProportion;

            return restMult;
        }
    }

    public void ChangeHungryLevel(float addition)
    {
        _hungryLevel += addition;
        _hungryLevel = Mathf.Clamp(_hungryLevel, 0, _maxHungryLevel);
    }

    public void ChangeThirthLevel(float addition)
    {
        _thirthLevel += addition;
        _thirthLevel = Mathf.Clamp(_thirthLevel, 0, _maxThirthLevel);
    }

    public void ChangeRestLevel(float addition)
    {
        _restLevel += addition;
        _restLevel = Mathf.Clamp(_restLevel, 0, _maxRestLevel);
    }

    private void RefreshTime()
    {
        _currentMaxTime = _maxTime * Mathf.Pow(_nextStepTimeMult, _currentStepNumber);
        _currentTime = _currentMaxTime;
    }
}
