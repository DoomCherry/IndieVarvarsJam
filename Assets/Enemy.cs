using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public ParticleSystem explosionEffectPrefab;
    public LayerMask player;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        this.RepeatForever(TryFindPlayer, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    private void OnDestroy()
    {
        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    void TryFindPlayer()
    {
        var hitInfo = this.SphereOverlap().FromGameObjectInWorld(gameObject).WithRadius(2).UseCustomLayerMask(player).DontIgnoreAnything();
        if (hitInfo.wasHit)
        {
            Player player = hitInfo.detectedColliders.First().gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.hp -= 1;
                Destroy(gameObject);
            }
        }
    }
}
