using UnityEngine;
using System.Collections;

namespace CamOn.Utils {
	public static class MathfExtra
	{
		
		public static float TanH (float x)
		{
			return MathfExtra.TanH (x, 1);
		}
		
		public static float TanH (float x, float a)
		{
			return (Mathf.Exp (a * x) - 1) / (Mathf.Exp (a * x) + 1);
		}
		
		public static float Sigmoid (float x)
		{
			return MathfExtra.Sigmoid (x, 1);
		}
		
		public static float Sigmoid (float x, float a)
		{
			return 1f / (1 + Mathf.Exp (-a * x));
		}					
	}
}