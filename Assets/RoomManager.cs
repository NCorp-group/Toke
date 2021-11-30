using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    public static event Action OnRoomComplete;
    public static event Action OnRoomExit;
    public static event Action OnRoomEnter;

    private int _n_waves;
    private bool _room_completed;
    private int _enemies_alive;

    private List<Transform> spawningPoints;
    
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
    void Start()
    {
        waves.ForEach((wave) => { _n_waves += wave.repetitions; });
       
        spawningPoints = GetComponentsInChildren<Transform>().Where(tf => tf.gameObject.CompareTag("EnemySpawningPoint")).ToList();
        Assert.IsNotNull(spawningPoints);

        StartCoroutine(Spawn());
        // StartCoroutine(Wait(10));
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
    }

    private void OnDisable()
    {
        Enemy.OnEnemySpawn -= EnemySpawnCB;
        Enemy.OnEnemyDie -= EnemyDieCB;
    }

    private void EnemySpawnCB()
    {
        Debug.Log("enemy spawned");
        _enemies_alive += 1;
    }

    private void EnemyDieCB()
    {
        Debug.Log("enemy died");
        _enemies_alive -= 1;
    }
    

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"waves left = {_n_waves} enemies left = {_enemies_alive}");
        if (_n_waves == 0 && _enemies_alive == 0)
        {
            _room_completed = true;
            OnRoomComplete?.Invoke();
        }
    }

    private IEnumerator Spawn()
    {


        foreach (var wave in waves)
        {
            Assert.IsTrue(wave.startDelay >= 0);
            yield return new WaitForSeconds(wave.startDelay);
            SpawnWave(wave);
            _n_waves--;
        }
    }

    private void SpawnWave(EnemyWave wave)
    {
        foreach (var enemy in wave.enemies)
        {
            var sp = PickRandomEnemySpawningPoint();
            Instantiate<GameObject>(enemy.gameObject, sp.position, Quaternion.identity);
        }
    }
    

    private Transform PickRandomEnemySpawningPoint() => spawningPoints[Random.Range(0, spawningPoints.Count)];
        
}



