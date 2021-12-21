using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;

    public static SystemManager Instance
    {
        get { return instance; }
    }
    [SerializeField]
    Player player;

    public Player Hero
    {
        get 
        {
            if(!player)
            {
                Debug.LogError("Main Player is not setted!");
            }

            return player; 
        }
    }

    GamePointAccumulator gamePointAccumulator = new GamePointAccumulator();

    public GamePointAccumulator GamePointAccumulator
    {
        get { return gamePointAccumulator; }
    }

    [SerializeField]
    EffectManager effectManager;

    public EffectManager EffectManager
    {
        get { return effectManager; }
    }

    [SerializeField]
    EnemyManager enemyManger;
    public EnemyManager EnemyManager
    {
        get { return enemyManger; }
    }

    [SerializeField]
    BulletManager bulletManager;

    public BulletManager BulletManager
    {
        get { return bulletManager; }
    }

    [SerializeField]
    DamageManager damageManager;
    public DamageManager DamageManager
    {
        get { return damageManager; }
    }
     
    PrefabCacheSystem enemyCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EnemyCacheSystem
    {
        get
        {
            return enemyCacheSystem;
        }
    }

    PrefabCacheSystem bullectCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem BulletCacheSystem
    {
        get
        {
            return bullectCacheSystem;
        }
    }

    PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EffectCacheSystem
    {
        get
        {
            return effectCacheSystem;
        }
    }

    PrefabCacheSystem damageCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem DamageCacheSystem
    {
        get
        {
            return damageCacheSystem;
        }
    }


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("SystemManager error");
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    //

}
