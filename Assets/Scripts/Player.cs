using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent<int> OnAttack;
    public GameObject spawn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dmg = 30;
            OnAttack?.Invoke(dmg);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (spawn != null)
            {
                var origin = Vector3.zero;
                var obj = Instantiate(spawn, origin, Quaternion.identity);
                
            }
        }
    }
}
