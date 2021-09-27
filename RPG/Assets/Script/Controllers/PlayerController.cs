using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat stat;

    Vector3 destPos;

    PlayerState state;

    public GameObject lockTarget;

    int lMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    float attackCoolTime = 2.0f;
    float attackCountDown = 0.0f;

    public enum PlayerState
    {
        Idle,
        Moving,
        Attack,
        Die,
    }

    void Start()
    {
        mf = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<MonsterFactory>();

        stat = gameObject.GetComponent<PlayerStat>();
        state = PlayerState.Idle;

        Managers.Input.MouseAction -= OnMouseEvent; //이벤트가 두번 추가되는것을 막기위해 한번 뺀후에 진행
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeUI(transform);
    }

    void Update()
    {
        AttackCount();
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Attack:
                UpdateAttack();
                break;
            case PlayerState.Die:
                break;
        }
        
    }

    void UpdateMoving()  
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if (lockTarget != null)
        {
            float distance = (destPos - transform.position).magnitude;
            if(distance <= 20/*사정거리*/)
            {
                state = PlayerState.Attack;
                return;
            }
        }

        //이동
        Vector3 dir = destPos - transform.position; //방향구하고

        if (dir.magnitude < 0.1f) // 도착지랑 거리가 가까우면 이동안함
        {
            state = PlayerState.Idle;
        }
        else //멀면 이동
        {
            //nma.CalculatePath
            
            float moveDist = stat.MoveSpeed * Time.deltaTime;  //캐릭터가 마우스를 찍은 곳에 도착하면 떨려서 추가
            if (moveDist >= dir.magnitude)
            {
                moveDist = dir.magnitude;
            }

            Debug.DrawRay(transform.position, dir.normalized, Color.red);
            if(Physics.Raycast(transform.position+Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Buildings"))) //건물위를 찍을경우 멈추지않고 이동하려해서 레이를 통해 건물을 인식할경우 멈추도록 만듬
            {
                state = PlayerState.Idle;
                return;
            }

            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    bool canAttack = false;
    void UpdateAttack()
    {
        if(canAttack)
        {
            Debug.Log("플레이어 공격");
            if(lockTarget != null)
            {
                Stat targetStat = lockTarget.GetComponent<Stat>();
                Stat myStat = gameObject.GetComponent<PlayerStat>();
                int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
                Debug.Log(lockTarget.name + "에게 데미지" + damage);
                targetStat.Hp -= damage;

                
                if (targetStat.Hp <= 0)
                {
                    Destroy(lockTarget);
                    RemoveMonsterCount();
                    lockTarget = null;
                }
                
            }
            canAttack = false;
            attackCountDown = 0.0f;

            if (stopAttack)
            {
                state = PlayerState.Idle;
            }
            else
            {
                state = PlayerState.Attack;
            }
        }
    }

    void AttackCount()
    {
        if(attackCountDown <= attackCoolTime)
        {
            attackCountDown += Time.deltaTime;

            if(attackCountDown >= attackCoolTime)
            {
                canAttack = true;
            }
        }
        
    }


    bool stopAttack = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (state) 
        {
            case PlayerState.Idle:
                OnMouseEvent_NotRunInAttack(evt); //playerstate.Skill 상태일때 이동관련 스크립트가 실행되지 않도록 만듬
                break;
            case PlayerState.Moving:
                OnMouseEvent_NotRunInAttack(evt);
                break;
            case PlayerState.Attack:
                {
                    if(evt == Define.MouseEvent.PointerUp) //마우스를 때면 공격이 멈추도록 만듬 
                    {
                        stopAttack = true;
                    }
                }
                break;
        }
    }

    void OnMouseEvent_NotRunInAttack(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, lMask);
        //ray를 이용해 클릭한 포인트를 받아옴 

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        destPos = hit.point;
                        state = PlayerState.Moving; //레이가 몬스터에 닿았건 땅에 닿았건 이동
                        stopAttack = false; //다시 공격하기 위해 stopAttack 초기화 

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) //레이가 몬스터에 닿았는지 땅에 닿았는지 확인 
                        {
                            lockTarget = hit.collider.gameObject; //몬스터에 닿았을경우 닿은 몬스터를 타겟으로 이동한다
                        }
                        else
                        {
                            lockTarget = null; //레이가 몬스터에 닿은게 아니면 타겟을 null로 
                        }
                    }
                }
                break;

            case Define.MouseEvent.Press:
                {
                    if (lockTarget != null)
                    {
                        destPos = lockTarget.transform.position; //타겟이 null이 아닐경우 도착지점을 타겟으로하고 마우스를 다른곳으로 이동해도 타겟을 향해 이동한다
                    }
                    else
                    {
                        if (raycastHit)
                        {
                            destPos = hit.point; //레이가 땅에 닿았을경우 ray에 hit.point로 이동 
                        }
                    }
                }
                break;

            case Define.MouseEvent.PointerUp:
                {
                    stopAttack = true; //마우스를 때면 공격을 멈추게 만듬
                }
                break;
        }
    }

    public MonsterFactory mf;
    void RemoveMonsterCount()
    {
        mf.monsterCount--;
    }
}
