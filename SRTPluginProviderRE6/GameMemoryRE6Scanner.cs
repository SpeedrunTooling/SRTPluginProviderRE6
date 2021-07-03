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
        private int pointerAddressHP;
        private int pointerAddressHP2;
        //private int pointerAddressDA;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }

        private MultilevelPointer PointerHP { get; set; }
        private MultilevelPointer PointerHP2 { get; set; }
        //private MultilevelPointer PointerDA { get; set; }

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
                PointerHP2 = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressHP2), 0xD0);
                //PointerDA = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressDA), 0x364);
            }
        }

        private void SelectPointerAddresses()
        {
            pointerAddressHP = 0x1464430;
            pointerAddressHP2 = 0x14645F4;
            //pointerAddressDA = 0xA3F716;
        }

        internal void UpdatePointers()
        {
            PointerHP.UpdatePointers();
            PointerHP2.UpdatePointers();
        }

        internal unsafe IGameMemoryRE6 Refresh()
        {
            bool success;

            // Player HP
            fixed (short* p = &gameMemoryValues._playerCurrentHealth)
                success = PointerHP.TryDerefShort(0xF10, p);
            
            fixed (short* p = &gameMemoryValues._playerMaxHealth)
                success = PointerHP.TryDerefShort(0xF12, p);

            // Player 2 HP
            fixed (short* p = &gameMemoryValues._playerCurrentHealth2)
                success = PointerHP2.TryDerefShort(0xF10, p);

            fixed (short* p = &gameMemoryValues._playerMaxHealth2)
                success = PointerHP2.TryDerefShort(0xF12, p);

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