using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectAndSoundControll : MonoBehaviour
{
    [Header("재생할 이펙트 오브젝트")]
    [Tooltip("스킬")]
    [SerializeField] private GameObject effectSkill;

    [Tooltip("트레일 랜더러")]
    [SerializeField] private GameObject trailRenderer;


    internal void TurnOnEffectAttack()
    {
        trailRenderer.SetActive(true);
    }

    internal void TurnOffEffectAttack()
    {
        trailRenderer.SetActive(false);
    }

    internal void TurnOnEffectSkill()
    {
        if (effectSkill != null)
        {
            effectSkill.SetActive(true);
        }
    }
}
