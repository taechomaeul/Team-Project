using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFuntion : MonoBehaviour
{
    [Header("고정 변수")]
    public float coolTime = 2f;
    public readonly int increaseHp = 30;
    public readonly float increaseAmount = 1.3f; // 30% 증가

    [Header("연결 X")]
    public EnemyPrefab enemyPrefabInfo;
    public PlayerInfo plInfo;
    public PlayerController plController;
    public PlayerLook playerLook;

    private void Start()
    {
        enemyPrefabInfo = GetComponent<EnemyPrefab>();
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        plController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerLook = GameObject.Find("Sight").GetComponent<PlayerLook>();
    }

    /// <summary>
    /// 영혼석에서 영혼의 무게를 옮겨 영혼석(HP)을 채우는 함수
    /// 영혼석의 무게(HP)은 최대 수치(soulHp = 666)를 넘길 수 없다.
    /// </summary>
    /// <param name="hp">영혼석에 담긴 영혼의 무게(HP)</param>
    public void MoveSoulToStone(int hp)
    {
        plInfo.soulHp += hp;
    }

    /// <summary>
    /// 영혼석을 사용하여 플레이어의 혼력(HP)을 채우는 함수
    /// 플레이어의 혼력은 최대치(maxHp = 100)를 넘길 수 없다.
    /// 
    /// ex. 최대 체력 100, 현재 체력 99, 회복하려는 숫자  2
    /// 2가 1보다 커서 최대체력보다 높은 숫자로 회복하게 된다.
    /// 따라서 100 - 99 한 숫자(= 1)를 현재 체력(99)에 더하고(99+1 = 100),
    /// 영혼석의 숫자(N)에는 빼서 최대 체력을 넘지 않게 & 올바르게 뺀 값(N-1)을 영혼석에 저장한다.
    /// </summary>
    /// <param name="hp">초당 회복하는 HP</param>
    public void FillHpUsingStone(int hp)
    {
        if (plInfo.maxHp - plInfo.curHp < hp) //만약 최대체력-현재체력 보다 회복하려는 숫자가 더 크다면
        {
            int subHp = plInfo.maxHp - plInfo.curHp;
            plInfo.curHp += subHp;
            plInfo.soulHp -= subHp;
            return;
            //회복하려는 숫자가 아니라 뺀 숫자만큼의 크기를 더하고 뺀다.
        }
        plInfo.curHp += hp;
        plInfo.soulHp -= hp;
    }

    /// <summary>
    /// 공격력 증가함수.
    /// </summary>
    public void IncreasePower()
    {
        plInfo.plAtk = (int)(plInfo.plAtk * increaseAmount);
        Debug.Log("공격력" + plInfo.plAtk);
    }

    /// <summary>
    /// 이동속도 증가함수.
    /// </summary>
    public void IncreaseSpeed()
    {
        plInfo.plMoveSpd *= increaseAmount;
        Debug.Log("이동속도" + plInfo.plMoveSpd);
    }

    /// <summary>
    /// 혼력(HP) 회복함수.
    /// </summary>
    public void IncreaseHp()
    {
        if (plInfo.maxHp - plInfo.curHp < increaseHp) //만약 최대체력-현재체력 보다 회복하려는 숫자가 더 크다면
        {
            int subHp = plInfo.maxHp - plInfo.curHp;
            plInfo.curHp += subHp;
            return;
            //회복하려는 숫자가 아니라 뺀 숫자만큼의 크기를 더하고 뺀다.
        }
        plInfo.curHp += increaseHp;
    }

    /// <summary>
    /// 빙의하기 기능
    /// </summary>
    /// <param name="player">원래 플레이어</param>
    /// <param name="enemy">현재 적 이름(죽은 시체)</param>
    public void ChangePrefab(GameObject player, GameObject enemy)
    {
        
        string enemyName = enemy.name.Split("_")[0]; //이름 '_'으로 분리한 후, 가장 앞에 저장된 이름을 저장한다
        Destroy(enemy.gameObject); //적 시체 삭제

        //바꿀 프리팹 이름 먼저 찾기
        for (int i=0; i< enemyPrefabInfo.enemyPrefabs.Length; i++)
        {
            if (enemyPrefabInfo.enemyPrefabs[i].name.Contains(enemyName))
            {
                Vector3 originPlayerPos = player.transform.position; //player의 위치(PlayerModel이 움직임)
                Quaternion originPlayerRot = player.transform.parent.rotation; //캐릭터 방향
                Quaternion originSightRot = GameObject.Find("Sight").GetComponent<PlayerLook>().transform.rotation; //시야방향

                Destroy(player.gameObject);

                //프리팹 생성 및 정보입력
                GameObject enemyPrefab = enemyPrefabInfo.enemyPrefabs[i]; //이름이 같은 프리팹 선택
                GameObject newPlayer = Instantiate(enemyPrefab, originPlayerPos, originPlayerRot); //생성
                GameObject sight = Instantiate(enemyPrefabInfo.sightPrefab);
                GameObject range = Instantiate(enemyPrefabInfo.atkRangePrefab);

                newPlayer.name = "PlayerModel";
                newPlayer.tag = "Player";
                newPlayer.transform.parent = GameObject.FindGameObjectWithTag("Player").transform; //PlayerModel은 Player 자식개체로 들어간다
                newPlayer.transform.position = originPlayerPos;
                newPlayer.transform.rotation = originPlayerRot;

                sight.name = "Sight";
                sight.transform.parent = newPlayer.transform; //sight의 부모개체는 newPlayer.
                sight.GetComponent<PlayerLook>().plInfo = player.GetComponent<PlayerInfo>();
                sight.transform.localPosition = Vector3.zero;
                sight.transform.localRotation = originSightRot;

                range.transform.parent = newPlayer.transform; //Atk Range 달아줌
                range.transform.position = new Vector3(0, 0, 2);
                range.transform.rotation = Quaternion.identity;

                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().attackRange = range;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().cameraTransform = sight.transform.GetChild(0);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().characterController = newPlayer.GetComponent<CharacterController>();
                
            }
        }

    }

    public void PauseGameForAct()
    {
        plInfo.plMoveSpd = 0;
        playerLook.sensitivity = 0;
        plController.plState = PlayerController.PL_STATE.ACT;
    }

    public void RestartGame()
    {
        plInfo.plMoveSpd = plController.moveSpd;
        playerLook.sensitivity = playerLook.readOnlySens;
        plController.isActivated = false;
        plController.plState = PlayerController.PL_STATE.IDLE;
    }
}
