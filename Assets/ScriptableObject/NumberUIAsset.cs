using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NumberUIAsset", menuName = "ScriptableObjects/CreateNumberUIAsset")]
public class NumberUIAsset : ScriptableObject
{
    public Sprite Zero;
    public Sprite One;
    public Sprite Two;
    public Sprite Three;
    public Sprite Four;
    public Sprite Five;
    public Sprite Six;
    public Sprite Seven;
    public Sprite Eight;
    public Sprite Nine;
    public Sprite this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:return Zero;
                case 1:return One;
                case 2:return Two;
                case 3:return Three;
                case 4:return Four;
                case 5:return Five;
                case 6:return Six;
                case 7:return Seven;
                case 8:return Eight;
                case 9:return Nine;
            }
            return null;
        }
    }
}
