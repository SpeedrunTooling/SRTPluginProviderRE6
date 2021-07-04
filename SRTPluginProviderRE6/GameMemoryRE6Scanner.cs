using ProcessMemory;
using System;
using System.Diagnostics;

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
        private int pointerAddressStats;
        private int pointerAddressCurrentLevel;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }

        private MultilevelPointer PointerHPLeon { get; set; }
        private MultilevelPointer[] PointerHP { get; set; }
        private MultilevelPointer PointerDA { get; set; }
        private MultilevelPointer PointerCurrentLevel { get; set; }

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
                var position = 0;
                if (PointerHP == null)
                {
                    PointerHP = new MultilevelPointer[8];
                }
                for (var i = 0; i < 8; i++)
                {
                    position = (i * 0x4) + 0x24;
                    PointerHP[i] = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressStats), position);
                }
                
                PointerDA = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressStats));
                PointerCurrentLevel = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressCurrentLevel));
            }
        }

        private void SelectPointerAddresses()
        {
            pointerAddressStats = 0x13C6468;
            pointerAddressCurrentLevel = 0x13C549C;
        }

        internal void UpdatePointers()
        {
            for (var i = 0; i < 8; i++)
            {
                PointerHP[i].UpdatePointers();
            }
            PointerDA.UpdatePointers();
            PointerCurrentLevel.UpdatePointers();
        }

        internal unsafe IGameMemoryRE6 Refresh()
        {
            bool success;

            // Leon
            fixed (short* p = &gameMemoryValues._leonCurrentHealth)
                success = PointerHP[0].TryDerefShort(0xF10, p);
            
            fixed (short* p = &gameMemoryValues._leonMaxHealth)
                success = PointerHP[0].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._leonDA)
                success = PointerDA.TryDerefInt(0x4120, p);

            // Helena
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[1].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[1].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._helenaDA)
                success = PointerDA.TryDerefInt(0x4124, p);

            // Chris
            fixed (short* p = &gameMemoryValues._chrisCurrentHealth)
                success = PointerHP[2].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._chrisMaxHealth)
                success = PointerHP[2].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._chrisDA)
                success = PointerDA.TryDerefInt(0x4128, p);

            // Piers
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[3].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[3].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._piersDA)
                success = PointerDA.TryDerefInt(0x412C, p);

            // Jake
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[4].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[4].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._jakeDA)
                success = PointerDA.TryDerefInt(0x4130, p);

            // Sherry
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[5].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[5].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._sherryDA)
                success = PointerDA.TryDerefInt(0x4134, p);

            // Ada
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[6].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[6].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._adaDA)
                success = PointerDA.TryDerefInt(0x4138, p);

            // Agent
            fixed (short* p = &gameMemoryValues._helenaCurrentHealth)
                success = PointerHP[7].TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._helenaMaxHealth)
                success = PointerHP[7].TryDerefShort(0xF12, p);

            fixed (int* p = &gameMemoryValues._agentDA)
                success = PointerDA.TryDerefInt(0x413C, p);

            // Current Level
            fixed (int* p = &gameMemoryValues._currentLevel)
                success = PointerCurrentLevel.TryDerefInt(0x412A4, p);

            HasScanned = true;
            return gameMemoryValues;
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