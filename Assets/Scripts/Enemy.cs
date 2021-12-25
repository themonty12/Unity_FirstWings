using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public enum State : int
    {
        None = -1, // �����
        Ready = 0, // �غ�Ϸ�
        Appear, // ����
        Battle, // ������
        Dead, // ���
        Disappear, // ����
    }
    
    [SerializeField]
    State CurrentState = State.None;

    const float MaxSpeed = 5.0f;

    const float MaxSpeedTime = 0.5f;

    [SerializeField]
    Vector3 TargetPosition;

    [SerializeField]
    float CurrentSpeed;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    float BulletSpeed = 1;

    Vector3 CurrentVelocity;

    float MoveStartTime = 0.0f;

    float LastActionUpdateTime = 0.0f;

    [SerializeField]
    int FireRemainCount = 1;

    [SerializeField]
    int GamePoint = 10;

    public string FilePath
    {
        get;
        set;
    }

    Vector3 AppearPoint;    // ����� ���� ��ġ
    Vector3 DisappearPoint; // ����� ��ǥ ����
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    protected override void UpdateActor()
    {
        switch (CurrentState)
        {
            case State.None:
                break;
            case State.Ready:
                UpdateReady();
                break;
            case State.Dead:
                break;
            case State.Appear:

            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattel();
                break;

        }

        if (CurrentState == State.Appear || CurrentState == State.Disappear)
        {
            UpdateSpeed();
            UpdateMove();
        }

    }


    void UpdateSpeed()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStartTime)/MaxSpeedTime);
    }

    void UpdateMove()
    {
        // �ӵ� = �Ÿ� / �ð�
        // �ð� = �Ÿ� / �ӵ�
        float distance = Vector3.Distance(TargetPosition, transform.position);
        if(distance == 0)
        {
            Arrived();
            return;
        }

        CurrentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed;

        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed);
        
        //if (CurrentState == State.Disappear)
        //{
        //    transform.Rotate(Vector3.right * Time.deltaTime * 45);
        //}

    }

    void Arrived()
    {
        CurrentSpeed = 0.0f;
        if (CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastActionUpdateTime = Time.time;
        }
        else if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
            SystemManager.Instance.EnemyManager.RemoveEnemy(this);
        }
    }

    public void Reset(EnemyGenerateData data)
    {
        CurrentHP = MaxHP = data.MaxHP;             // CurrentHP���� �ٽ� �Է�
        Damage = data.Damage;                       // �Ѿ� ������
        crashDamage = data.CrashDamage;             // �浹 ������
        BulletSpeed = data.BulletSpeed;             // �Ѿ� ���ǵ�
        FireRemainCount = data.FireRemainCount;     // �߻��� �Ѿ� ����
        GamePoint = data.GamePoint;                 // �ı��� ���� ����

        AppearPoint = data.AppearPoint;             // ����� ���� ��ġ
        DisappearPoint = data.DisappearPoint;       // ����� ���� ��ġ

        CurrentState = State.Ready;
        LastActionUpdateTime = Time.time;
    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;
        CurrentState = State.Appear;

        MoveStartTime = Time.time;

    }

    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0;
        CurrentState = State.Disappear;

        MoveStartTime = 0.0f;
    }

    void UpdateReady()
    {
        if (Time.time - LastActionUpdateTime > 1.0f)
        {
            Appear(AppearPoint);
        }
    }

    void UpdateBattel()
    {
        if (Time.time - LastActionUpdateTime > 1.0f)
        {

            if(FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(DisappearPoint);
            }


            LastActionUpdateTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter Enemy");
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            //Debug.Log("Into Player");
            if (!player.IsDead)
            {
                BoxCollider box = ((BoxCollider)other);
                Vector3 crashPos = player.transform.position + box.center;
                crashPos.x += box.size.x * 0.5f;
                player.OnCrash(this, crashDamage, other.transform.position);
            }
            
        }
    }

    public override void OnCrash(Actor attacker, int damage, Vector3 crashPos)
    {
        
        base.OnCrash(attacker, damage, crashPos);
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.EnemyBulletIndex);
        bullet.Fire(this, FireTransform.position, -FireTransform.right, BulletSpeed, Damage);
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);

        SystemManager.Instance.GamePointAccumulator.Accumulate(GamePoint);
        SystemManager.Instance.EnemyManager.RemoveEnemy(this);

        CurrentState = State.Dead;
        
    }

    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value, damagePos);
        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.DamageManager.Generate(DamageManager.EnemyDamageIndex, damagePoint , value, Color.magenta);
    }
}
