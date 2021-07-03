namespace SRTPluginProviderRE6
{
    public interface IGameMemoryRE6
    {
        // Versioninfo
        string GameName { get; }
        string VersionInfo { get; }

        // Player 1 HP
        short PlayerCurrentHealth { get; set; }
        short PlayerMaxHealth { get; set; }

        // Player 2 HP
        short PlayerCurrentHealth2 { get; set; }
        short PlayerMaxHealth2 { get; set; }
    }
}