using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public Vector3 mousePos;

    GameObject _qSkill;

    GameObject _eSkil;

    void Start()
    {
        _qSkill = Resources.Load<GameObject>("Prefabs/QSKiLL_OBJECT");

        _eSkil = Resources.Load<GameObject>("Prefabs/W_Skill_Object");

    }


    void Update()
    {
        UseSkill();
    }

    RaycastHit qhit;
    void UseSkill()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        bool raycastHit = Physics.Raycast(ray, out qhit, 100.0f);

        QSkill();
        WSkill();
        ESkill();
    }

    void QSkill()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            mousePos = qhit.point;

            Instantiate(_qSkill, transform.position, transform.rotation);
        }
    }

    void WSkill()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            mousePos = qhit.point;

            Instantiate(_eSkil, transform.position + new Vector3(0,1.0f,0) , transform.rotation);
        }
    }

    Vector3 dir;

    void ESkill()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            mousePos = qhit.point;

            dir = mousePos - transform.position;

            flash();

        }
            
    }

    void flash()
    {
        mousePos = qhit.point;

        Vector3 d = mousePos - transform.position;

        Vector3 f = Vector3.ClampMagnitude(d, 2.0f);

        transform.position += f;
    }
}
