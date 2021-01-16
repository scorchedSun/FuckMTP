using FuckMTP.Core.Contracts;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Collections.Generic;

namespace FuckMTP
{
    internal sealed class DeviceFileSource : IFileSource, IDisposable
    {
        private readonly Interactor interactor;
        private readonly IDevice device;
        private bool disposed = false;

        public DeviceFileSource(Interactor interactor, IDevice device)
        {
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
            this.device = device ?? throw new ArgumentNullException(nameof(device));
        }

        ~DeviceFileSource()
        {
            Dispose();
        }

        public IReadOnlyList<IFile> SelectFiles() => interactor.SelectFilesFrom(device);

        public void Dispose()
        {
            if (disposed) return;

            try
            {
                device?.Dispose();
            }
            catch (Exception) { }
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
