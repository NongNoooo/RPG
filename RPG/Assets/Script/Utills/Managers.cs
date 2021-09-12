using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; //유일성 보장
    public static Managers instance { get { Init(); return _instance; } } // 매니저 가져오기

    InputManager input = new InputManager();
    public static InputManager Input { get { return instance.input; } }

    void Start()
    {
        Init();
    }


    void Update()
    {
        input.OnUpdate();
    }

    static void Init() // 매니저가 없을경우 생성
    {
        if(_instance == null) 
        {
            GameObject go = GameObject.Find("Managers");
            if(go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
        }
    }
}
