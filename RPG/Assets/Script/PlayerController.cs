using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat stat;

    Vector3 destPos;

    PlayerState state;

    Texture2D attackCursor;
    Texture2D handCursor;

    

    void Start()
    {
        attackCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Basic");

        stat = gameObject.GetComponent<PlayerStat>();
        state = PlayerState.Idle; 
        Managers.Input.MouseAction -= OnMouseEvent; //이벤트가 두번 추가되는것을 막기위해 한번 뺀후에 진행
        Managers.Input.MouseAction += OnMouseEvent;
    }

    public enum PlayerState
    {
        Idle,
        Moving,
        Attack,
        Die,
    }

    void Update()
    {
        UpdateMouseCursor();

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
                UpdateDie();
                break;
        }
        
    }

    int lMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, lMask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                Cursor.SetCursor(handCursor, new Vector2(handCursor.width / 4, 0), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(attackCursor, new Vector2(attackCursor.width / 5, 0), CursorMode.Auto);
            }
        }
    }


    void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position; //방향구하고
        if (dir.magnitude < 0.1f) // 도착지랑 거리가 가까우면 이동안함
        {
            state = PlayerState.Idle;
        }
        else //멀면 이동
        {
            NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
            //nma.CalculatePath
            
            float moveDist = stat.MoveSpeed * Time.deltaTime;  //캐릭터가 마우스를 찍은 곳에 도착하면 떨려서 추가
            if (moveDist >= dir.magnitude)
            {
                moveDist = dir.magnitude;
            }
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position, dir.normalized, Color.red);
            if(Physics.Raycast(transform.position+Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Buildings"))) //건물위를 찍을경우 멈추지않고 이동하려해서 레이를 통해 건물을 인식할경우 멈추도록 만듬
            {
                state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    void UpdateAttack()
    {

    }

    void UpdateDie()
    {

    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, lMask))
        {
            destPos = hit.point;
            state = PlayerState.Moving;

            if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                Debug.Log("Ground Click");
            }
            else
            {
                Debug.Log("Monster Click");
                
            }
        }
    }
}
