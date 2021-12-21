using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class BossHealthController : MonoBehaviour
{
    public event Action<Boss, float, float> OnBossTakeDamage;
    public event Action<Boss> OnBossDefeated;
    public event Action OnBossBecomeVulnerable;
    public event Action OnBossBecomeInVulnerable;
    
    [Min(100)]
    public int hp = 1000;

    private float current_hp;
    private bool _vulnerable = true;
    
    private Animator _animator;
    private Collider2D _collider2d;
 
    
    private static readonly int Death = Animator.StringToHash("death");
    private static readonly int GetHit = Animator.StringToHash("get hit");

    public enum Boss
    {
        Hel
    }

    public Boss boss;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2d = GetComponent<Collider2D>();
        current_hp = hp;
    }
    private void SetVulnerability(bool vulnerable = true)
    {
        _vulnerable = vulnerable;
        _collider2d.enabled = vulnerable;
    }

    private void MakeVulnerable()
    {
        SetVulnerability(vulnerable: true);
        OnBossBecomeVulnerable?.Invoke();
    }

    private void MakeInVulnerable()
    {
        SetVulnerability(vulnerable: false);
        OnBossBecomeVulnerable?.Invoke();
    } 
    
    public void TakeDamage(float damage)
    {
        if (!_vulnerable)
        {
            Debug.Log("Haha fool! i am invulnerable");
            return;
        }
        
        current_hp -= damage;
        
        if (current_hp <= 0f)
        {
            Die();
        }
        else
        {
            _animator.SetTrigger(GetHit);
            OnBossTakeDamage?.Invoke(boss, current_hp, hp);
        }
    }

    private void Die()
    {
        // should call DestroySelf()
        Debug.Log("boss is dead");
        _animator.SetTrigger(Death);
        OnBossDefeated?.Invoke(boss);
    }
    
    public void DestroySelf() => Destroy(gameObject);
}
