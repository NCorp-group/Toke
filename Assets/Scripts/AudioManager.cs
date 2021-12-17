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

    }


    public void Play(string name)
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

    public void PlayWithOverlap(string name)
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


    public void GoToMainMenu()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }

        //Play main menu music
    }
    
    public void PlayMusic()
    {
        //Sound s = Array.Find(sounds, sound => sound.name == "music");
        // Changing the volume of the sound depending on user settings
        //s.source.volume = s.volume * music * master;
        //s.source.Play();
    }


    public void RoomCompleteSound(DoorPreviewController.RoomType x, DoorPreviewController.RoomType y)
    {
        Play("gong1");
    }

    public void WaveCompleteSound()
    {
        Play("viking-horn1");
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
        PlayWithOverlap($"{EnemyTypeToString(type)}-hit{UnityEngine.Random.Range(1, 2)}");
    }


    void EnemyDeathSound(Enemy.EnemyType type)
    {
        PlayWithOverlap($"{EnemyTypeToString(type)}-death{UnityEngine.Random.Range(1, 4)}");
    }


    void EnemySpawnSound(Enemy.EnemyType type)
    {
        PlayWithOverlap("enemy-spawn");
    }

    void EnemyFireSound(Enemy.EnemyType type)
    {
        enemyFireCounter++;
        PlayWithOverlap($"{EnemyTypeToString(type)}-fire{enemyFireCounter}");
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
        PlayWithOverlap($"toke-fire{playerFireCounter}");
        if (playerFireCounter == 10)
            playerFireCounter = 0;
    }

    void PlayerTakeDamageSound()
    {
        PlayWithOverlap($"toke-hit{UnityEngine.Random.Range(1, 4)}");
    }

    void PlayerDeathSound()
    {
        Play($"toke-death{UnityEngine.Random.Range(1, 3)}");
        Play("death-music");
    }
}
