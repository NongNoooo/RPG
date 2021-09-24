using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPbar : MonoBehaviour
{
    Stat stat;
    public GameObject hpbar;
    public Slider slider;

    private void Start()
    {
        stat = transform.parent.GetComponent<Stat>();
        hpbar = transform.GetChild(0).gameObject;
        slider = hpbar.GetComponent<Slider>();
    }


    private void Update()
    {
        Transform parent = gameObject.transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
        float ratio = stat.Hp / (float)stat.MaxHp;

        setHPRatio(ratio);
    }

    public void setHPRatio(float ratio)
    {
        slider.value = ratio;
    }
}
