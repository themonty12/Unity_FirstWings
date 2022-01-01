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
    EnemyTable enemyTable;
    public EnemyTable EnemyTable
    {
        get { return enemyTable; }
    }

    BaseSceneMain currentSceneMain;

    public BaseSceneMain CurrentSceneMain
    {
        set { currentSceneMain = value; }
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

    private void Start()
    {
        BaseSceneMain baseSceneMain = GameObject.FindObjectOfType<BaseSceneMain>();
        Debug.Log("OnSceneLoaded! baseSceneMain.name = " + baseSceneMain.name);
        SystemManager.instance.currentSceneMain = baseSceneMain;
    }

    private void Update()
    {
        
    }

    public T GetCurrentSceneMain<T>()
        where T : BaseSceneMain
    {
        return currentSceneMain as T;
    }

}
