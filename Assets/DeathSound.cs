using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSound : MonoBehaviour
{

    public string sound;


    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnEnemyDie += PlayDeathSound;
    }

    void PlayDeathSound()
    {
        FindObjectOfType<AudioManager>().Play(sound);
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }
}
