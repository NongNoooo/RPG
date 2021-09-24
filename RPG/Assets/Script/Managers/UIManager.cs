using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    Canvas canvas;

    public void MakeUI(Transform _parents)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/UI/HP_UI");
        GameObject UI = GameObject.Instantiate(go);
        UI.transform.SetParent(_parents.transform);

        canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

    }
}
