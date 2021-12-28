using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneMain : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScene();
    }

    private void OnDestroy()
    {
        Terminate();
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }

    /// <summary>
    /// �ܺο��� �ʱ�ȭ ȣ��
    /// </summary>
    
    public virtual void Initialize()
    {

    }

    protected virtual void UpdateScene()
    {

    }

    protected virtual void Terminate()
    {

    }
}
