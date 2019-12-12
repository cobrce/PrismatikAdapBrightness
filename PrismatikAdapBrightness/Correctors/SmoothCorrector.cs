using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace PrismatikAdapBrightness.Correctors
{
	class SmoothCorrector : AbstractCorrector
	{
		public override event CorrectorFeedback Corrected;

		BackgroundWorker worker = new BackgroundWorker();

		bool working;

		public override void Start(int currentBrightness)
		{
			base.Start(currentBrightness);
			working = true;
			worker.DoWork += AsyncCorrection;
			worker.RunWorkerAsync();
		}

		private void AsyncCorrection(object sender, DoWorkEventArgs e)
		{
			const int nSamples = 10;
			Queue<int> queue = new Queue<int>();
			int? meanDesiredBrightness = null;
			while (working)
			{
				if (desiredBrightness == null || desiredBrightness.Value == currentBrightness) continue;
				if (meanDesiredBrightness == null)
					meanDesiredBrightness = desiredBrightness;

				while (queue.Count < nSamples)
					queue.Enqueue(desiredBrightness.Value);

				queue.Enqueue(desiredBrightness.Value);

				while (queue.Count > nSamples)
					queue.TryDequeue(out int _);

				// calcualte the mean of signal to filter fast changes
				int newMeanDesiredBrightness = Enumerable.Sum(queue) / nSamples;

				// filter small changes
				if (Math.Abs(newMeanDesiredBrightness - meanDesiredBrightness.Value) > 3)
					meanDesiredBrightness = newMeanDesiredBrightness;

				// smooth update of current brightness
				int Delta = (meanDesiredBrightness.Value - currentBrightness) / 3;
				if (Delta == 0)
					continue;

				currentBrightness += Delta;

				Corrected?.Invoke(currentBrightness);
				Thread.Sleep(50);
			}
		}

		public override void Stop()
		{
			base.Stop();
			working = false;
		}

		private int? desiredBrightness = null;

		public override void Update(int value)
		{
			desiredBrightness = value;
		}

	}
}
