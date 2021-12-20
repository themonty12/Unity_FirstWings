using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : BasePanel
{
    [SerializeField]
    Text scoreValue;

    [SerializeField]
    Gage HPGage;
    // Start is called before the first frame update

    public void SetScore(int value)
    {
        Debug.Log("SetSCore Value = " + value);

        scoreValue.text = value.ToString();
    }

    public void SetHP(float currentValue, float maxValue)
    {
        HPGage.SetHP(currentValue, maxValue);
    }
   
}
