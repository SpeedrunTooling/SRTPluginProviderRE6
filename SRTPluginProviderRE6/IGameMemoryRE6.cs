namespace SRTPluginProviderRE6
{
    public interface IGameMemoryRE6
    {
        // Versioninfo
        string GameName { get; }
        string VersionInfo { get; }

        // Leon Stats
        short LeonCurrentHealth { get; set; }
        short LeonMaxHealth { get; set; }
        int LeonDA { get; set; }

        // Helena Stats
        short HelenaCurrentHealth { get; set; }
        short HelenaMaxHealth { get; set; }
        int HelenaDA { get; set; }

        // Chris Stats
        short ChrisCurrentHealth { get; set; }
        short ChrisMaxHealth { get; set; }
        int ChrisDA { get; set; }

        // Piers Stats
        short PiersCurrentHealth { get; set; }
        short PiersMaxHealth { get; set; }
        int PiersDA { get; set; }

        // Jake Stats
        short JakeCurrentHealth { get; set; }
        short JakeMaxHealth { get; set; }
        int JakeDA { get; set; }

        // Sherry Stats
        short SherryCurrentHealth { get; set; }
        short SherryMaxHealth { get; set; }
        int SherryDA { get; set; }

        // Ada Stats
        short AdaCurrentHealth { get; set; }
        short AdaMaxHealth { get; set; }
        int AdaDA { get; set; }

        // Agent Stats
        short AgentCurrentHealth { get; set; }
        short AgentMaxHealth { get; set; }
        int AgentDA { get; set; }

        int CurrentLevel { get; set; }

    }
}