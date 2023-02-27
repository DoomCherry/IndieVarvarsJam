using NaughtyAttributes;
using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Answare
{
    public BonusInfo bonusForAnsware;
    public string _answare;
}

[System.Serializable]
public struct AnswaresGroup
{
    public Answare[] answares;
}

public class Drakon : MonoBehaviour
{
    [SerializeField] private AnswaresGroup[] _answaresGroups;
    [SerializeField] private TextController _textController;

    public Button[] buttons;
    public Slider timer;
    public GameObject eventBody;

    public string IsDieBoolName = "IsDestroy";
    public Animator animator;
    public BonusPoint _negativeViewMarmelad;
    public BonusPoint _positiveViewMarmelad;

    public float deadTime = 2;
    public float _maxWaitTime = 5;
    public float _currentWaitTime = 0;
    private TextMeshProUGUI[] texts;

    private Player _player;


    public int bonusCount = 10;
    public float bonusRadiusSpawn = 2;
    private bool IsDead = false;

    public AudioSource voice;
    public AudioSource scream;
    public AudioClip[] variations;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _textController.OnTextWrite += PlayRandomVoice;
    }

    private void Start()
    {
        texts = buttons.Select(n => n.GetComponentInChildren<TextMeshProUGUI>()).ToArray();

        Answare[] answares = _answaresGroups[_textController.currentPhrase].answares.OrderBy(n => Random.Range(0, 999)).ToArray();

        for (int i = 0; i < buttons.Length; i++)
        {
            texts[i].text = answares[i]._answare;

            if (i == 0)
                buttons[i].onClick.AddListener(OnClickButton0);

            if (i == 1)
                buttons[i].onClick.AddListener(OnClickButton1);

            if (i == 2)
                buttons[i].onClick.AddListener(OnClickButton2);

            if (i == 3)
                buttons[i].onClick.AddListener(OnClickButton3);


            void OnClickButton0()
            {
                SpawnSomething(0);
                KillDragon();
            }

            void OnClickButton1()
            {
                SpawnSomething(1);
                KillDragon();
            }

            void OnClickButton2()
            {
                SpawnSomething(2);
                KillDragon();
            }

            void OnClickButton3()
            {
                SpawnSomething(3);
                KillDragon();
            }

            void SpawnSomething(int index)
            {
                if (answares[index].bonusForAnsware.bonusAddCount < 0)
                {
                    SpawnAround(_negativeViewMarmelad, answares[index].bonusForAnsware);
                }
                else
                {
                    SpawnAround(_positiveViewMarmelad, answares[index].bonusForAnsware);
                }
            }
        }


    }

    public void PlayRandomVoice()
    {
        int value = Random.Range(0, variations.Length);
        voice.clip = variations[value];

        voice.volume = 1 - (Mathf.Clamp(Vector3.Distance(_player.transform.position, transform.position), 0, voice.maxDistance) / voice.maxDistance);

        voice.Play();
    }


    private void SpawnAround(BonusPoint prefab, BonusInfo bonusInfo)
    {
        Transform player = _player.transform;

        for (int i = 0; i < bonusCount; i++)
        {
            BonusPoint point = Instantiate(prefab);
            point.bonusInfo = bonusInfo;

            Transform pointTr = point.transform;

            float x = Mathf.Cos((float)i / bonusCount * 2 * Mathf.PI);
            float y = Mathf.Sin((float)i / bonusCount * 2 * Mathf.PI);
            pointTr.position = player.position + new Vector3(x, 0, y) * bonusRadiusSpawn;
        }

    }

    [Button]
    public void KillDragon()
    {
        IsDead = true;
        animator.SetBool(IsDieBoolName, true);
        scream.Play();

        this.Delay(deadTime, DestroyDragon);
    }

    public void DestroyDragon()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (IsDead)
            return;

        _currentWaitTime += Time.deltaTime;

        timer.value = 1 - (_currentWaitTime / _maxWaitTime);

        if (_maxWaitTime <= _currentWaitTime)
        {
            SpawnAround(_positiveViewMarmelad, _answaresGroups[_textController.currentPhrase].answares[0].bonusForAnsware);
            KillDragon();
        }
    }

    private void OnDestroy()
    {
        Destroy(eventBody.gameObject);
    }
}
