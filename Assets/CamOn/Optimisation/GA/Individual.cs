using System;
using UnityEngine;
using CamOn.Utils;

namespace CamOn.Optimisation.GA
{
	public class Individual
	{
		Vector3 position;
		Vector2 rotation;
		CameraOptimiser optimizer;
		internal float fitness = 0;
		Individual localOptimum;
		
		Vector3 min,max;
		
		public float Fitness {
			get {
				return fitness;	
			}
		}
		
		public Vector3 Position {
			get {
				return position;
			}
		}
		
		
		public Vector2 Rotation {
			get {
				return rotation;
			}
		}
		
		protected Individual (Vector3 position, Vector2 rotation, CameraOptimiser optimizer)
		{
			this.position = position;
			this.rotation = rotation;
			this.optimizer = optimizer;
			localOptimum = this;
		}
		
		public float Evaluate ()
		{
			optimizer.tmpCamera.transform.position = position;
			optimizer.tmpCamera.transform.SetSphericalRotation (rotation);
			fitness = optimizer.EvaluateCamera (optimizer.tmpCamera);
			
			if (fitness >= localOptimum.fitness)
				localOptimum = this.Clone ();
			
			return fitness;
		}
		
		public Individual Clone ()
		{
			Individual tmp = new Individual (Position, Rotation, optimizer);
			tmp.fitness = fitness;
			tmp.min = this.min;
			tmp.max = this.max;
			return tmp;
		}
		
		public static Individual Random (Vector3 min, Vector3 max, CameraOptimiser optimizer)
		{
			Vector3 mean = (max + min) / 2;
			Vector3 size = (max - mean) / 2;
			Vector3 position = GeometryUtilityExtra.RandomValidPosition (mean, size);
			Vector2 rotation = new Vector2 (UnityEngine.Random.value * 360 - 180,
										   UnityEngine.Random.value * 180 - 90);
			Individual i = new Individual (position, rotation, optimizer);
			i.min = min;
			i.max = max;
			return i;
		}
		
		public float this [int i] {
			get {
				if (i < 3)
					return position [i];
				else
					return rotation [i - 3];
			}
			
			set {
				if (i < 3)
					position [i] = value;
				else
					rotation [i - 3] = value;
			}
		}
		
		public static int Compare (Individual a, Individual b)
		{
			return a.Fitness.CompareTo (b.Fitness);
		}
		
		public static Individual UniformCrossover (Individual a, Individual b, float probability)
		{
			Individual n = a.Clone ();
			for (int i = 0; i<5; i++)
				if (UnityEngine.Random.value < probability)
					n [i] = a [i];
				else
					n [i] = b [i];
			return n;
		}
		
		public static Individual LinearCrossover (Individual a, Individual b, float probability)
		{
			Individual n = a.Clone ();
			if (UnityEngine.Random.value < probability)
				for (int i = 0; i<5; i++)
					n [i] = (a [i] + b [i]) / 2;
			else
				for (int i = 0; i<5; i++)
					n [i] = a [i];
			return n;
		}
		
		public void Mutate (float probability)
		{
			for (int i = 0; i<5; i++)
				if (UnityEngine.Random.value < probability)
					this [i] += (UnityEngine.Random.value - 0.5f) * 0.01f * (max-min).magnitude;		
		}
	}
}

