using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform playerTranform;
    private Text score;
    private int value = 0;
    private static int printedValue = 0;
    private float maxPlayerPosX;

    void Start()
    {
        maxPlayerPosX = playerTranform.position.x;
        score = GetComponent<Text>();
    }

    void Update()
    {
        if(playerTranform.position.x > maxPlayerPosX)
        {
            maxPlayerPosX = playerTranform.position.x;
            value++;
        }
        printedValue = value / 10;
        score.text = printedValue.ToString();
    }

    public static int GetScore()
    {
        return printedValue;
    }
}
