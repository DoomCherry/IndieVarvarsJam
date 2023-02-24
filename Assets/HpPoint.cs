using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HpPoint : MonoBehaviour
{
    public LayerMask player;

    void Start()
    {
        this.RepeatForever(TryFindPlayer, 0.5f);
    }

    void TryFindPlayer()
    {
        var hitInfo = this.SphereOverlap().FromGameObjectInWorld(gameObject).WithRadius(2).UseCustomLayerMask(player).DontIgnoreAnything();
        if (hitInfo.wasHit)
        {
            Player player = hitInfo.detectedColliders.First().gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.hp += 1;
                Destroy(gameObject);
            }
        }
    }
}
