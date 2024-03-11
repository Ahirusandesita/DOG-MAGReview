using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreView : MonoBehaviour
{
    public void Display(int score)
    {
        this.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
