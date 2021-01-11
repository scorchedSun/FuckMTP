using FuckMTP.Core;
using FuckMTP.Core.Contracts;
using FuckMTP.DeviceConnector.Contracts;
using FuckMTP.MTPDeviceConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FuckMTP
{

    /*
     * Current state of affairs:
     * 
     * Holy fucking shit, I really fucking hate MTP. It seems like Microsoft removed the "SendTo" context menu
     * option for MTP devices. Which means my first point of attack is gone. "Ok, well I'm just gonna use drag
     * 'n drop, that should also be pretty easy to understand for users." or so I thought. Turns out dragging
     * and dropping files from an MTP device directly into a WPF windows isn't working. I always get <null> 
     * when trying to access the paths.
     * So what options do I have?
     *  - Use ADB directly, get all files and let the user select the desired files
     *  - Use MTP library to be able to access MTP devices and see where that leads me
     * 
     */

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

    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int showState);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [STAThread]
        static void Main(string[] args)
        {
            HideConsole();

            Interactor interactor = new Interactor();
            try
            {
                using (IDevice device = interactor.SelectDeviceFrom(new DeviceSource()))
                using(DeviceFileSource fileSource = new DeviceFileSource(interactor, device))
                {
                    Logic logic = new Logic(interactor, fileSource);
                    logic.Run();
                }
            }
            catch (NoDeviceConnectedException)
            {
                interactor.NotifyNoDeviceConnected();
            }
            catch (NoDeviceSelectedException)
            {
                interactor.NotifyNoDeviceSelected();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected exception occurred: {ex.Message}");
            }
        }

        private static void HideConsole()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 0);
        }
    }
}
