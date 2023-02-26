using SimpleMan.AsyncOperations;
using SimpleMan.VisualRaycast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BonusType
{
    Health,
    Food,
    Drink,
    Gold,
    RestDoping
}

[Serializable]
public struct BonusInfo
{
    public BonusType bonusType;
    public float bonusAddCount;
    public bool isVisualize;
    public bool playTakeSound;
}

public class BonusPoint : MonoBehaviour
{
    public BonusInfo bonusInfo;
    public BonusInfo[] additionalBonuses;
    public LayerMask player;
    public float radius;

    void Start()
    {
        this.RepeatForever(TryFindPlayer, 0.1f);
    }

    void TryFindPlayer()
    {
        var hitInfo = this.SphereOverlap().FromGameObjectInWorld(gameObject).WithRadius(radius).UseCustomLayerMask(player).DontIgnoreAnything();
        if (hitInfo.wasHit)
        {
            Player player = hitInfo.detectedColliders.First().gameObject.GetComponent<Player>();

            if (player != null)
            {
                ApplyBonus(player, bonusInfo);

                foreach (var item in additionalBonuses)
                {
                    ApplyBonus(player, item);
                }

                GameManager.self.TakeAllEffectsUI();

                Destroy(gameObject);
            }
        }
    }

    private void ApplyBonus(Player player, BonusInfo bonusInfo)
    {
        switch (bonusInfo.bonusType)
        {
            case BonusType.Health:
                GameManager.self.TakeDamage(-bonusInfo.bonusAddCount);

                if (bonusInfo.isVisualize) player.ChangeHp(bonusInfo.bonusAddCount > 0);
                if (bonusInfo.playTakeSound) player.TakeSomething();
                break;

            case BonusType.Food:
                GameManager.self.ChangeHungryLevel(bonusInfo.bonusAddCount);
                if (bonusInfo.isVisualize) player.ChangeHungry(bonusInfo.bonusAddCount > 0);
                if (bonusInfo.playTakeSound) player.TakeSomething();
                break;

            case BonusType.Drink:
                GameManager.self.ChangeThirthLevel(bonusInfo.bonusAddCount);
                if (bonusInfo.isVisualize) player.ChangeThirst(bonusInfo.bonusAddCount > 0);
                if (bonusInfo.playTakeSound) player.TakeSomething();
                break;

            case BonusType.Gold:
                GameManager.self.AddGold((int)bonusInfo.bonusAddCount);
                if (bonusInfo.isVisualize) player.ChangeGold(bonusInfo.bonusAddCount > 0);
                if (bonusInfo.playTakeSound) player.TakeSomething();
                break;

            case BonusType.RestDoping:
                GameManager.self.ChangeRestLevel((int)bonusInfo.bonusAddCount);
                if (bonusInfo.isVisualize) player.ChangeRest(bonusInfo.bonusAddCount > 0);
                if (bonusInfo.playTakeSound) player.TakeSomething();
                break;
        }
    }
}
