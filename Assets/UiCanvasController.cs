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


    private void Update()
    {
        time.value = GameManager.self.CurrentTimeProportion;

        hp.fillAmount = GameManager.self.CurrentHpProportion;
        hungry.fillAmount = GameManager.self.CurrentHungryProportion;
        thirst.fillAmount = GameManager.self.CurrentThirstProportion;
        rest.fillAmount = GameManager.self.CurrentRestProportion;
        goldCount.text = GameManager.self.GoldCount.ToString();
    }
}
