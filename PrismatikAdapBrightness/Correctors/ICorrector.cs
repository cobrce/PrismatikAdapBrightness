using System;
using System.Collections.Generic;
using System.Text;

namespace PrismatikAdapBrightness.Correctors
{
	public delegate void CorrectorFeedback(int brightness);
	interface ICorrector
	{
		event CorrectorFeedback Corrected;
		void Stop();
		void Start(int currentBrightness);
		void Update(int value);
	}
}
