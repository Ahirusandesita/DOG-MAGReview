using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDigitManager : MonoBehaviour
{
    [SerializeField]
    private NumberUIAsset numberUIAsset;

    [SerializeField]
    private Image Digit3;
    [SerializeField]
    private Image Digit2;
    [SerializeField]
    private Image Digit1;

    private int[] Digits = new int[3];
    public void Display(int displayNumber)
    {
        if(displayNumber > 99)
        {
            int digit1 = displayNumber % 10;
            Digits[0] = digit1;

            displayNumber -= digit1;
            displayNumber /= 10;

            int digit2 = displayNumber % 10;
            Digits[1] = digit2;
            displayNumber -= digit2;
            displayNumber /= 10;

            int digit3 = displayNumber % 10;
            Digits[2] = digit3;
        }

        else if(displayNumber > 9)
        {
            Digit3.enabled = false;
            int digit1 = displayNumber % 10;
            Digits[0] = digit1;

            displayNumber -= digit1;
            displayNumber /= 10;

            int digit2 = displayNumber % 10;
            Digits[1] = digit2;
        }
        else if(displayNumber > 0)
        {
            Digit2.enabled = false;
            Digits[0] = displayNumber;
        }

        Digit1.sprite = numberUIAsset[Digits[0]];

        if(Digits[2] != 0)
        {
            Digit3.sprite = numberUIAsset[Digits[2]];
        }

        if(Digits[2] == 0 && Digits[1] == 0)
        {
            return;
        }

        Digit2.sprite = numberUIAsset[Digits[1]];

    }
}
