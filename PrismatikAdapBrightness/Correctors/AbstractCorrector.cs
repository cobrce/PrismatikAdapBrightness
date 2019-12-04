using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PrismatikAdapBrightness.Correctors
{
	abstract class AbstractCorrector : ICorrector
	{
		public enum CorrectorTypeEnum
		{
			Threshold,
			Smooth
		}

		protected long? LastUpdated = null;
		public abstract void Update(int value);

		protected int currentBrightness;

		public abstract event CorrectorFeedback Corrected;

		public virtual void Start(int currentBrightness)
		{
			this.currentBrightness = currentBrightness;
			LastUpdated = null;
		}

		public virtual void Stop()
		{
			LastUpdated = null;
		}

		internal static ICorrector CreateCorrector(CorrectorTypeEnum correctorType)
		{
			return correctorType switch
			{
				CorrectorTypeEnum.Smooth => new SmoothCorrector(),
				_ => new ThresholdCorrector(),
			};
		}
	}
}
