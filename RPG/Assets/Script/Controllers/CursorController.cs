using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int lMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    Texture2D attackCursor;
    Texture2D handCursor;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType cursorType = CursorType.None;


    void Start()
    {
        attackCursor = Resources.Load<Texture2D>("Textures/Cursor/Attack");
        handCursor = Resources.Load<Texture2D>("Textures/Cursor/Basic");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, lMask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handCursor, new Vector2(handCursor.width / 4, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }

            }
            else
            {
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackCursor, new Vector2(attackCursor.width / 5, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }

            }
        }
    }
}
