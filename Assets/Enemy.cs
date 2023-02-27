using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public ParticleSystem explosionEffectPrefab;
    public LayerMask player;
    private Player target;
    public float damage = 10;
    public float ActivationDelay = 1;
    public float livetime = 12;

    // Start is called before the first frame update
    void Start()
    {
        this.Delay(ActivationDelay, ActivateEnemy);
        this.Delay(livetime, () => Destroy(gameObject));
    }

    public void ActivateEnemy()
    {
        target = FindObjectOfType<Player>();
        this.RepeatForever(TryFindPlayer, 0.5f);
    }

    private void OnDestroy()
    {
        if (explosionEffectPrefab != null)
        {
            var e = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            e.GetComponentInChildren<AudioSource>().Play();
            e.Play();
        }
    }

    void TryFindPlayer()
    {
        var hitInfo = this.SphereOverlap().FromGameObjectInWorld(gameObject).WithRadius(2).UseCustomLayerMask(player).DontIgnoreAnything();
        if (hitInfo.wasHit)
        {
            Player player = hitInfo.detectedColliders.First().gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.DeactivateBomb();
                target.ChangeHp(false);
                GameManager.self.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
