using UnityEngine.InputSystem;

public interface IPlayerInfo
{
    public int PlayerIndex { get; }

    public InputDevice Device { get; }

    public CharacterAsset CharacterData { get; }

    public TeamInfo TeamInfo { get; }

}
