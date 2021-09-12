using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;

    bool moveToDest = false;
    Vector3 destPos;

    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClick; //이벤트가 두번 추가되는것을 막기위해 한번 뺀후에 진행
        Managers.Input.MouseAction += OnMouseClick;
    }


    void Update()
    {
        if (moveToDest) //참일경우
        {
            Vector3 dir = destPos - transform.position; //방향구하고
            if (dir.magnitude < 0.01f) // 도착지랑 거리가 가까우면 이동안함
            {
                moveToDest = false;
            }
            else //멀면 이동
            {
                transform.position = transform.position + dir.normalized * speed * Time.deltaTime;
                transform.LookAt(destPos);
            }
        }
    }

    void OnMouseClick(Define.MouseEvent evt)
    {
        //if (evt != Define.MouseEvent.Click)
        //    return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            destPos = hit.point;
            moveToDest = true;
        }
    }
}
