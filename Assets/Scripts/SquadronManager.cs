using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquadronData
{
    public float SqadronGenerateTime;
    public Squadron sqadron;
}
public class SquadronManager : MonoBehaviour
{
    float GameStartedTime;

    int SquadronIndex;

    [SerializeField]
    SquadronData[] squadronDatas;

    bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartGame();
        }

        CheckSquadronGeneratings();
    }

    public void StartGame()
    {
        GameStartedTime = Time.time;
        SquadronIndex = 0;
        running = true;
        Debug.Log("Game Start!");
    }

    void CheckSquadronGeneratings()
    {
        if (!running)
            return;

        if(Time.time - GameStartedTime >= squadronDatas[SquadronIndex].SqadronGenerateTime)
        {
            GenerateSquadron(squadronDatas[SquadronIndex]);
            SquadronIndex++;

            if (SquadronIndex >= squadronDatas.Length)
            {
                AllSquadronGenerated();
                return;
            }
        }
    }

    void GenerateSquadron(SquadronData data)
    {
        Debug.Log("GenerateSquadron");
        data.sqadron.GenerateAllData();
    }

    void AllSquadronGenerated()
    {
        Debug.Log("AllSquadronGenrated");

        running = false;
    }
}
