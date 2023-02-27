using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float stoppingSpeed = 0.15f;
    public bool isDestroyAble = false;
    public ParticleSystem deathEffect;

    private void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, stoppingSpeed);
    }

    private void OnDestroy()
    {
        if (isDestroyAble)
        {
            ParticleSystem effect = Instantiate(deathEffect);
            effect.gameObject.SetActive(true);
            effect.Play();
        }
    }
}
