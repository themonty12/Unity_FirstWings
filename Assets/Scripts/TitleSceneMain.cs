using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleSceneMain : BaseSceneMain
{
    public void OnStartButton()
    {
        Debug.Log("OnStartButton");

        PanelManager.GetPanel(typeof(NetworkConfigPanel)).Show();
    }

    public void GotoNextScene()
    {
        SceneController.Instance.LoadScene(SceneNameContstants.LoadingScene);
    }
}
