using UnityEngine.InputSystem;

/// <summary>
/// �A�E�g�Q�[���Ŋm�肳�ꂽ�v���C���[�̏��
/// </summary>
public class PlayerInfo : IPlayerInfo
{
    public int PlayerIndex { get; private set; } = default;

    public InputDevice Device { get; private set; } = default;

    public CharacterAsset CharacterData { get; private set; } = default;

    public TeamInfo TeamInfo { get; private set; } = default;


    public PlayerInfo(int playerIndex, InputDevice device)
    {
        PlayerIndex = playerIndex;
        Device = device;
    }

    public PlayerInfo(int playerIndex, InputDevice device, CharacterAsset useCharacterData)
    {
        PlayerIndex = playerIndex;
        Device = device;
        CharacterData = useCharacterData;
    }


    public void SetCharacter(CharacterAsset useCharacterData)
    {
        CharacterData = useCharacterData;
    }

    public void SetTeamInfo(TeamInfo teamInfo)
    {
        TeamInfo = teamInfo;
    }
}
