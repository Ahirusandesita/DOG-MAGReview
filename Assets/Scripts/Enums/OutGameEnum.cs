
namespace OutGameEnum
{
    public enum GameModeType
    {
        Hockey,
    }

    public enum HockeyRule
    {
        /// <summary>
        /// 時間制
        /// </summary>
        Time,
        /// <summary>
        /// 得点先取制
        /// </summary>
        Points,
    }

    public enum TeamType
    {
        Red = 0,
        Blue = 1,
        Yellow = 2,
        Green = 3,

        Null = 999,
    }
}
