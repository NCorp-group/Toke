using UnityEngine;

public class UpdateSceneBasedOnBossState : MonoBehaviour
{
    public Transform[] disableOnBossDefeated;
    public Transform[] enableOnBossDefeated;
    public Transform[] disableOnBossSpawn;
    public Transform[] enableOnBossSpawn;

    private BossHealthController _bhc;
    public SpawnObjectsOnTargetEnter spte;
    
    private void OnEnable()
    {
        spte.OnObjectsSpawned += OnObjectsSpawned;
    }

    private void OnObjectsSpawned()
    {
        foreach (var t in disableOnBossSpawn)
        {
            t.gameObject.SetActive(false);
        }

        foreach (var t in enableOnBossSpawn)
        {
            t.gameObject.SetActive(true);
        }

        _bhc = FindObjectOfType<BossHealthController>();
        _bhc.OnBossDefeated += OnBossDefeated;
    }

    private void OnBossDefeated(BossHealthController.Boss obj)
    {
        foreach (var t in disableOnBossDefeated)
        {
            t.gameObject.SetActive(false);
        }

        foreach (var t in enableOnBossDefeated)
        {
            t.gameObject.SetActive(true);
        }
    }
}
