using UnityEngine;
using System;

namespace CamOn.Utils {
public class Random
{
	private static System.Random randomSeed = null;
	private static Random instance = null;
	private static Random Instance {
		get {
			if (instance == null) {
				instance = new Random ();
			}
		return instance;
		}
	}
	
	private float y2;
	private bool use_last = false;
	
	private Random (){}
	
	public static float UniformFloat (float min, float max)
	{
		if (randomSeed == null)
			randomSeed = new System.Random ();
		
		return (float)(randomSeed.NextDouble())*(max - min)+min;
	}
	
	public static float GaussianFloat(float m, float s){
		/* mean m, standard deviation s */
		float x1, x2, w, y1;

		if (Instance.use_last)		        /* use value from previous call */
		{
			y1 = Instance.y2;
			Instance.use_last = false;
		}
		else
		{
			do {
				x1 = (float)(2.0 * UnityEngine.Random.value - 1.0);
				x2 = (float)(2.0 * UnityEngine.Random.value - 1.0);
				w = x1 * x1 + x2 * x2;
			} while ( w >= 1.0 );
	
			w =  Mathf.Sqrt( (-2.0f * Mathf.Log( w ) ) / w );
			y1 = x1 * w;
			Instance.y2 = x2 * w;
			Instance.use_last = true;
		}

		return( m + y1 * s );
	}
	
	public static  Vector3 GaussianVector3(Vector3 m, float s)
	{
		return new Vector3 (GaussianFloat (m.x, s), GaussianFloat (m.y, s), GaussianFloat (m.z, s));
	}
	
	public static Vector3 GaussianVector3(Vector3 m, Vector3 s)
	{
		return new Vector3 (GaussianFloat (m.x, s.x), GaussianFloat (m.y, s.y), GaussianFloat (m.z, s.z));
	}
}
}