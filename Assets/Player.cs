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




    private SpriteRenderer SpriteRenderer
    {
        get => _spriteRenderer;
    }

    private Animator Animator
    {
        get => _animator = _animator ??= GetComponent<Animator>();
    }




    void Start()
    {
        this.RepeatForever(TryShot, 0.5f);
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

    void TryShot()
    {
        var hitInfo = this.Boxcast().FromGameObjectInWorld(transform).ToDirection(transform.forward)
            .SingleHit().WithSize(Vector3.one).WithDefaultRotation().WithDistance(10).UseCustomLayerMask(enemy).DontIgnoreAnything();

        if (Input.GetMouseButton(0) && hitInfo.wasHit)
        {
            foreach (var item in hitInfo.hits)
            {
                if (item.collider.TryGetComponent(out Enemy enemy))
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
