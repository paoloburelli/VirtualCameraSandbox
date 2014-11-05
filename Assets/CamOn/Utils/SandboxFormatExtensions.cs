using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CamOn.Utils {
	public static class SandboxFormatExtensions
	{
	    public static string ToSandboxString (this Vector3 v)
	    {
	    	return v.ToString ().Replace (",", "").TrimEnd (')').TrimStart ('(');
	    }
	
	    public static string ToSandboxString (this Vector2 v)
		{
			return v.ToString ("0.000").Replace (",", "").TrimEnd (')').TrimStart ('(');
		}
		
		public static string ToSandboxString (this float[] v)
		{
			string rValue = "";
			foreach (float f in v)
				rValue += f.ToString ("0.000") + " ";
			
			return rValue.TrimEnd (' ');
		}
		
		public static Vector3 ParseVector3String (string s)
		{
			string[] numbers = s.Split (' ');
			if (numbers.Length != 3)
				throw new FormatException ("invalid position");
			float[] fPosition = new float[3];
			for (int i = 0; i < 3; i++)
				if (!float.TryParse (numbers[i], out fPosition[i]))
					throw new FormatException ("invalid vector");
			
			return new Vector3 (fPosition[0], fPosition[1], fPosition[2]);
		}
		
		public static Vector2 ParseVector2String (string s)
		{
			string[] numbers = s.Split (' ');
			if (numbers.Length != 2)
				throw new FormatException ("invalid position");
			float[] fPosition = new float[2];
			for (int i = 0; i < 2; i++)
				if (!float.TryParse (numbers[i], out fPosition[i]))
					throw new FormatException ("invalid vector");
			
			return new Vector2 (fPosition[0], fPosition[1]);
		}
		
		public static float ParseFloatString (string s)
		{
			float degrees;
			if (!float.TryParse (s, out degrees))
				throw new FormatException ("invalid float");
			return degrees;
		}
	}
}
	
