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
        public short PlayerCurrentHealth { get => _playerCurrentHealth; set => _playerCurrentHealth = value; }
        internal short _playerCurrentHealth;

        public short PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = value; }
        internal short _playerMaxHealth;

        // Player 2 HP
        public short PlayerCurrentHealth2 { get => _playerCurrentHealth2; set => _playerCurrentHealth2 = value; }
        internal short _playerCurrentHealth2;

        public short PlayerMaxHealth2 { get => _playerMaxHealth2; set => _playerMaxHealth2 = value; }
        internal short _playerMaxHealth2;
    }
}
