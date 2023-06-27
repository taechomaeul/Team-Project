using System.Collections;
using UnityEngine;

public class EnemyEffectControll : MonoBehaviour
{
    [Header("이펙트 오브젝트")]
    [Tooltip("공격1")]
    [SerializeField] private GameObject effectAttack1;

    [Tooltip("공격2")]
    [SerializeField] private GameObject effectAttack2;

    [Tooltip("공격3")]
    [SerializeField] private GameObject effectAttack3;

    [Tooltip("스킬")]
    [SerializeField] private GameObject effectSkill;

    [Header("이펙트 선딜레이")]
    [Tooltip("공격1")]
    [SerializeField] private float delayAttack1;

    [Tooltip("공격2")]
    [SerializeField] private float delayAttack2;

    [Tooltip("공격3")]
    [SerializeField] private float delayAttack3;

    [Tooltip("스킬")]
    [SerializeField] private float delaySkill;



    internal void TrunOnEffectAttack(int attackType)
    {
        switch (attackType)
        {
            case 0:
                StartCoroutine(Attack1());
                break;
            case 1:
                StartCoroutine(Attack2());
                break;
            case 2:
                StartCoroutine(Attack3());
                break;
        }
    }

    internal void TrunOnEffectSkill()
    {
        StartCoroutine(Skill());
    }

    private IEnumerator Attack1()
    {
        yield return new WaitForSeconds(delayAttack1);
        effectAttack1.SetActive(true);
    }

    private IEnumerator Attack2()
    {
        yield return new WaitForSeconds(delayAttack2);
        effectAttack2.SetActive(true);
    }

    private IEnumerator Attack3()
    {
        yield return new WaitForSeconds(delayAttack3);
        effectAttack3.SetActive(true);
    }

    private IEnumerator Skill()
    {
        yield return new WaitForSeconds(delaySkill);
        effectSkill.SetActive(true);
    }
}
