using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Start()
    {
        GameObject _player = Resources.Load<GameObject>("Prefabs/Character/Player");
        GameObject player = Instantiate(_player);
        Camera.main.GetComponent<CameraController>().setPlayer(player);

        GameObject _monster = Resources.Load<GameObject>("Prefabs/Character/Enemy1");
        GameObject monster = Instantiate(_monster);

    }
}
