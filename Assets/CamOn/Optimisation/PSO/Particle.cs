using System;
using UnityEngine;
using CamOn.Utils;

namespace CamOn.Optimisation.PSO
{
	public class Particle
	{
		Vector3 position,lookAtPosition;
		Vector3 positionVelocity = Vector3.zero,lookVelocity = Vector3.zero;
		ParticleSwarmOptimisation optimizer;
		float fitness = 0;
		Particle localOptimum;
		
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
		
		public Vector3 LookAt {
			get {
				return lookAtPosition;
			}
		}
		
		public Vector2 Rotation {
			get {
				return (lookAtPosition - position).GetSphericalRotation ();
			}
		}
		
		public Particle (Vector3 position, Vector2 rotation, ParticleSwarmOptimisation optimizer)
		{
			this.position = position;
			this.lookAtPosition = position + Vector3.forward.Rotate (rotation);
			this.optimizer = optimizer;
			localOptimum = this;
		}
		
		public float Evaluate ()
		{
			optimizer.tmpCamera.transform.position = position;
			optimizer.tmpCamera.transform.LookAt (lookAtPosition);
			fitness = optimizer.EvaluateCamera (optimizer.tmpCamera);
			
			if (fitness >= localOptimum.fitness)
				localOptimum = this.Clone ();
			
			return fitness;
		}
		
		public void UpdateVelocity ()
		{
			lookVelocity = optimizer.inertia * lookVelocity + optimizer.cognitiveFactor * UnityEngine.Random.value * (optimizer.GlobalOptimum.LookAt - LookAt) + optimizer.socialFactor * UnityEngine.Random.value * (localOptimum.LookAt - LookAt);
			positionVelocity = optimizer.inertia * positionVelocity + optimizer.cognitiveFactor * UnityEngine.Random.value * (optimizer.GlobalOptimum.Position - Position) + optimizer.socialFactor * UnityEngine.Random.value * (localOptimum.Position - Position);
		}
		
		public void Move ()
		{
			position += positionVelocity;
			lookAtPosition += lookVelocity;
		}
		
		public Particle Clone ()
		{
			Particle tmp = new Particle (Position, Rotation, optimizer);
			tmp.fitness = fitness;
			return tmp;
		}
		
		public static Particle Random (Vector3 min, Vector3 max, ParticleSwarmOptimisation optimizer)
		{
			Vector3 mean = (max + min) / 2;
			Vector3 size = (max - mean) / 2;
			Vector3 position = GeometryUtilityExtra.RandomValidPosition (mean, size);
			Vector2 rotation = new Vector2 (UnityEngine.Random.value * 360 - 180,
										   UnityEngine.Random.value * 180 - 90);
			return new Particle (position, rotation, optimizer);
		}
	}
}

