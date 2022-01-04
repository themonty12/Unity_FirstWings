using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : Actor
{
    [SerializeField]
    [SyncVar]
    Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    NetworkIdentity NetworkIdentity = null;

    [SerializeField]
    float Speed;

    [SerializeField]
    BoxCollider boxCollider;
        

    [SerializeField]
    Transform FireTransform;
        

    [SerializeField]
    float BulletSpeed = 1;

    protected override void Initialize()
    {
        base.Initialize();
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);

        if (isLocalPlayer)
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    protected override void UpdateActor()    
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        if(MoveVector.sqrMagnitude == 0)
        {
            return;
        }

        MoveVector = AdjustMoveVector(MoveVector);

        //transform.position += MoveVector;
        CmdMove(MoveVector);
    }

    [Command]
    public void CmdMove(Vector3 moveVector)
    {
        this.MoveVector = moveVector;
        transform.position += moveVector;
        base.SetDirtyBit(1);
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }

    Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Transform mainBGQuadTransform = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().MainBGQuadTransform;

        Vector3 result = Vector3.zero;
        

        result = boxCollider.transform.position + moveVector;

        if(result.x - boxCollider.size.x * 0.5f < -mainBGQuadTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0;
        }
        if (result.x + boxCollider.size.x * 0.5f > mainBGQuadTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0;
        }
        if (result.y - boxCollider.size.y * 0.5f < -mainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0;
        }
        if (result.y + boxCollider.size.y * 0.5f > mainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0;
        }

        return moveVector;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter Player");
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
        {
            if (!enemy.IsDead)
            {
                BoxCollider box = ((BoxCollider)other);
                Vector3 crashPos = enemy.transform.position + box.center;
                crashPos.x += box.size.x * 0.5f;
                enemy.OnCrash(this, CrashDamage, crashPos);
            }
            
        }
    }

    public override void OnCrash(Actor attacker, int damage, Vector3 crashPos)
    {
        base.OnCrash(attacker, damage, crashPos);
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
    }

    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value, damagePos);
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);

        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.PlayerDamageIndex, damagePoint , value, Color.red);
        
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        gameObject.SetActive(false);
    }
}
