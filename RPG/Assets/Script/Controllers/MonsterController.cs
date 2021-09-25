using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour //공용 controller를 작성하여 플레이어나 몬스터의 공통되는 상태등을 상속받아 작성해도되지만 처음부터 그렇게 하지않았기때문에 그냥 제작하였음
{
    public enum MonsterState
    {
        Idle,
        Moving,
        Attack,
        Die,
        MoveBack,
    }

    MonsterState state;

    Stat monsterStat;

    NavMeshAgent nma;

    PlayerStat playerStat;

    GameObject player;

    GameObject lockTarget;

    Vector3 destPos;

    Vector3 firstPos;

    float attackCoolTime = 2.0f;
    float attackCountDown = 0.0f;

    [SerializeField]
    float scanRange = 10;
    [SerializeField]
    float attackRange = 2;

    void Start()
    {
        monsterStat = GetComponent<Stat>();
        nma = gameObject.GetComponent<NavMeshAgent>();

        state = MonsterState.Idle;

        CreateHpBar();

        firstPos = transform.position;
    }


    void Update()
    {
        AttackCount();

        ResetPos();

        switch (state)
        {
            case MonsterState.Idle:
                UpdateIdle();
                break;
            case MonsterState.Moving:
                UpdateMoving();
                break;
            case MonsterState.MoveBack:
                UpdateMoveBack();
                break;
            case MonsterState.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            state = MonsterState.MoveBack;
            return;
        }

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance <= scanRange) // 플레이어와의 거리가 사정거리보다 작거나 같으면 플레이어를 타겟으로 설정 후 상태를 moving으로 변경 
        {
            lockTarget = player;
            state = MonsterState.Moving;
            return;
        }
    }

    void UpdateMoving()
    {
        if(lockTarget != null)
        {
            destPos = lockTarget.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if(distance <= attackRange) //플레이어가 사정거리 안이면 공격 
            {
                state = MonsterState.Attack;

                nma.SetDestination(transform.position);

                return;
            }
        }

        //공격범위 밖이라면 이동
        Vector3 dir = destPos - transform.position;

        if(dir.magnitude <= 1.0f)
        {
            state = MonsterState.Idle;
        }
        else
        {
            nma.SetDestination(destPos);
            nma.speed = monsterStat.MoveSpeed;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

    }

    void UpdateAttack()
    {
        if(lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

        OnHitEvent();
        
    }

    void OnHitEvent()
    {

        if (lockTarget != null)
        {
            playerStat = lockTarget.GetComponent<PlayerStat>();
            monsterStat = gameObject.GetComponent<Stat>();

            if(attackCountDown >= attackCoolTime)
            {
                int damage = Mathf.Max(0, monsterStat.Attack - playerStat.Defense);

                Debug.Log("몬스터 공격");
                Debug.Log(lockTarget.name + "에게 데미지" + damage);

                playerStat.Hp -= damage;

                attackCountDown = 0.0f;

                if(playerStat.Hp <= 0)
                {
                    Destroy(lockTarget);
                }
            }

            if (playerStat.Hp >= 0)
            {
                state = MonsterState.Idle;
            }
            else
            {
                float distance = (lockTarget.transform.position - transform.position).magnitude;
                if(distance <= attackRange)
                {
                    state = MonsterState.Attack;
                }
                else
                {
                    state = MonsterState.Moving;
                }
            }
        }
        else
        {
            state = MonsterState.Idle;
        }
    }

    void AttackCount()
    {
        if (attackCountDown <= 2.0f)
        {
            attackCountDown += Time.deltaTime;
        }
    }

    void ResetPos()
    {
        float incountRange = 20.0f;

        float distance = (firstPos - transform.position).magnitude;
        if(incountRange <= distance)
        {
            state = MonsterState.MoveBack;
        }
    }

    void UpdateMoveBack()
    {
        Debug.Log("몬스터의 경계범위 밖입니다 몬스터가 되돌아갑니다");

        nma.SetDestination(firstPos);
        nma.speed = monsterStat.MoveSpeed;

        float distance = (firstPos - transform.position).magnitude;
        if (distance <= 0.1f)
        {
            state = MonsterState.Idle;
        }
    }

    void CreateHpBar()
    {
        if(gameObject.GetComponentInChildren<UI_HPbar>() == null) // 자식들중 UI_HPbar 스크립트가진 자식이 없다면 
        {
            Managers.UI.MakeUI(transform); //Hpbar생성
            
        }
    }
}
