using UnityEngine.Audio;
using System;
using System.Collections;
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

    private string currentMusic;

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

        //Music
        RoomManager.OnRoomEnter += StartMusic;
        RoomManager.OnRoomExit += FadeMusic;
        //InteractableArea.OnDoorInteraction += FadeMusic;

        //MainMenu.OnMainMenu += GoToMainMenu;
        //MainMenu.OnStartGame += StartGame;

        //TODO: Killing boss stops music and plays some other music indicating game is over
        //TODO: Music volume can be changed mid game (when music is unpaused, volume is adjusted?)
    }


    void OnEnable()
    {
        MainMenu.OnMainMenu += GoToMainMenu;
        MainMenu.OnStartGame += StartGame;
    }


    void OnDisable()
    {
        MainMenu.OnMainMenu -= GoToMainMenu;
        MainMenu.OnStartGame -= StartGame;
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

    public void PlayMusic(string name)
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
            s.source.volume = s.volume * music * master;
            s.source.Play();
        }
        else
        {
            Debug.Log("Song already playing");
        }
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
    private IEnumerator _FadeOut(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float startVolume = s.source.volume;

        while (s.source.volume > .1f)
        {
            s.source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        s.source.Stop();
        s.source.volume = startVolume;
    }
    
    public void FadeOut(string name, float fadeTime)
    {
        StartCoroutine(_FadeOut(name, fadeTime));
    }


    public void GoToMainMenu()
    {
        StopAll();
        //StopAll();
        PlayMusic("menu-music");
        currentMusic = "menu-music";
    }

    public void StartGame()
    {
        FadeOut("menu-music", 1f);
    }

    public void FadeMusic(DoorPreviewController.RoomType roomType)
    {
        Debug.Log("FadeMusic");
        float fadeTime = 5f;

        if (roomType == DoorPreviewController.RoomType.BOSS && currentMusic != "boss-music")
        {
            FadeOut(currentMusic, fadeTime);
        }
        else if (roomType == DoorPreviewController.RoomType.SHOP && currentMusic != "menu-music")
        {
            FadeOut(currentMusic, fadeTime);
        }
        else
        {
            if (currentMusic != "default-music")
            {
                FadeOut(currentMusic, fadeTime);
            }
        }
    }

    public void StartMusic(DoorPreviewController.RoomType roomType)
    {
        Debug.Log("StartMusic");
        if (roomType == DoorPreviewController.RoomType.BOSS && currentMusic != "boss-music")
        {
            PlayMusic("boss-music");
            currentMusic = "boss-music";
        }
        else if (roomType == DoorPreviewController.RoomType.SHOP && currentMusic != "menu-music")
        {
            PlayMusic("menu-music");
            currentMusic = "menu-music";
        }
        else
        {
            if (currentMusic != "default-music")
            {
                PlayMusic("default-music");
                currentMusic = "default-music";
            }
        }
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
        //-hit
        //-death
        //-fire
        return type switch
        {
            Enemy.EnemyType.SLIME => "slime", //Done
            Enemy.EnemyType.WORM => "worm",
            Enemy.EnemyType.BLUESLIME => "blueslime",
            Enemy.EnemyType.DARKBORNIMP => "darkbornimp",
            Enemy.EnemyType.ARCANEARCHER => "arcanearcher",
            Enemy.EnemyType.EVILWIZARD => "evilwizard",
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
        StopAll();
        PlaySFX($"toke-death{UnityEngine.Random.Range(1, 3)}");
        PlaySFX("death-music");
    }
}
