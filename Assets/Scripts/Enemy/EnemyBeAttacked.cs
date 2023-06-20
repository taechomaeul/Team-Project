using System.Collections;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // 적 정보
    Enemy enemyInfo;

    // 영혼석
    public GameObject soulStone;

    // 애니메이터 컨트롤
    EnemyAnimationControll eac;
    void Start()
    {
        // enemyInfo 초기화
        if (enemyInfo == null)
        {
            // 일반 몬스터라면
            if (GetComponent<BossInfo>() == null)
            {
                enemyInfo = GetComponent<EnemyInfo>().stat;
            }
            // 보스라면
            else
            {
                enemyInfo = GetComponent<BossInfo>().stat;
            }
        }
        // 영혼석 off
        soulStone.SetActive(false);

        // 애니메이터 컨트롤 세팅
        eac = GetComponent<EnemyAnimationControll>();
    }

    // damage만큼 공격 받음
    public void BeAttacked(int damage)
    {
        // 현재 체력에서 damage만큼 차감
        enemyInfo.SetCurrentHp(enemyInfo.GetCurrentHp() - damage);

        // 적 현재 체력이 0 이하라면
        if (enemyInfo.GetCurrentHp() <= 0)
        {
            // 적 사망
            enemyInfo.SetIsDead(true);
        }

        // 이전 코루틴 중지
        StopAllCoroutines();

            // 피격, 사망 애니메이션 재생
            StartCoroutine(HitAnimation());
    }

    // 피격, 사망 애니메이션
    IEnumerator HitAnimation()
    {
        // 살아있다면
        if (!enemyInfo.GetIsDead())
        {
            // 공격 중이 아니라면
            if (!enemyInfo.GetIsAttacking())
            {
                // 피격 중 -> true
                enemyInfo.SetIsAttacked(true);

                // 피격 애니메이션 재생
                eac.SetAnimationState(EnemyAnimationControll.Animation_State.Hit);

                // 애니메이션 끝날 때까지 기다림
                yield return new WaitForSeconds(eac.GetAnimationDurationTime(EnemyAnimationControll.Animation_State.Hit));

                // 피격 애니메이션 끝
                eac.SetAnimationState(EnemyAnimationControll.Animation_State.Idle);

                // 피격 중 -> false
                enemyInfo.SetIsAttacked(false);
            }

            // 탐지 대상을 인식하지 못하고 있다면
            if (!enemyInfo.GetIsTracking())
            {
                // 추격 중 -> true
                enemyInfo.SetIsTracking(true);
                // 탐지 대상에게 회전함
                transform.LookAt(enemyInfo.target.transform.position);
            }
        }
        else
        {
            // 사망 애니메이션 재생
            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Dead);

            // 애니메이션 끝날 때까지 기다림(임의값)
            yield return new WaitForSeconds(eac.GetAnimationDurationTime(EnemyAnimationControll.Animation_State.Dead));

            // 영혼석 on
            soulStone.SetActive(true);
        }
    }
}
