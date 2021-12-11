using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public enum State : int
    {
        None = -1, // 사용전
        Ready = 0, // 준비완료
        Appear, // 등장
        Battle, // 전투중
        Dead, // 사망
        Disappear, // 퇴장
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

    float LastBattelUpdateTime = 0.0f;

    [SerializeField]
    int FireRemainCount = 1;

    [SerializeField]
    int GamePoint = 10;

    public string FilePath
    {
        get;
        set;
    }
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

            case State.Ready:
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
        // 속도 = 거리 / 시간
        // 시간 = 거리 / 속도
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
            LastBattelUpdateTime = Time.time;
        }
        else if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
        }
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

    void UpdateBattel()
    {
        if (Time.time - LastBattelUpdateTime > 1.0f)
        {

            if(FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
            }
            

            LastBattelUpdateTime = Time.time;
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
                
                player.OnCrash(this, crashDamage);
            }
            
        }
    }

    public override void OnCrash(Actor attacker, int damage)
    {
        
        base.OnCrash(attacker, damage);
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
}
