using System;
using UnityEngine;
using CamOn.Properties;
using System.Collections.Generic;

namespace CamOn.Optimisation.PSO
{
	public class ParticleSwarmOptimisation : CameraOptimiser
	{
		public float inertia;
		public float cognitiveFactor;
		public float socialFactor;
		public int populationSize;
		Particle globalOptimum;
		public Particle GlobalOptimum {
			get { 
				return globalOptimum; 
			}
		}
		List<Particle> particles;
		IEnumerator<Particle> enumerator=null;
		public Particle Current {
			get {
				return enumerator.Current;
			}
		}

		protected override void init ()
		{

		}
		
		
		internal override void loop ()
		{
			if (enumerator == null) {
				enumerator = particles.GetEnumerator ();
				globalOptimum = particles[0].Clone ();
			}
			
			if (!enumerator.MoveNext ()) {
				enumerator = particles.GetEnumerator ();
				enumerator.MoveNext ();
			}

			enumerator.Current.Evaluate ();
			enumerator.Current.Move ();
			enumerator.Current.Evaluate ();
			enumerator.Current.UpdateVelocity ();
			

			globalOptimum.Evaluate();
			if (enumerator.Current.Fitness > globalOptimum.Fitness) {
				globalOptimum = enumerator.Current.Clone ();
				currentBest.transform.position = globalOptimum.Position;
				currentBest.transform.LookAt (GlobalOptimum.LookAt);
			}
			currentBestFitness = globalOptimum.Fitness;
		}
		

		public ParticleSwarmOptimisation (float inertia, float cognitiveFactor, float socialFactor, int popSize, Vector3 min, Vector3 max, Controller co) : base(co)
		{
			this.inertia = inertia;
			this.socialFactor = socialFactor;
			this.cognitiveFactor = cognitiveFactor;
			this.populationSize = popSize;
			particles = new List<Particle> ();
			for (int i=0;i<popSize;i++)
				particles.Add(Particle.Random(min,max,this));
		}
	}
}

