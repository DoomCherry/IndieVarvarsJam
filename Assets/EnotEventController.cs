using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnotEventController : MonoBehaviour
{
    public int _goldToPay = 10;
    public float maxSpeed = 100;
    public float minSpeed = 50;
    public float speedChangeMult = 0.25f;
    public string _isWalkBoolName = "IsWalk";
    public Button _payButton;
    public TextController _beggarText;
    public TextController _gratitudeText;

    public AudioSource source;
    public AudioClip[] voiceVariation;

    public ParticleSystem puf;
    private Player _player;

    public SpriteRenderer _sprite;
    public Animator _animator;
    public UnityEvent OnButtonClick;
    public Rigidbody rigidbody;
    public float _currentSpeed;


    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _beggarText.gameObject.SetActive(true);
        _gratitudeText.gameObject.SetActive(false);

        _payButton.onClick.AddListener(OnPayButtonClick);

        _beggarText.OnTextWrite += PlayRandomVoice;
        _gratitudeText.OnTextWrite += PlayRandomVoice;
        _currentSpeed = maxSpeed;
    }

    public void PlayRandomVoice()
    {
        int value = Random.Range(0, voiceVariation.Length);
        source.clip = voiceVariation[value];

        source.volume = 1 - (Mathf.Clamp(Vector3.Distance(_player.transform.position, transform.position), 0, source.maxDistance) / source.maxDistance);

        source.Play();
    }

    private void OnPayButtonClick()
    {        
        OnButtonClick?.Invoke();
    }

    public void MoneyTrade()
    {
        if (GameManager.self.GoldCount < _goldToPay)
            return;

        _player.ChangeGold(false);
        GameManager.self.AddGold(-_goldToPay);
        GameManager.self.TakeAllEffectsUI();

        HideAllUI();
    }

    public void RunAway()
    {
        HideAllUI();
        MoveToRandomDirection();
    }

    public void GetPlayerEatBomb()
    {
        _player.StartBomb();
        puf.Play();
    }

    public void MoveToRandomDirection()
    {
        Vector2 playerDir = new Vector2(transform.position.x, transform.position.z) - new Vector2(_player.transform.position.x, _player.transform.position.z);

        Vector3 direction = playerDir.x > playerDir.y ? new Vector3(playerDir.x, 0, 0) : new Vector3(0, 0, playerDir.y);
        direction = direction.normalized;

        this.RepeatForever(AutoMoving, Time.fixedDeltaTime);




        void AutoMoving()
        {
            _sprite.flipX = direction.x == -1;
            _animator.SetBool(_isWalkBoolName, true);

            _currentSpeed = Mathf.Lerp(_currentSpeed, minSpeed, speedChangeMult);

            rigidbody.velocity = direction.normalized * _currentSpeed;
        }
    }

    private void HideAllUI()
    {
        _beggarText.gameObject.SetActive(false);
        _gratitudeText.gameObject.SetActive(true);
        _payButton.gameObject.SetActive(false);
    }
}
