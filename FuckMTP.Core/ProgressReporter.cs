using System;

namespace FuckMTP.Core
{
    public abstract class ProgressReporter : IDisposable
    {
        private bool disposed;
        private int numberOfCallsToStepOne;
        private readonly Progress<int> progress = new Progress<int>();
        protected readonly int maximum = 100;

        protected ProgressReporter(int maximum)
        {
            if (maximum <= 0) throw new ArgumentOutOfRangeException("The given maximum value needs to be larger than 1.");

            this.maximum = maximum;
            progress.ProgressChanged += HandleProgressChanged;
        }

        public void StepOne()
        {
            if (numberOfCallsToStepOne < maximum)
                (progress as IProgress<int>).Report(++numberOfCallsToStepOne);
        }

        protected abstract void HandleProgressChanged(object sender, int value);

        protected virtual void Finish() => (progress as IProgress<int>).Report(maximum);

        public void Dispose()
        {
            if (disposed) return;

            Finish();

            progress.ProgressChanged -= HandleProgressChanged;

            disposed = true;
        }
    }
}
