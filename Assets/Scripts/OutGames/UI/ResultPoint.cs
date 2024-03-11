using UnityEngine;
using UnityEngine.UI;

public class ResultPoint : MonoBehaviour
{
    [SerializeField] private NumberUIAsset numberUIAsset = default;
    [SerializeField] private Image tensPlaceImage = default;
    [SerializeField] private Image onesPlaceImage = default;


    public void SetNumber(int point)
    {
        int onesPlacePoint = point / 10;
        int tensPlacePoint = point - onesPlacePoint;

        if (tensPlacePoint == 0)
        {
            Destroy(tensPlaceImage.gameObject);
        }
        else
        {
            tensPlaceImage.sprite = numberUIAsset[tensPlacePoint - 1];
        }

        onesPlaceImage.sprite = numberUIAsset[onesPlacePoint];
    }
}
