using UniRx;

public interface IScoreCountable
{
    IReadOnlyReactiveProperty<int> ScoreProperty { get; }
}
