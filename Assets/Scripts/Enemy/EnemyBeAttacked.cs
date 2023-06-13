using System.Collections;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // 적 정보
    EnemyInfo enemyInfo;

    // 영혼석
    public GameObject soulStone;

    void Start()
    {
        // enemyInfo 초기화
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // 영혼석 off
        soulStone.SetActive(false);
    }

    // damage만큼 공격 받음
    public void BeAttacked(float damage)
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

        // 죽었다면
        if (enemyInfo.GetIsDead())
        {
            // 사망 애니메이션 재생
            StartCoroutine(HitAnimation());
        }
        // 살아있다면
        else
        {
            // 피격 애니메이션 재생(테스트용 임의시간값)
            StartCoroutine(HitAnimation(1));
        }
    }

    // 피격 애니메이션(애니메이션 시간)
    IEnumerator HitAnimation(float time)
    {
        // 살아있다면
        if (!enemyInfo.GetIsDead())
        {
            // 공격 중이 아니라면
            if (!enemyInfo.GetIsAttacking())
            {
                // 피격 애니메이션 재생
                Debug.Log("ㅁㄴㅇㄹ");

                // 애니메이션 끝날 때까지 기다림
                yield return new WaitForSeconds(time);
                Debug.Log("ㅁㄴㅇㄹ2");
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
    }

    IEnumerator HitAnimation()
    {
        // 죽었다면
        if (enemyInfo.GetIsDead())
        {
            // 사망 애니메이션 재생
            Debug.Log("사망 애니메이션");

            // 애니메이션 끝날 때까지 기다림(임의값)
            yield return new WaitForSeconds(2);

            // 영혼석 on
            soulStone.SetActive(true);
        }
    }
}
