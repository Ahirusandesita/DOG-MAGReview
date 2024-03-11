using UnityEngine;
using UniRx;

public class OutGameProgresser : MonoBehaviour
{
    [SerializeField] private TitleManager titleManager = default;
    [SerializeField] private PlayerCountSelectManager playerCountSelectManager = default;
    [SerializeField] private PlayerJoinManager playerJoinManager = default;
    [SerializeField] private CharacterSelectManager characterSelectManager = default;

    private void Awake()
    {
        titleManager.gameObject.SetActive(true);
        playerCountSelectManager.gameObject.SetActive(true);
        playerJoinManager.gameObject.SetActive(true);

        titleManager.SubmitTitleSubject.Subscribe(
            _ => playerCountSelectManager.OnAwake(),
           () => titleManager.gameObject.SetActive(false)
           );

        playerCountSelectManager.SubmitPlayerCountSubject.Subscribe(
            value => playerJoinManager.OnAwake(value),
            () => playerCountSelectManager.gameObject.SetActive(false)
            );

        playerCountSelectManager.SubmitPlayerCountSubject.Subscribe(value => characterSelectManager.OnAwake(value));
    }
}
