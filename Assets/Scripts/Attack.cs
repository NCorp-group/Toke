using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("attack 1");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("hit");
        }
    }
}
