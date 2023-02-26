using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCanvasController : MonoBehaviour
{
    public Slider time;
    public Image hp;
    public Image hungry;
    public Image thirst;
    public Image rest;
    public TMPro.TextMeshProUGUI goldCount;

    public string _viewAdditionTriggerName = "View";
    public Color _neitralColor;
    public Color _positiveColor;
    public Color _negativeColor;

    public TMPro.TextMeshProUGUI _timeAddition;
    public TMPro.TextMeshProUGUI _hpAddition;
    public TMPro.TextMeshProUGUI _hungryAddition;
    public TMPro.TextMeshProUGUI _thirstAddition;
    public TMPro.TextMeshProUGUI _restAddition;
    public TMPro.TextMeshProUGUI _goldAddition;

    private Animator _timeAdditionAnimator;
    private Animator _hpAdditionAnimator;
    private Animator _hungryAdditionAnimator;
    private Animator _thirstAdditionAnimator;
    private Animator _restAdditionAnimator;
    private Animator _goldAdditionAnimator;



    public Animator TimeAdd => _timeAdditionAnimator = _timeAdditionAnimator ??= _timeAddition.GetComponent<Animator>();
    public Animator HpAdd => _hpAdditionAnimator = _hpAdditionAnimator ??= _hpAddition.GetComponent<Animator>();
    public Animator HungryAdd => _hungryAdditionAnimator = _hungryAdditionAnimator ??= _hungryAddition.GetComponent<Animator>();
    public Animator ThirstAdd => _thirstAdditionAnimator = _thirstAdditionAnimator ??= _thirstAddition.GetComponent<Animator>();
    public Animator RestAdd => _restAdditionAnimator = _restAdditionAnimator ??= _restAddition.GetComponent<Animator>();
    public Animator goldAdd => _goldAdditionAnimator = _goldAdditionAnimator ??= _goldAddition.GetComponent<Animator>();


    private void Start()
    {
        GameManager.self.OnTakeDamageUI += AddHpVisual;
        GameManager.self.OnAddHungryUI += AddHungryVisual;
        GameManager.self.OnAddThirstUI += AddThirstVisual;
        GameManager.self.OnAddRestUI += AddRestVisual;
        GameManager.self.OnAddGoldUI += AddGoldVisual;
    }


    private void Update()
    {
        time.value = GameManager.self.CurrentTimeProportion;

        hp.fillAmount = GameManager.self.CurrentHpProportion;
        hungry.fillAmount = GameManager.self.CurrentHungryProportion;
        thirst.fillAmount = GameManager.self.CurrentThirstProportion;
        rest.fillAmount = GameManager.self.CurrentRestProportion;
        goldCount.text = GameManager.self.GoldCount.ToString();
    }

    public void AddTimeVisual(float add)
    {
        SetColor(_timeAddition, add);

        _timeAddition.text = add.ToString();

        TimeAdd.SetTrigger(_viewAdditionTriggerName);
    }

    public void AddHpVisual(float add)
    {
        SetColor(_hpAddition, add);

        _hpAddition.text = add.ToString();

        HpAdd.SetTrigger(_viewAdditionTriggerName);
    }

    public void AddHungryVisual(float add)
    {
        SetColor(_hungryAddition, add);

        _hungryAddition.text = add.ToString();

        HungryAdd.SetTrigger(_viewAdditionTriggerName);
    }

    public void AddThirstVisual(float add)
    {
        SetColor(_thirstAddition, add);

        _thirstAddition.text = add.ToString();

        ThirstAdd.SetTrigger(_viewAdditionTriggerName);
    }

    public void AddRestVisual(float add)
    {
        SetColor(_restAddition, add);

        _restAddition.text = add.ToString();

        RestAdd.SetTrigger(_viewAdditionTriggerName);
    }

    public void AddGoldVisual(int add)
    {
        SetColor(_goldAddition, add);

        _goldAddition.text = add.ToString();

        goldAdd.SetTrigger(_viewAdditionTriggerName);
    }

    private void SetColor(TMPro.TextMeshProUGUI toText, float idetify)
    {
        if (idetify == 0)
            toText.color = _neitralColor;

        if (idetify > 0)
            toText.color = _positiveColor;

        if (idetify < 0)
            toText.color = _negativeColor;
    }
}
