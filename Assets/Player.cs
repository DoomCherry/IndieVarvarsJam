using NaughtyAttributes;
using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bombUI;
    public TextMeshProUGUI textBomb;
    public int bombTimer = 10;
    public float bombEatAdd = 20;

    public string _isWalkBoolName = "IsWalk";
    public string _isBombBoolName = "IsWithBomb";
    public Rigidbody rigidbody;
    public float speed = 2;
    public Vector2 _autoMoveDuration = new Vector2(2, 4);
    public LayerMask enemy;
    public int hp = 3;
    public SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public Color _positiveColor;
    public Color _negativeColor;

    public ParticleSystem _changeHp, _changeHungry, changeThirst, changeRest, changeGold;


    public System.Action OnEatSomething;
    public System.Action OnDrinkSomething;
    public System.Action OnTakeGoldSomething;
    public System.Action OnTakeSomething;

    public bool IsAutoMove = false;
    private Coroutine AutoMoveProcess;

    private SpriteRenderer SpriteRenderer
    {
        get => _spriteRenderer;
    }

    private Animator Animator
    {
        get => _animator = _animator ??= GetComponent<Animator>();
    }




    private void Start()
    {
        DiactivateBombMenu();
    }

    void FixedUpdate()
    {
        if (!GameManager.self.isTick)
            return;

        Move();

        Vector2 mousePos2D = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, Camera.main.farClipPlane));
        Vector3 target = mousePos - transform.position;
        target.y = 0;

        //transform.forward = target.normalized;



        void Move()
        {
            if (IsAutoMove)
                return;

            Vector3 moveDirection = Vector3.zero;
            Animator.SetBool(_isWalkBoolName, false);

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection.z = 1;
                Animator.SetBool(_isWalkBoolName, true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveDirection.z = -1;
                Animator.SetBool(_isWalkBoolName, true);
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDirection.x = -1;
                SpriteRenderer.flipX = true;
                Animator.SetBool(_isWalkBoolName, true);
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveDirection.x = 1;
                SpriteRenderer.flipX = false;
                Animator.SetBool(_isWalkBoolName, true);
            }

            rigidbody.velocity = moveDirection.normalized * speed;
        }
    }

    public void TakeSomething()
    {
        OnTakeSomething?.Invoke();
    }

    private float bombTick = 0;
    private Coroutine bombProcess;

    [Button]
    public void StartBomb()
    {
        bombUI.SetActive(true);
        textBomb.text = bombTimer.ToString();
        bombTick = bombTimer;
        Animator.SetBool(_isBombBoolName, true);

        bombProcess = this.RepeatUntil(IsExploid, MinusTimer, Explosion, 1);



        bool IsExploid()
        {
            return bombTick == 0;
        }

        void Explosion()
        {
            DiactivateBombMenu();

            ChangeHungry(true);

            GameManager.self.ChangeHungryLevel(bombEatAdd);
            GameManager.self.TakeAllEffectsUI();

            StopCoroutine(bombProcess);
        }

        void MinusTimer()
        {
            bombTick--;
            textBomb.text = bombTick.ToString();
        }
    }

    public void DeactivateBomb()
    {
        DiactivateBombMenu();

        if (bombProcess != null)
            StopCoroutine(bombProcess);
    }

    private void DiactivateBombMenu()
    {
        Animator.SetBool(_isBombBoolName, false);
        bombUI.SetActive(false);
        textBomb.text = "";
    }



    [Button]
    public void ChangeHp(bool IsPositive = true)
    {
        var main = _changeHp.main;
        main.startColor = IsPositive ? _positiveColor : _negativeColor;
        _changeHp.Play();
    }

    [Button]
    public void ChangeHungry(bool IsPositive = true)
    {
        var main = _changeHungry.main;
        main.startColor = IsPositive ? _positiveColor : _negativeColor;
        _changeHungry.Play();

        OnEatSomething?.Invoke();
    }

    [Button]
    public void ChangeThirst(bool IsPositive = true)
    {
        var main = changeThirst.main;
        main.startColor = IsPositive ? _positiveColor : _negativeColor;
        changeThirst.Play();

        OnDrinkSomething?.Invoke();
    }

    [Button]
    public void ChangeRest(bool IsPositive = true)
    {
        var main = changeRest.main;
        main.startColor = IsPositive ? _positiveColor : _negativeColor;
        changeRest.Play();
    }

    [Button]
    public void ChangeGold(bool IsPositive = true)
    {
        var main = changeGold.main;
        main.startColor = IsPositive ? _positiveColor : _negativeColor;
        changeGold.Play();

        OnTakeGoldSomething?.Invoke();
    }

    [Button]
    public void AutoMove()
    {
        AutoMove(-Vector3.right);
    }

    public void AutoMove(Vector3 direction)
    {
        IsAutoMove = true;

        if (AutoMoveProcess != null)
            StopCoroutine(AutoMoveProcess);

        AutoMoveProcess = this.RepeatForever(AutoMoving, Time.fixedDeltaTime);
        this.Delay(direction.x == 0 ? _autoMoveDuration.y : _autoMoveDuration.x, AutoMovingEnd);




        void AutoMoving()
        {
            if (!GameManager.self.isTick)
                return;

            SpriteRenderer.flipX = direction.x == -1;
            Animator.SetBool(_isWalkBoolName, true);
            rigidbody.velocity = direction.normalized * speed;
        }
    }


    private void AutoMovingEnd()
    {
        IsAutoMove = false;
        StopCoroutine(AutoMoveProcess);
    }
}
