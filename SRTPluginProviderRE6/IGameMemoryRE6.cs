using SRTPluginProviderRE6.Structs.GameStructs;

namespace SRTPluginProviderRE6
{
    public interface IGameMemoryRE6
    {
        // Versioninfo
        string GameName { get; }
        string VersionInfo { get; }

        // Player 1 HP
        GamePlayer Player { get; set; }

        // Player 2 HP
        GamePlayer Player2 { get; set; }

        // DA Scores
        GameDifficultyAdjustment RankManager { get; set; }

        int CurrentLevel { get; set; }

    }
}