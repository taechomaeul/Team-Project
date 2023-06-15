using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEAD
    }

    public ENEMYSTATE enemyState;
    public Animator enemyAnim;

    private EnemyMovingAndDetecting Moving;

    void Start()
    {
        
    }

    void Update()
    {
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                enemyAnim.SetInteger("ENEMYSTATE", (int)ENEMYSTATE.IDLE);
                break;
            case ENEMYSTATE.MOVE:
                break;
            case ENEMYSTATE.ATTACK:
                break;
            case ENEMYSTATE.DAMAGE:
                break;
            case ENEMYSTATE.DEAD:
                break;
            default:
                break;
        }

    }
}
