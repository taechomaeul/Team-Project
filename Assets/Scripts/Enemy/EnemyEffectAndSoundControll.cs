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

    //[Header("이펙트 선딜레이")]
    //[Tooltip("공격1")]
    //[SerializeField] private float delayAttack1;

    //[Tooltip("공격2")]
    //[SerializeField] private float delayAttack2;

    //[Tooltip("공격3")]
    //[SerializeField] private float delayAttack3;

    //[Tooltip("스킬")]
    //[SerializeField] private float delaySkill;

    //[Header("사운드")]
    //[Tooltip("효과음 재생")]
    //[SerializeField]AudioSource audioSource;

    //[Header("공격 사운드")]
    //[Tooltip("공격1")]
    //[SerializeField] private AudioClip soundAttack1;

    //[Tooltip("공격2")]
    //[SerializeField] private AudioClip soundAttack2;

    //[Tooltip("공격3")]
    //[SerializeField] private AudioClip soundAttack3;

    //[Tooltip("스킬")]
    //[SerializeField] private AudioClip soundSkill;



    internal void TrunOnEffectAttack(int attackType)
    {
        switch (attackType)
        {
            case 2:
                //StartCoroutine(Attack1());
                effectAttack1.SetActive(true);
                break;
            case 3:
                //StartCoroutine(Attack2());
                effectAttack2.SetActive(true);
                break;
            case 4:
                //StartCoroutine(Attack3());
                effectAttack3.SetActive(true);
                break;
        }
    }

    internal void TrunOnEffectSkill()
    {
        //StartCoroutine(Skill());
        effectSkill.SetActive(true);
    }

    //private IEnumerator Attack1()
    //{
    //    yield return new WaitForSeconds(delayAttack1);
    //    effectAttack1.SetActive(true);

    //}

    //private IEnumerator Attack2()
    //{
    //    yield return new WaitForSeconds(delayAttack2);
    //    effectAttack2.SetActive(true);
    //}

    //private IEnumerator Attack3()
    //{
    //    yield return new WaitForSeconds(delayAttack3);
    //    effectAttack3.SetActive(true);
    //}

    //private IEnumerator Skill()
    //{
    //    yield return new WaitForSeconds(delaySkill);
    //    effectSkill.SetActive(true);
    //}
}
