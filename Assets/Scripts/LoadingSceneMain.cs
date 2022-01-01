using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneMain : BaseSceneMain
{
    const float NextSceneInterval = 3.0f;
    const float TextUpdateInterval = 0.15f;
    const string LoadingTextValue = "Loading...";

    [SerializeField]
    Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;

    float SceneStartTime;
    bool NextsceneCall = false;

    protected override void OnStart()
    {
        SceneStartTime = Time.time;
    }

    protected override void UpdateScene()
    {
        base.UpdateScene();
        float currentTime = Time.time;
        if(currentTime - LastUpdateTime > TextUpdateInterval)
        {
            
            LoadingText.text = LoadingTextValue.Substring(0, TextIndex + 1);

            TextIndex++;
            if(TextIndex >= LoadingTextValue.Length)
            {
                TextIndex = 0;
            }

            LastUpdateTime = currentTime;
        }
        if (currentTime - SceneStartTime > NextSceneInterval)
        {
            if (!NextsceneCall)
            {
                GoToNextScene();
            }
        }
    }

    void GoToNextScene()
    {
        SceneController.Instance.LoadScene(SceneNameContstants.InGame);
        NextsceneCall = true;
    }

}
