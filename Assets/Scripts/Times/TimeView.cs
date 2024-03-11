using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TimeView : MonoBehaviour
{
    [SerializeField]
    private NumberUIAsset numberUIAsset;
    private Image image;

    [SerializeField]
    private AudioClip countSE;
    private AudioSource audioSource;
    

    private void Awake()
    {
        image = this.GetComponent<Image>();
        audioSource = this.GetComponent<AudioSource>();
    }
    public void Display(string displayString)
    {
        image.sprite = numberUIAsset[int.Parse(displayString)];
        audioSource.PlayOneShot(countSE);
    }

    public void Close()
    {
        image.enabled = false;
    }
}
