using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneMain : BaseSceneMain
{
    const float GameReadyInterval = 3.0f;
    public enum GameState : int
    {
        Ready = 0,
        Running,
        End,
    }

    GameState currentGameState = GameState.Ready;
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }
    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {
            if (!player)
            {
                Debug.LogError("Main Player is not setted!");
            }

            return player;
        }
        set
        {
            player = value;
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
 

    [SerializeField]
    SquadronManager squadronManager;
    public SquadronManager SquadronManager
    {
        get
        {
            return squadronManager;
        }
    }

    float SceneStartTime;

    [SerializeField]
    Transform mainBGQuadTransform;

    public Transform MainBGQuadTransform
    {
        get
        {
            return mainBGQuadTransform;
        }
    }
    protected override void OnStart()
    {
        SceneStartTime = Time.time;
    }
    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;

        if(currentGameState == GameState.Ready)
        {
            if (currentTime - SceneStartTime > GameReadyInterval)
            {
                //squadronManager.StartGame();
                currentGameState = GameState.Running;
            }
        }
    }
}
