using ProcessMemory;
using SRTPluginProviderRE6.Structs.GameStructs;
using System;
using System.Diagnostics;
using System.Linq;

namespace SRTPluginProviderRE6
{
    internal unsafe class GameMemoryRE6Scanner : IDisposable
    {

        // Variables
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryRE6 gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private int pointerAddressHP;
        private int pointerAddressHP2;
        private int pointerAddressDA;
        private int pointerAddressCurrentLevel;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }

        private MultilevelPointer PointerHPLeon { get; set; }
        private MultilevelPointer PointerHP { get; set; }
        private MultilevelPointer PointerHP2 { get; set; }
        private MultilevelPointer PointerDA { get; set; }
        private MultilevelPointer PointerCurrentLevel { get; set; }

        private GamePlayer NewPlayerObj = new GamePlayer();

        public static Campaign CampaignID = Campaign.Leon;

        public enum Campaign
        {
            Leon,
            Chris,
            Jake,
            Ada
        }

        internal GameMemoryRE6Scanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryRE6();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            SelectPointerAddresses();

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_32BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.
                //POINTERS
                PointerHP = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressHP), 0x364);
                PointerHP2 = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressHP2), 0xD20);

                PointerDA = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressDA), 0x0);
                PointerCurrentLevel = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressCurrentLevel));
            }
        }

        private void SelectPointerAddresses()
        {
            pointerAddressHP = 0x1464430;
            pointerAddressHP2 = 0x13B7384;
            pointerAddressDA = 0x1427F64;
            pointerAddressCurrentLevel = 0x13C549C;
        }

        internal void UpdatePointers()
        {
            PointerHP.UpdatePointers();
            PointerHP2.UpdatePointers();
            PointerDA.UpdatePointers();
            PointerCurrentLevel.UpdatePointers();
        }

        internal unsafe IGameMemoryRE6 Refresh()
        {
            // Current Level ID
            gameMemoryValues._currentLevel = PointerCurrentLevel.DerefInt(0x412A4);
            SetCampaignID();

            // Player Names
            gameMemoryValues._playerName = GetName()[0];
            gameMemoryValues._playerName2 = GetName()[1];

            // Player 1 HP
            gameMemoryValues._player = PointerHP.Deref<GamePlayer>(0xF10);

            // Player 2 HP
            gameMemoryValues._player2 = PointerHP.Address != PointerHP2.Address ? PointerHP2.Deref<GamePlayer>(0xF10) : NewPlayerObj;

            // DA Scores
            gameMemoryValues._rankManager = PointerDA.Deref<GameDifficultyAdjustment>(0x4120);

            int[] DA = { 
                gameMemoryValues._rankManager.Leon, 
                gameMemoryValues._rankManager.Helena, 
                gameMemoryValues._rankManager.Chris, 
                gameMemoryValues._rankManager.Piers, 
                gameMemoryValues._rankManager.Jake,
                gameMemoryValues._rankManager.Sherry,
                gameMemoryValues._rankManager.Ada,
                gameMemoryValues._rankManager.Agent 
            };

            gameMemoryValues._playerDA = GetDA(DA)[0];
            gameMemoryValues._player2DA = GetDA(DA)[1];

            HasScanned = true;
            return gameMemoryValues;
        }

        private void SetCampaignID()
        {
            if (gameMemoryValues.CurrentLevel == 1100) CampaignID = Campaign.Leon;
            else if (gameMemoryValues.CurrentLevel == 1110) CampaignID = Campaign.Chris;
            else if (gameMemoryValues.CurrentLevel == 1101) CampaignID = Campaign.Jake;
            else if (gameMemoryValues.CurrentLevel == 1130) CampaignID = Campaign.Ada;
        }

        public static ArraySegment<int> GetDA(int[] DAList)
        {
            switch (CampaignID)
            {
                case Campaign.Chris:
                    return new ArraySegment<int>(DAList, 6, 2);
                case Campaign.Jake:
                    return new ArraySegment<int>(DAList, 4, 2);
                case Campaign.Ada:
                    return new ArraySegment<int>(DAList, 2, 2);
                default:
                    return new ArraySegment<int>(DAList, 0, 2);
            }
        }

        public static ArraySegment<string> GetName()
        {
            string[] names = { "Leon: ", "Helena: ", "Chris: ", "Piers: ", "Jake: ", "Sherry: ", "Ada: ", "Agent: "};
            switch (CampaignID)
            {
                case Campaign.Chris:
                    return new ArraySegment<string>(names, 6, 2);
                case Campaign.Jake:
                    return new ArraySegment<string>(names, 4, 2);
                case Campaign.Ada:
                    return new ArraySegment<string>(names, 2, 2);
                default:
                    return new ArraySegment<string>(names, 0, 2);
            }
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls


        private unsafe bool SafeReadByteArray(IntPtr address, int size, out byte[] readBytes)
        {
            readBytes = new byte[size];
            fixed (byte* p = readBytes)
            {
                return memoryAccess.TryGetByteArrayAt(address, size, p);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}