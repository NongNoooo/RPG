using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode camMode = Define.CameraMode.QuarterView;
    [SerializeField]
    Vector3 camPosition = new Vector3(0.0f, 10.0f, -10.0f);
    [SerializeField]
    GameObject player = null;

    void Start()
    {
        
    }


    void LateUpdate() // 캐릭터 이동보다 나중에 스크립트가 실행되게 만들어서 화면이 떨리는것을 방지하기 위해 lateupdate를 사용
    {
        if(camMode == Define.CameraMode.QuarterView)
        {
            transform.position = player.transform.position + camPosition;
            transform.LookAt(player.transform);
        }
    }

    public void SetQuarterView(Vector3 camPos) //자꾸 오류 뿜어서 camMode 사용해줌
    {
        camMode = Define.CameraMode.QuarterView;
        camPosition = camPos;
    }
}
