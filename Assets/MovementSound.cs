using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour
{

    public string sound;
    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        var magnitude = character.GetComponent<Movement>().magnitude;
        if (magnitude > 0)
        {       
            FindObjectOfType<AudioManager>().Play(sound);
        }

    }
}
