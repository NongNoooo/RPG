using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        GameObject _player = Resources.Load<GameObject>("Prefabs/Character/Player");
        GameObject player = Instantiate(_player);
        Camera.main.GetComponent<CameraController>().setPlayer(player);

        //GameObject go = Instantiate(new GameObject(name = "monsterFactory"));
        //go.transform.parent = gameObject.transform;
        //go.AddComponent<MonsterFactory>();
        
    }
}
