using System;
using System.Collections;
using UnityEngine;

// This AudioManager is partly inspired by this youtube video
// https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] music;

    public static AudioManager instance;

    private int playerFireCounter = 0;
    private int enemyFireCounter = 0;
    private int enemyAttackCounter = 0;
    private int darkSpikesSpawnCounter = 0;
    private int helBasicAttackCounter = 0;

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

        foreach (Music m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    private BossHealthController bhc;
    private Hel hel;

    void Start()
    {

        Movement.OnPlayerMovement += PlayerMovementSound;
        Movement.OnPlayerDash += PlayerDashSound;
        RangedWeapon.OnFire += PlayerFireSound;
        Enemy.OnEnemyDie += EnemyDeathSound;
        Enemy.OnEnemySpawn += EnemySpawnSound;
        Enemy.OnEnemyTakeDamage += EnemyTakeDamageSound;
        RangedAttack.OnEnemyRangedAttack += EnemyFireSound;
        MeleeAttack.OnEnemyMeleeAttack += EnemyAttackSound;
        Stats.OnPlayerTakeDamage += PlayerTakeDamageSound;
        Stats.OnPlayerDie += PlayerDeathSound;

        //Rooms/Waves
        //RoomManager.OnRoomComplete += RoomCompleteSound;
        RoomManager.DropReward += RoomCompleteSound;
        RoomManager.OnWaveComplete += WaveCompleteSound;

        //Music
        RoomManager.OnRoomEnter += StartMusic;
        RoomManager.OnRoomExit += FadeMusic;
        //InteractableArea.OnDoorInteraction += FadeMusic;

        //Volume
        OptionsMenu.OnVolumeChanged += ChangeVolume;

        Hel.OnHelDoSpecialAttackStatic += HelSpecialAttackSound;
        Hel.OnHelDoBasicAttackStatic += HelBasicAttackSound;
 

        BlackSpike.OnSpikeSpawn += DarkSpikesSpawnSound;
        GravitySphere.OnGravitySphereSpawn += GravitySphereSpawnSound;

        BossHealthController.OnBossTakeDamageStatic += HelTakeDamageSound;
        BossHealthController.OnBossDefeatedStatic += BossDeathSound;

        BifrostLight.OnBifrost += BifrostSound;


        //TODO: Killing boss stops music and plays some other music indicating game is over
        //TODO: Music volume can be changed mid game (when music is unpaused, volume is adjusted?)
    }


    void OnEnable()
    {
        MainMenu.OnMainMenu += GoToMainMenu;
        StoryController.OnStartGame += StartGame;
    }

    void OnDisable()
    {
        MainMenu.OnMainMenu -= GoToMainMenu;
        StoryController.OnStartGame -= StartGame;
    }

    public void ChangeVolume(float _master, float _music, float _sfx)
    {
        //Debug.Log($"Master Volume (Audio): {_master}");
        //Debug.Log($"Music Volume (Audio): {_music}");
        //Debug.Log($"SFX Volume (Audio): {_sfx}");
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * _master * _sfx;
        }
        foreach (Music m in music)
        {
            m.source.volume = m.volume * _master * _music;
        }
    }

    public void PlayMusic(string name)
    {
        Music m = Array.Find(music, music => music.name == name);
        if (m == null)
        {
            //Debug.LogWarning("Music: " + name + "not found!");
            return;
        }
        if (!m.source.isPlaying)
        {
            m.source.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }     
    }

    public void PlaySFXWithOverlap(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        // Changing the volume of the sound depending on user settings
        //s.source.volume = s.volume;
        s.source.Play();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
        foreach (Music m in music)
        {
            m.source.Stop();
        }
    }


    //FadeOut is inspired by https://forum.unity.com/threads/fade-out-audio-source.335031/
    private IEnumerator _FadeOutMusic(string name, float fadeTime)
    {
        Music m = Array.Find(music, music => music.name == name);

        if (m != null)
        {
            float startVolume = m.source.volume;

            while (m.source.volume > .1f)
            {
                m.source.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            m.source.Stop();
            m.source.volume = startVolume;
        }
    }
    
    public void FadeOutMusic(string name, float fadeTime)
    {
        StartCoroutine(_FadeOutMusic(name, fadeTime));
    }




    public void BifrostSound()
    {
        PlaySFX("bifrost");
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
        FadeOutMusic("menu-music", 1f);
    }

    public void FadeMusic(DoorPreviewController.RoomType roomType)
    {
        float fadeTime = 5f;

        if (roomType == DoorPreviewController.RoomType.BOSS && currentMusic != "boss-music")
        {
            FadeOutMusic(currentMusic, fadeTime);
        }
        else if (roomType == DoorPreviewController.RoomType.SHOP && currentMusic != "menu-music")
        {
            FadeOutMusic(currentMusic, fadeTime);
        }
        else
        {
            if (currentMusic != "default-music")
            {
                FadeOutMusic(currentMusic, fadeTime);
            }
        }
    }

    public void StartMusic(DoorPreviewController.RoomType roomType)
    {
        //Debug.Log("StartMusic");
        if (roomType == DoorPreviewController.RoomType.BOSS && currentMusic != "boss-music")
        {
            PlayMusic("boss-music");
            Music m = Array.Find(music, music => music.name == "boss-music");
            Debug.Log($"Bos music volume: {m.source.volume}");
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

    public void RoomCompleteSound(DoorPreviewController.RoomType roomType)
    {
        if (roomType != DoorPreviewController.RoomType.SHOP && roomType != DoorPreviewController.RoomType.UNASSIGNED)
        {
            PlaySFX("gong1");
        }
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
            Enemy.EnemyType.SLIME => "slime", 
            Enemy.EnemyType.WORM => "worm",
            Enemy.EnemyType.BLUESLIME => "blueslime",
            Enemy.EnemyType.NIGHTBORNIMP => "nightbornimp",
            Enemy.EnemyType.ARCANEARCHER => "arcanearcher",
            Enemy.EnemyType.EVILWIZARD => "evilwizard",
        };
    }

    void EnemyTakeDamageSound(Enemy.EnemyType type)
    {
        //Currently same hit-sound for all
        PlaySFXWithOverlap($"worm-hit{UnityEngine.Random.Range(1, 2)}");
    }

    void EnemyDeathSound(Enemy.EnemyType type)
    {
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-death{UnityEngine.Random.Range(1, 4)}");
        Debug.Log($"Enemy Death: {type}");
    }

    void EnemySpawnSound(Enemy.EnemyType type)
    {
        PlaySFXWithOverlap("enemy-spawn");
    }

    void EnemyFireSound(Enemy.EnemyType type)
    {
        enemyFireCounter++;
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-fire{enemyFireCounter}");
        if (enemyFireCounter == 4)
            enemyFireCounter = 0;
    }

    void EnemyAttackSound(Enemy.EnemyType type)
    {
        enemyAttackCounter++;
        PlaySFXWithOverlap($"{EnemyTypeToString(type)}-attack{enemyAttackCounter}");
        if (enemyAttackCounter == 4)
            enemyAttackCounter = 0;
    }



    private string HelSpecialAttackTypeToString(Hel.SpecialAttack type)
    {
        return type switch
        {
            //Hel.SpecialAttack.SpawnGravitationalCircles => "spawngravitationalcircles",
            Hel.SpecialAttack.IciclePincer => "iciclepincer",
            Hel.SpecialAttack.SwirlSpray => "swirlspray",
            //Hel.SpecialAttack.SpawnMinions => "spawnminions", //Irrelevant
            //Hel.SpecialAttack.SpawnDarkSpikes => "spawndarkspikes", 
            _ => "",
        };
    }

    void HelSpecialAttackSound(Hel.SpecialAttack a)
    {
        String attack = HelSpecialAttackTypeToString(a);
        if(attack != "")
        {
            PlaySFX($"hel-{attack}");
        }
    }

    void HelBasicAttackSound()
    {
        helBasicAttackCounter++;
        PlaySFX($"hel-basicattack{helBasicAttackCounter}");
        if (helBasicAttackCounter == 3)
            helBasicAttackCounter = 0;
    }

    void GravitySphereSpawnSound()
    {
        PlaySFX("enemy-spawn");
    }

    void DarkSpikesSpawnSound()
    {
        darkSpikesSpawnCounter++;
        PlaySFX($"darkspikes-spawn{darkSpikesSpawnCounter}");
        if (darkSpikesSpawnCounter == 5)
            darkSpikesSpawnCounter = 0;
    }

    void BossDeathSound(BossHealthController.Boss type)
    {
        switch (type)
        {
                case BossHealthController.Boss.Hel:
                PlaySFX("hel-death");
                break;
        }

        FadeMusic(DoorPreviewController.RoomType.BOSS);
    }

    void HelTakeDamageSound()
    {
        PlaySFXWithOverlap($"worm-hit{UnityEngine.Random.Range(1, 2)}");
    }




    void PlayerMovementSound()
    {
        //Adding variance to the step sound of the player
        //by randomizing volume and pitch for every step
        Sound s = Array.Find(sounds, sound => sound.name == "toke-step");
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

    void PlayerDashSound()
    {
        PlaySFXWithOverlap($"toke-dash");
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
