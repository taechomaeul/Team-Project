using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectAndSoundControll : MonoBehaviour
{
    [Header("재생할 이펙트 오브젝트")]
    [Tooltip("공격1")]
    [SerializeField] private GameObject effectAttack1;

    [Tooltip("공격2")]
    [SerializeField] private GameObject effectAttack2;

    [Tooltip("공격3")]
    [SerializeField] private GameObject effectAttack3;

    [Tooltip("스킬")]
    [SerializeField] private GameObject effectSkill;



    internal void TurnOnEffectAttack(int attackType)
    {
        switch (attackType)
        {
            case 4:
                effectAttack1.SetActive(true);
                break;
            case 5:
                effectAttack2.SetActive(true);
                break;
            case 6:
                effectAttack3.SetActive(true);
                break;
        }
    }

    internal void TurnOnEffectSkill()
    {
        if (effectSkill != null)
        {
            effectSkill.SetActive(true);
        }
    }
}
