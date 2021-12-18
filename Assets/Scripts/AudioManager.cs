using UnityEngine.Audio;
using System;
using UnityEngine;

// This AudioManager is partly inspired by this youtube video
// https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private float sfx = 1;
    private float music = 1;
    private float master = 1;
    private int playerFireCounter = 0;
    private int enemyFireCounter = 0;

    //TEST
    private Sound currentMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Start subscribes to all events
    void Start()
    {
        GoToMainMenu();
    }

    private void OnEnable()
    {
        Movement.OnPlayerMovement += PlayerMovementSound;
        RangedWeapon.OnFire += PlayerFireSound;
        Enemy.OnEnemyDie += EnemyDeathSound;
        Enemy.OnEnemySpawn += EnemySpawnSound;
        Enemy.OnEnemyTakeDamage += EnemyTakeDamageSound;
        RangedAttack.OnEnemyRangedAttack += EnemyFireSound;
        PlayerHealthController.OnPlayerTakeDamage += PlayerTakeDamageSound;
        PlayerHealthController.OnPlayerDie += PlayerDeathSound;

        //Rooms/Waves
        RoomManager.OnRoomComplete += RoomCompleteSound;
        RoomManager.OnWaveComplete += WaveCompleteSound;

        CollectItem.OnDoorInteraction += PlayMusic;
    }

    private void OnDisable()
    {
        Movement.OnPlayerMovement -= PlayerMovementSound;
        RangedWeapon.OnFire -= PlayerFireSound;
        Enemy.OnEnemyDie -= EnemyDeathSound;
        Enemy.OnEnemySpawn -= EnemySpawnSound;
        Enemy.OnEnemyTakeDamage -= EnemyTakeDamageSound;
        RangedAttack.OnEnemyRangedAttack -= EnemyFireSound;
        PlayerHealthController.OnPlayerTakeDamage -= PlayerTakeDamageSound;
        PlayerHealthController.OnPlayerDie -= PlayerDeathSound;

        //Rooms/Waves
        RoomManager.OnRoomComplete -= RoomCompleteSound;
        RoomManager.OnWaveComplete -= WaveCompleteSound;

        CollectItem.OnDoorInteraction -= PlayMusic;
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            // Changing the volume of the sound depending on user settings
            s.source.volume = s.volume * sfx * master;
            s.source.Play();
        }
            
    }

    public void PlaySFXWithOverlap(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        // Changing the volume of the sound depending on user settings
        s.source.volume = s.volume * sfx * master;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void PauseAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
    }

    public void UnPauseAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
    }


    //FadeOut is inspired by https://forum.unity.com/threads/fade-out-audio-source.335031/
    public void FadeOut(Sound s)
    {
        float startVolume = s.source.volume;

        float fadeTime = 0.50f;
        
        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / fadeTime;
        }

        s.source.Stop();
        s.source.volume = startVolume;
    }

    public void GoToMainMenu()
    {
        StopAll();

        //Play main menu music
        //Set current music to main menu music
    }

    public void PlayMusic(DoorPreviewController.RoomType roomType)
    {
        if (roomType == DoorPreviewController.RoomType.BOSS)
        {
            //FadeOut(currentMusic);
            //Play boss music (should loop)
            //Set current music to boss music
        }
        else if (roomType == DoorPreviewController.RoomType.SHOP)
        {
            //FadeOut(currentMusic);
            //Play shop music (should loop)
            //Set current music to shop music
        }
        else
        {
            if (!true)
            {
                //If current music is normal music, do nothing
            }
            else
            {
                //FadeOut(currentMusic);
                //Play normal music (should loop)
                //Set current music to normal music
            }
        }

        //Room types
        //UNASSIGNED,
        //ITEM_DROP,
        //PENNINGAR_DROP,
        //HEALTH_DROP,

        //SHOP,
        //BOSS
    }



    public void RoomCompleteSound(DoorPreviewController.RoomType x, DoorPreviewController.RoomType y)
    {
        PlaySFX("gong1");
    }

    public void WaveCompleteSound()
    {
        PlaySFX("viking-horn1");
    }




    private string EnemyTypeToString(Enemy.EnemyType type)
    {
        return type switch
        {
            Enemy.EnemyType.SLIME => "slime",
            Enemy.EnemyType.WORM => "worm"
        };
    }

    void EnemyTakeDamageSound(Enemy.EnemyType type)
    {
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-hit{UnityEngine.Random.Range(1, 2)}");
    }

    void EnemyDeathSound(Enemy.EnemyType type)
    {
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-death{UnityEngine.Random.Range(1, 4)}");
    }

    void EnemySpawnSound(Enemy.EnemyType type)
    {
        PlaySFXWithOverlap("enemy-spawn");
    }

    void EnemyFireSound(Enemy.EnemyType type)
    {
        enemyFireCounter++;
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-fire{enemyFireCounter}");
        if (enemyFireCounter == 8)
            enemyFireCounter = 0;
    }
        


    void PlayerMovementSound()
    {
        //Adding variance to the step sound of the player
        //by randomizing volume and pitch for every step
        Sound s = Array.Find(sounds, sound => sound.name == "toke-step");
        s.source.volume = s.volume * UnityEngine.Random.Range(0.8f, 1) * sfx * master;
        s.source.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
        if (!s.source.isPlaying)
            s.source.Play();
    }

    void PlayerFireSound()
    {
        playerFireCounter++;
        PlaySFXWithOverlap($"toke-fire{playerFireCounter}");    
        if (playerFireCounter == 10)
            playerFireCounter = 0;
    }

    void PlayerTakeDamageSound()
    {
        PlaySFXWithOverlap($"toke-hit{UnityEngine.Random.Range(1, 4)}");
    }

    void PlayerDeathSound()
    {
        PlaySFX($"toke-death{UnityEngine.Random.Range(1, 3)}");
        PlaySFX("death-music");
    }
}
