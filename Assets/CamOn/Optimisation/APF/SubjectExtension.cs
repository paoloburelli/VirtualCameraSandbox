using System;
using CamOn.Properties;

namespace CamOn.Optimisation.APF
{
	public static class SubjectExtension
	{
		public static bool ShoulBeVisible (this SubjectBackend s)
		{
			foreach (Property p in s)
				if (p is Properties.Visibility)
					return ((Visibility)p).DesiredVisibility > 0;
			return false;
		}
		
		public static float VisibilitySatisfaction (this SubjectBackend s)
		{
			foreach (Property p in s)
				if (p is Properties.Visibility)
					return ((Visibility)p).Satisfaction;
			return 1;
		}
	}
}

