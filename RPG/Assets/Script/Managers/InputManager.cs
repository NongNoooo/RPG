using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action<Define.MouseEvent> MouseAction = null;

    bool pressed = false;
    float pressedTime = 0;
    float press = 0.2f;

    public void OnUpdate()
    {
        if(MouseAction != null) //마우스를 눌렀다 땔때 클릭 이벤트를 발생시키기 위해 작성
        {
            if (Input.GetMouseButton(1)) //마우스 왼쪽을 눌렀을때
            {
                if (!pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    pressedTime += Time.deltaTime;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                pressed = true; //마우스가 눌러졌을때 pressed를 true로 변경
            }
            else //마우스 왼쪽을 땔때
            {
                if (pressed) // pressed가 true라면
                {
                    if(pressedTime < press)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click); //클릭발동 
                    }
                    MouseAction.Invoke(Define.MouseEvent.PointerUp); // press보다 짧은 시간내에 마우스를 때면 pointerup으로 취급 
                }
                pressed = false;
                pressedTime = 0;
            }
        }
    }

    public void Clear()
    {

        MouseAction = null;
    }
}
