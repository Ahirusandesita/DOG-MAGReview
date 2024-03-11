using UnityEngine;
using UniRx;
using VContainer;

public class PlayerPresenter : MonoBehaviour
{
    private PlayerView playerView;
    private PlayerMove playerMove;
    private IShot shot;
    private Status status;

    [Inject]
    public void Inject(PlayerView playerView,PlayerMove playerMove,IShot shot,Status status)
    {
        this.playerView = playerView;
        this.playerMove = playerMove;
        this.shot = shot;
        this.status = status;

        Subscription();
    }

    private void Subscription()
    {
        shot.BulletCountProperty.Subscribe(numberOfBullet => playerView.Use(numberOfBullet));
        status.HpProperty.Subscribe(hp => shot.Life(hp));
        status.HpProperty.Subscribe(hp => playerMove.Life(hp));
        status.HpProperty.Subscribe(hp => playerView.Life(hp));
    }
}
