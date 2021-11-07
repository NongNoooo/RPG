using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine("followTarget", player);

    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        transform.Translate(transform.forward * 2 * Time.deltaTime);
    }

    IEnumerator followTarget(GameObject p)
    {
        if (p != null)
        {
            while (gameObject.activeSelf)
            {
                Vector3 target = (p.transform.position - transform.position).normalized;

                float dot = Vector3.Dot(transform.forward, target);
                if (dot < 1.0f)
                {
                    float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                    Vector3 cross = Vector3.Cross(transform.forward, target);

                    if (cross.z < 0)
                    {
                        angle = transform.rotation.eulerAngles.z - Mathf.Min(10, angle);
                    }
                    else
                    {
                        angle = transform.rotation.eulerAngles.z + Mathf.Min(10, angle);
                    }

                    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, angle));
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
