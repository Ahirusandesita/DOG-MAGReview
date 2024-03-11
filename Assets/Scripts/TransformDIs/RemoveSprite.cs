using UnityEngine;

public class RemoveSprite : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this.GetComponent<SpriteRenderer>());
        Destroy(this);
    }
}
