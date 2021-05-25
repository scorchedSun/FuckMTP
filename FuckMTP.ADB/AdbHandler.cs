using FileSystem;
using FluentAdb;
using FluentAdb.Enums;
using FluentAdb.Interfaces;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FuckMTP.ADB
{

    public sealed class AdbHandler : IDisposable
    {
        private static readonly object syncRoot = new object();

        private static IAdb adb;

        private bool disposed;

        public AdbHandler(IConfiguration configuration)
        {
            lock (syncRoot)
            {
                if (adb is null)
                    adb = Adb.New(configuration.PathToExecutable);
            }
        }

        public IAdb AdbInstance => adb;

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (disposed) return;

                if (adb != null)
                    Adb.Die();

                disposed = true;
            }
        }
    }
}
