using NaughtyAttributes;
using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string _isWalkBoolName = "IsWalk";
    public Rigidbody rigidbody;
    public float speed = 2;
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


    private SpriteRenderer SpriteRenderer
    {
        get => _spriteRenderer;
    }

    private Animator Animator
    {
        get => _animator = _animator ??= GetComponent<Animator>();
    }




    void FixedUpdate()
    {
        if (hp > 8)
            hp = 8;

        Move();

        Vector2 mousePos2D = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, Camera.main.farClipPlane));
        Vector3 target = mousePos - transform.position;
        target.y = 0;

        //transform.forward = target.normalized;



        void Move()
        {
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
}
