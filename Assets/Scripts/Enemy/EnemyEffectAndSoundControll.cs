using UnityEngine;

public class EnemyEffectAndSoundControll : MonoBehaviour
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



    internal void TrunOnEffectAttack(int attackType)
    {
        switch (attackType)
        {
            case 2:
                effectAttack1.SetActive(true);
                break;
            case 3:
                effectAttack2.SetActive(true);
                break;
            case 4:
                effectAttack3.SetActive(true);
                break;
        }
    }

    internal void TrunOnEffectSkill()
    {
        if(effectSkill!=null)
        {
        effectSkill.SetActive(true);
        }
    }
}
