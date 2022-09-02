using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenScore : MonoBehaviour
{
    private TextMeshProUGUI score;
    
    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        score.text = Score.GetScore().ToString();
    }
}
