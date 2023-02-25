using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropViewer : MonoBehaviour
{
    public int lesOrder = 6;
    public int moreOrder = 3;
    public float zOffset = 1;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;


    private SpriteRenderer SpriteRenderer
    {
        get => _spriteRenderer = _spriteRenderer ??= GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _transform = transform;
    }

    void FixedUpdate()
    {
        if (_transform.position.z + zOffset < _player.position.z)
            SpriteRenderer.sortingOrder = lesOrder;
        else
            SpriteRenderer.sortingOrder = moreOrder;
    }
}
