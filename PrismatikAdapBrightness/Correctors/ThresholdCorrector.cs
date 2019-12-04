using System;
using System.Collections.Generic;
using System.Text;

namespace PrismatikAdapBrightness.Correctors
{
	class ThresholdCorrector : AbstractCorrector
	{
		public ThresholdCorrector()
		{
		}

		public override event CorrectorFeedback Corrected;

		public override void Update(int value)
		{
			if (Math.Abs(value - currentBrightness) >= 10)
			{
				long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

				if (LastUpdated != null && (now - LastUpdated.Value) < 500) // value isn't stable for 500ms
				{
					LastUpdated = now;
				}
				else
				{
					currentBrightness = value;
					Corrected?.Invoke(value);
				}
			}
		}
	}
}
