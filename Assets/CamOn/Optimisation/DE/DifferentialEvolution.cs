using System;
using UnityEngine;
using CamOn.Properties;
using System.Collections.Generic;
using CamOn.Utils;
using CamOn.Optimisation.GA;

namespace CamOn.Optimisation.DE
{
	public class DifferentialEvolution : CameraOptimiser
	{
		public float crossoverProbability, weightingFactor;
		public int populationSize;
		Individual globalOptimum;
		public Individual GlobalOptimum {
			get { 
				return globalOptimum; 
			}
		}
		List<Individual> population;
		public Individual Current {
			get {
				return population[currentIndex];
			}
			
			protected set {
				population[currentIndex] = value;
			}
		}
		int currentIndex = 0;
		
		protected override void init ()
		{
			globalOptimum = Current.Clone ();
		}
		
		
		internal override void loop ()
		{
			List<int> indexes = new List<int> (4);
			indexes.Add (currentIndex);
			for (int j = 0; j < 3; j++) {
				int temp = (int)Mathf.Floor (UnityEngine.Random.value * (population.Count - 1));
				while (indexes.Contains (temp))
					temp = (int)Mathf.Floor (UnityEngine.Random.value * (population.Count - 1));
				indexes.Add (temp);
			}
			int R = (int)Mathf.Floor (UnityEngine.Random.value * 5);
			Individual newInd = Current.Clone ();
			for (int j = 0; j < 5; j++) {
				float r = UnityEngine.Random.value;
				if (j == R || r < crossoverProbability)
					newInd[j] = population[indexes[1]][j] + weightingFactor * (population[indexes[2]][j] - population[indexes[3]][j]);
			}

			Current.Evaluate();
			newInd.Evaluate ();
			if (newInd.Fitness > Current.Fitness)
				Current = newInd;
			

			globalOptimum.Evaluate();
			if (Current.Fitness > globalOptimum.Fitness) {
				globalOptimum = Current.Clone ();
				currentBest.transform.position = globalOptimum.Position;
				currentBest.transform.SetSphericalRotation (globalOptimum.Rotation);
			}
			currentBestFitness = globalOptimum.Fitness;
			
			currentIndex++;
			if (currentIndex >= population.Count)
				currentIndex = 0;
		}
		

		public DifferentialEvolution (float crossoverProbability, float weightingFactor, int popSize, Vector3 min, Vector3 max, Controller co) : base(co)
		{
			this.crossoverProbability = crossoverProbability;
			this.weightingFactor = weightingFactor;
			this.populationSize = popSize;
			population = new List<Individual> ();
			for (int i=0;i<popSize;i++)
				population.Add(Individual.Random(min,max,this));
		}
	}
}

