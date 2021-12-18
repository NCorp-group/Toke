using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static DoorPreviewController;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    public static event Action<RoomType, RoomType> OnRoomComplete;
    public static event Action<RoomType> DropReward; 
    public static event Action OnWaveComplete;
    public static event Action OnRoomExit;
    public static event Action OnRoomEnter;

    private int _n_waves;
    private bool _room_completed;
    private int _enemies_alive;
    /// <summary>
    /// used to avoid a race condition
    /// </summary>
    private bool _an_enemy_has_spawned = false;

    private List<(Transform, bool)> spawningPoints;
    private RoomType dropType;

    [System.Serializable]
    public class EnemyWave
    {
        public enum EnemyWaveVariant
        {
            PERIODIC,
            CONDITIONAL,
            NATURAL,
        }

        [Header("A delay in seconds added before the wave spawns.")]
        public float startDelay = 0f;
        [Header("TODO:")]
        public EnemyWaveVariant typeOfWave = EnemyWaveVariant.NATURAL;
        public float period = 1f;
        public Func<bool> condition; // hmm
        [Header("Number of times this wave should be repeatedly spawned.")]
        [Range(1, 10)]
        public int repetitions = 1;
        public List<Enemy> enemies;
    }

    public List<EnemyWave> waves = new List<EnemyWave>();

    //when to spawn next wave ??? periodic or on event ???
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("START");
        waves.ForEach((wave) => { _n_waves += wave.repetitions; });
        spawningPoints = GetComponentsInChildren<Transform>()
            .Where(tf => tf.gameObject.CompareTag("EnemySpawningPoint"))
            .Select(tf => (tf, false))
            .ToList();

        // spawningPoints = GetComponentsInChildren<Transform>().Where(tf => tf.gameObject.CompareTag("EnemySpawningPoint")).ToList();
        Assert.IsNotNull(spawningPoints);

        StartCoroutine(Spawn());

        
        dropType = (RoomType) PlayerPrefs.GetInt(ROOM_TYPE);
        Debug.Log("This room's reward is: " + dropType);
    }

    private void ChangeRoom(RoomType nextRoomType)
    {
        StartCoroutine(_ChangeRoom(nextRoomType));
    }

    private IEnumerator _ChangeRoom(RoomType nextRoomType)
    {
        
        yield return new WaitUntil(() => writtenToPlayerPrefs);
        switch (nextRoomType)
        {
            case RoomType.SHOP:
                SceneManager.LoadScene(8);
                break;
            case RoomType.BOSS:
                SceneManager.LoadScene(9);
                break;
            default:
                var nextRoom = Random.Range(2, 6);
                SceneManager.LoadScene(nextRoom);
                break;
        }
    }

    // dummy stub
    private IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _n_waves = 0;
    }

    private void OnEnable()
    {
        Enemy.OnEnemySpawn += EnemySpawnCB;
        Enemy.OnEnemyDie += EnemyDieCB;
        InteractableArea.OnDoorInteraction += ChangeRoom;
    }

    private void OnDisable()
    {
        Enemy.OnEnemySpawn -= EnemySpawnCB;
        Enemy.OnEnemyDie -= EnemyDieCB;
        InteractableArea.OnDoorInteraction -= ChangeRoom;
    }

    private void EnemySpawnCB(Enemy.EnemyType type)
    {
        _an_enemy_has_spawned = true;
        //Debug.Log("enemy spawned");
        _enemies_alive += 1;
    }

    private void EnemyDieCB(Enemy.EnemyType type)
    {
        //Debug.Log("enemy died");
        _enemies_alive -= 1;
        if (_enemies_alive == 0)
        {
            _n_waves--;
            if (_n_waves > 0)
            {
                OnWaveComplete?.Invoke();
            }
        }
        Debug.Log($"enemies alive {_enemies_alive}");
        Debug.Log($"waves remaining: {_n_waves}");

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log($"waves left = {_n_waves} enemies left = {_enemies_alive}");
        Debug.Log($"_room_completed = {_room_completed}");
        if (_room_completed)
        {
            return;
        }
        if (_n_waves == 0)
        {
            _room_completed = true;
            var room1 = (RoomType)Random.Range(DROP_START, DROP_END);
            var room2 = (RoomType)Random.Range(DROP_START, DROP_END);
            while (room2 == room1)
            {
                room2 = (RoomType)Random.Range(DROP_START, DROP_END);
            }
            //Debug.Log("Room Complete");
            //Debug.Log("room1 = " + room1);
            //Debug.Log("room2 = " + room2);
            //Use this for implementing sound indicating all waves in a room is done
            OnRoomComplete?.Invoke(room1, room2);
            DropReward?.Invoke(dropType);
        }
    }

    private IEnumerator Spawn()
    {
        foreach (var wave in waves)
        {
            Assert.IsTrue(wave.startDelay >= 0);
            //Debug.Log($"enemies alive: {_enemies_alive}");
            yield return new WaitForSeconds(wave.startDelay);
            var _curr_n_waves = _n_waves;
            SpawnWave(wave);
            //Debug.Log($"_curr_n_waves = {_curr_n_waves}");

            yield return new WaitUntil(() => _enemies_alive == 0 && (_curr_n_waves - 1) == _n_waves && _an_enemy_has_spawned);
        }
    }

    private void SpawnWave(EnemyWave wave)
    {
        // don't know how to clone reference objects ...
        var spawningPointsClone = spawningPoints.Select(x => x).ToList();


        foreach (var enemy in wave.enemies)
        {
            var idx = Random.Range(0, spawningPointsClone.Count);
            var sp = spawningPointsClone[idx].Item1;
            spawningPointsClone.RemoveAt(idx);
            // var sp = PickRandomEnemySpawningPoint();
            Instantiate<GameObject>(enemy.gameObject, sp.position, Quaternion.identity);
        }

        spawningPoints = spawningPoints.Select(p => (p.Item1, false)).ToList();
    }


    // private Transform PickRandomEnemySpawningPoint() => spawningPoints[Random.Range(0, spawningPoints.Count)];

}