﻿using FuckMTP.Core.Contracts;
using FuckMTP.DeviceConnector.Contracts;
using System;
using SharpAdbClient;
using IDevice = FuckMTP.DeviceConnector.Contracts.IDevice;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading;

namespace FuckMTP.ADB
{
    public sealed class FileHandler : IFileHandler
    {
        private const string Root = "/sdcard";

        private readonly IDevice device;
        private readonly IConfiguration configuration;
        AdbServer server;
        AdbClient client;
        DeviceData deviceData;

        private FileHandler(IDevice device, IConfiguration configuration)
        {
            this.device = device ?? throw new ArgumentNullException(nameof(device));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static RequiresConfiguration For(IDevice device) => new RequiresConfiguration(device);

        public void Copy(string filePath, string targetPath, bool overwriteExisting)
        {
            if (server is null)
            {
                server = new AdbServer();
                server.StartServer(configuration.PathToExecutable, false);
                client = new AdbClient();
                deviceData = client.GetDevices().Single(d => d.Serial.Equals(device.SerialNumber, StringComparison.InvariantCulture));               
            }

            using (SyncService syncService = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), deviceData))
            {
                /* Use for ADB device connector ;)
                var receiver = new ConsoleOutputReceiver();
                client.ExecuteRemoteCommand("ls -a /sdcard/", deviceData, receiver);
                */

                // ToDo: Temp hack, replace with actual resolving logic
                string sourcePath = filePath.Replace(@"\Phone", Root).Replace(@"\", "/");
                using (FileStream stream = File.OpenWrite(Path.Combine(targetPath, Path.GetFileName(filePath))))
                {
                    syncService.Pull(sourcePath, stream, null, CancellationToken.None);
                }
            }
        }

        public void Move(string filePath, string targetPath, bool overwriteExisting)
        {
        }

        public sealed class RequiresConfiguration
        {
            private readonly IDevice device;

            internal RequiresConfiguration(IDevice device) => this.device = device;

            public FileHandler With(IConfiguration configuration) => new FileHandler(device, configuration);
        }
    }
}