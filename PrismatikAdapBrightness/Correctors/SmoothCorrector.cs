using System;
using System.Collections.Generic;
using System.ComponentModel;
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
			while (working)
			{
				if (desiredBrightness == null || desiredBrightness.Value == currentBrightness) continue;

				int Delta = (desiredBrightness.Value - currentBrightness) / 3;
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
