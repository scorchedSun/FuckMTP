using System;

namespace FuckMTP.Core.Contracts
{
    public abstract class ProgressReporter : IDisposable
    {
        private bool disposed;
        private int numberOfCallsToStepOne;
        private readonly Progress<int> progress = new Progress<int>();
        protected readonly uint maximum = 100;

        protected ProgressReporter()
        {
            progress.ProgressChanged += HandleProgressChanged;
        }

        public void StepOne()
        {
            if (numberOfCallsToStepOne < maximum)
                (progress as IProgress<int>).Report(++numberOfCallsToStepOne);
        }

        protected abstract void HandleProgressChanged(object sender, int value);

        protected virtual void Finish() => (progress as IProgress<int>).Report(100);

        public void Dispose()
        {
            if (disposed) return;

            Finish();

            progress.ProgressChanged -= HandleProgressChanged;

            disposed = true;
        }
    }
}
