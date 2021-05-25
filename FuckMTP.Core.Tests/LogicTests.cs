using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuckMTP.Core.Tests
{
    [TestClass]
    public class LogicTests
    {
        private const string SamplePath = @".\Test\";
        private const string SampleTargetPath = @".\Test\Processed";

        private readonly Mock<IInteractor> interactorMock = new Mock<IInteractor>();
        private readonly Mock<IFileSource> fileSourceMock = new Mock<IFileSource>();
        private readonly Mock<IFileHandler> fileHandlerMock = new Mock<IFileHandler>();
        private readonly Mock<IPathHandler> pathHandlerMock = new Mock<IPathHandler>();
        private readonly Mock<IFile> fileMock = new Mock<IFile>();
        private readonly Mock<IOperationConfiguration> operationConfigurationMock = new Mock<IOperationConfiguration>();
        private Logic logic;

        [TestInitialize]
        public void Init()
        {
            fileMock.Setup(file => file.Path).Returns(SamplePath);

            fileSourceMock.Setup(fileSource => fileSource.SelectFiles()).Returns(new List<IFile> { fileMock.Object }.AsReadOnly());

            operationConfigurationMock.Setup(operationConfiguration => operationConfiguration.Mode).Returns(Mode.Copy);

            interactorMock
                .Setup(interactor => interactor.SelectTargetPath())
                .Returns(Path.GetFullPath(SampleTargetPath));
            interactorMock
                .Setup(interactor => interactor.GetOperationConfiguration())
                .Returns(operationConfigurationMock.Object);
            interactorMock
                .Setup(interactor => interactor.RunWithProgressReport(It.IsAny<int>(), It.IsAny<Action<ProgressReporter>>()))
                .Callback((int numberOfEntries, Action<ProgressReporter> action) =>
                {
                    using (ProgressReporter progressReporter = new TestableProgressReporter(numberOfEntries))
                        action(progressReporter);
                });

            logic = new Logic(interactorMock.Object, fileSourceMock.Object, fileHandlerMock.Object, pathHandlerMock.Object);
        }

        #region Functional tests

        [TestMethod]
        public void Run_Copy_Succeeds()
        {
            using (AutoCleanupFile file = new AutoCleanupFile(Path.GetFullPath(SamplePath), Guid.NewGuid().ToString()))
            {
                fileSourceMock.Setup(fileSource => fileSource.SelectFiles()).Returns(new List<IFile> { file }.AsReadOnly());
                fileHandlerMock.Setup(fileHandler => fileHandler.CopyAsync(It.IsAny<string>(), It.IsAny<string>())).Callback((string sourcePath, string targetPath) => System.IO.File.Copy(sourcePath, targetPath));

                logic.Run();

                Assert.IsTrue(System.IO.File.Exists(Path.Combine(SampleTargetPath, file.Name)));
            }
        }

        [TestMethod]
        public void Run_Move_Succeeds()
        {
            using (AutoCleanupFile file = new AutoCleanupFile(Path.GetFullPath(SamplePath), Guid.NewGuid().ToString()))
            {
                operationConfigurationMock.Setup(operationConfiguration => operationConfiguration.Mode).Returns(Mode.Move);
                fileSourceMock.Setup(fileSource => fileSource.SelectFiles()).Returns(new List<IFile> { file }.AsReadOnly());
                fileHandlerMock.Setup(fileHandler => fileHandler.MoveAsync(It.IsAny<string>(), It.IsAny<string>())).Callback((string sourcePath, string targetPath) => System.IO.File.Move(sourcePath, targetPath));

                logic.Run();

                Assert.IsTrue(System.IO.File.Exists(Path.Combine(SampleTargetPath, file.Name)));
                Assert.IsFalse(System.IO.File.Exists(file.Path));
            }
        }

        #endregion

        #region Technical tests

        [TestMethod]
        public void Run_Copy_CallsFileHandlerAndReportsSuccess()
        {
            logic.Run();

            fileHandlerMock.Verify(fileHandler => fileHandler.CopyAsync(It.IsAny<string>(), It.IsAny<string>()));
            interactorMock.Verify(interactor => interactor.ReportSuccess());
            Assert.IsTrue(TestableProgressReporter.NumberOfTimesTheProgressChanged > 0);
        }

        [TestMethod]
        public void Run_Move_CallsFileHandlerAndReportsSuccess()
        {
            operationConfigurationMock.Setup(operationConfiguration => operationConfiguration.Mode).Returns(Mode.Move);

            logic.Run();

            fileHandlerMock.Verify(fileHandler => fileHandler.MoveAsync(It.IsAny<string>(), It.IsAny<string>()));
            interactorMock.Verify(interactor => interactor.ReportSuccess());
            Assert.IsTrue(TestableProgressReporter.NumberOfTimesTheProgressChanged > 0);
        }

        [TestMethod]
        public void Run_IfNoFilesSelected_ReportsAndQuits()
        {
            fileSourceMock.Setup(fileSource => fileSource.SelectFiles()).Returns(Enumerable.Empty<IFile>().ToList().AsReadOnly());

            logic.Run();

            interactorMock.Verify(interactor => interactor.NotifyNoFilesSelected(), Times.Once());
            interactorMock.Verify(interactor => interactor.SelectTargetPath(), Times.Never());
        }

        [TestMethod]
        public void Run_IfSelectedTargetIsNull_ReportsAndQuits()
        {
            interactorMock.Setup(interactor => interactor.SelectTargetPath()).Returns(null as string);

            logic.Run();

            interactorMock.Verify(interactor => interactor.NotifyNoTargetPathSelected(), Times.Once());
            interactorMock.Verify(interactor => interactor.GetOperationConfiguration(), Times.Never());
        }

        [TestMethod]
        public void Run_IfSelectedTargetIsEmptyString_ReportsAndQuits()
        {
            interactorMock.Setup(interactor => interactor.SelectTargetPath()).Returns(string.Empty);

            logic.Run();

            interactorMock.Verify(interactor => interactor.NotifyNoTargetPathSelected(), Times.Once());
            interactorMock.Verify(interactor => interactor.GetOperationConfiguration(), Times.Never());
        }

        [TestMethod]
        public void Run_IfSelectedTargetIsWhitespaces_ReportsAndQuits()
        {
            interactorMock.Setup(interactor => interactor.SelectTargetPath()).Returns("   ");

            logic.Run();

            interactorMock.Verify(interactor => interactor.NotifyNoTargetPathSelected(), Times.Once());
            interactorMock.Verify(interactor => interactor.GetOperationConfiguration(), Times.Never());
        }

        [TestMethod]
        public void Run_IfOperationConfigurationIsNull_ReportsAndQuits()
        {
            interactorMock.Setup(interactor => interactor.GetOperationConfiguration()).Returns(null as IOperationConfiguration);

            logic.Run();

            interactorMock.Verify(interactor => interactor.NotifyNoOperationConfigurationProvided(), Times.Once());
            fileHandlerMock.Verify(fileHandler => fileHandler.CopyAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            fileHandlerMock.Verify(fileHandler => fileHandler.MoveAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        #endregion

        private sealed class TestableProgressReporter : ProgressReporter
        {
            public TestableProgressReporter(int maximum) : base(maximum)
            {
            }

            public static int NumberOfTimesTheProgressChanged { get; private set; }


            protected override void HandleProgressChanged(object sender, int value)
            {
                NumberOfTimesTheProgressChanged++;
            }
        }

        private sealed class AutoCleanupFile : IFile, IDisposable
        {
            private bool disposed;

            public string Name { get; }

            public string Path { get; }

            public AutoCleanupFile(string directoryPath, string name)
            {
                string path = System.IO.Path.Combine(directoryPath, name);
                System.IO.File.CreateText(path).Close();

                Name = name;
                Path = path;
            }

            public void Dispose()
            {
                if (disposed) return;

                if (System.IO.File.Exists(Path))
                    System.IO.File.Delete(Path);

                string targetFilePath = System.IO.Path.Combine(System.IO.Path.GetFullPath(SampleTargetPath), Name);
                if (System.IO.File.Exists(targetFilePath))
                    System.IO.File.Delete(targetFilePath);

                disposed = true;
            }
        }
    }
}
