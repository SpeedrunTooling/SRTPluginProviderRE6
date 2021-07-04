using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRE6
{
    public class GameMemoryRE6 : IGameMemoryRE6
    {
        public string GameName => "RE6";

        // Versioninfo
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // Leon Stats
        public short LeonCurrentHealth { get => _leonCurrentHealth; set => _leonCurrentHealth = value; }
        internal short _leonCurrentHealth;

        public short LeonMaxHealth { get => _leonMaxHealth; set => _leonMaxHealth = value; }
        internal short _leonMaxHealth;

        public int LeonDA { get => _leonDA; set => _leonDA = value; }
        internal int _leonDA;

        // Helena Stats
        public short HelenaCurrentHealth { get => _helenaCurrentHealth; set => _helenaCurrentHealth = value; }
        internal short _helenaCurrentHealth;

        public short HelenaMaxHealth { get => _helenaMaxHealth; set => _helenaMaxHealth = value; }
        internal short _helenaMaxHealth;

        public int HelenaDA { get => _helenaDA; set => _helenaDA = value; }
        internal int _helenaDA;

        // Chris Stats
        public short ChrisCurrentHealth { get => _chrisCurrentHealth; set => _chrisCurrentHealth = value; }
        internal short _chrisCurrentHealth;

        public short ChrisMaxHealth { get => _chrisMaxHealth; set => _chrisMaxHealth = value; }
        internal short _chrisMaxHealth;

        public int ChrisDA { get => _chrisDA; set => _chrisDA = value; }
        internal int _chrisDA;

        // Piers Stats
        public short PiersCurrentHealth { get => _piersCurrentHealth; set => _piersCurrentHealth = value; }
        internal short _piersCurrentHealth;

        public short PiersMaxHealth { get => _piersMaxHealth; set => _piersMaxHealth = value; }
        internal short _piersMaxHealth;

        public int PiersDA { get => _piersDA; set => _piersDA = value; }
        internal int _piersDA;

        // Jake Stats
        public short JakeCurrentHealth { get => _jakeCurrentHealth; set => _jakeCurrentHealth = value; }
        internal short _jakeCurrentHealth;

        public short JakeMaxHealth { get => _jakeMaxHealth; set => _jakeMaxHealth = value; }
        internal short _jakeMaxHealth;

        public int JakeDA { get => _jakeDA; set => _jakeDA = value; }
        internal int _jakeDA;

        // Sherry Stats
        public short SherryCurrentHealth { get => _sherryCurrentHealth; set => _sherryCurrentHealth = value; }
        internal short _sherryCurrentHealth;

        public short SherryMaxHealth { get => _sherryMaxHealth; set => _sherryMaxHealth = value; }
        internal short _sherryMaxHealth;

        public int SherryDA { get => _sherryDA; set => _sherryDA = value; }
        internal int _sherryDA;

        // Ada Stats
        public short AdaCurrentHealth { get => _adaCurrentHealth; set => _adaCurrentHealth = value; }
        internal short _adaCurrentHealth;

        public short AdaMaxHealth { get => _adaMaxHealth; set => _adaMaxHealth = value; }
        internal short _adaMaxHealth;

        public int AdaDA { get => _adaDA; set => _adaDA = value; }
        internal int _adaDA;

        // Agent Stats
        public short AgentCurrentHealth { get => _agentCurrentHealth; set => _agentCurrentHealth = value; }
        internal short _agentCurrentHealth;

        public short AgentMaxHealth { get => _agentMaxHealth; set => _agentMaxHealth = value; }
        internal short _agentMaxHealth;

        public int AgentDA { get => _agentDA; set => _agentDA = value; }
        internal int _agentDA;

        // Current Level
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        internal int _currentLevel;

    }
}
