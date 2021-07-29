using SRTPluginProviderRE6.Structs.GameStructs;
using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRE6
{
    public class GameMemoryRE6 : IGameMemoryRE6
    {
        public string GameName => "RE6";

        // Versioninfo
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // Player 1 HP
        public string PlayerName { get => _playerName; set => _playerName = value; }
        internal string _playerName;
        public GamePlayer Player { get => _player; set => _player = value; }
        internal GamePlayer _player;
        public int PlayerDA { get => _playerDA; set => _playerDA = value; }
        internal int _playerDA;

        // Player 2 HP
        public string PlayerName2 { get => _playerName2; set => _playerName2 = value; }
        internal string _playerName2;
        public GamePlayer Player2 { get => _player2; set => _player2 = value; }
        internal GamePlayer _player2;
        public int Player2DA { get => _player2DA; set => _player2DA = value; }
        internal int _player2DA;

        // DA Scores
        public GameDifficultyAdjustment RankManager { get => _rankManager; set => _rankManager = value; }
        internal GameDifficultyAdjustment _rankManager;

        // Current Level
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        internal int _currentLevel;

    }
}
