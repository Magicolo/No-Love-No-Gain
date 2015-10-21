using UnityEngine;
using System.Collections;
using Magicolo;

public class CrabPince : MonoBehaviour {

    [Disable]
    public float timer;

    public float MinCoolDown;
    public float MaxCoolDown;

    public float AttackTime;
    public float AttackDamage;
    public DamageType AttackType;

    private SpriteRenderer Sprite;

    public enum CrabPinceState{CoolDown, Attacking};
    public CrabPinceState state;

    void Start()
    {
        this.Sprite = GetComponent<SpriteRenderer>();
        GoInCoolDown();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        switch (this.state)
        {
            case CrabPinceState.CoolDown: CooDown(); break;
            case CrabPinceState.Attacking: Attack(); break;

        }
    }

    private void Attack()
    {
        if (timer <= 0)
        {
            GoInCoolDown();
        }
    }

    private void CooDown()
    {
        if(timer < 0)
        {
            PrepareAttack();
        }
    }

    private void GoInCoolDown()
    {
        timer = Random.Range(MinCoolDown, MaxCoolDown);
        this.Sprite.enabled = false;
        state = CrabPinceState.CoolDown;
    }


    private void PrepareAttack()
    {
        timer = AttackTime;
        this.Sprite.enabled = true;
        state = CrabPinceState.Attacking;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (state == CrabPinceState.Attacking)
        {
            IDamageable damagable = other.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.Damage(this.AttackDamage, this.AttackType);
            }
        }
    }
}
