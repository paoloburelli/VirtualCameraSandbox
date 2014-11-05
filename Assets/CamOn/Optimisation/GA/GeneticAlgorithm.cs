using System;
using UnityEngine;
using CamOn.Properties;
using System.Collections.Generic;
using CamOn.Utils;
using System.Linq;

namespace CamOn.Optimisation.GA
{
	public class GeneticAlgorithm : CameraOptimiser
	{
		public float crossoverProbability, mutationProbability;
		public int populationSize;
		Individual globalOptimum;
		public Individual GlobalOptimum {
			get { 
				return globalOptimum; 
			}
		}
		List<Individual> population;
		int currentIndex = 0;
		
		protected override void init ()
		{
			globalOptimum = population.First ();
		}
		
		
		public KeyValuePair<Individual,Individual> CurrentOffsprings {
			get {
				return new KeyValuePair<Individual, Individual> (population [currentIndex], population [currentIndex+1]);
			}
		}
		
		internal override void loop ()
		{
			Individual offa = Individual.UniformCrossover (population [currentIndex], population [currentIndex + 1], crossoverProbability);
			Individual offb = Individual.UniformCrossover (population [currentIndex + 1], population [currentIndex], crossoverProbability);
			
			offa.Mutate (mutationProbability);
			offb.Mutate (mutationProbability);
			
			offa.Evaluate ();
			offb.Evaluate ();



			population [currentIndex] = offa;
			population [currentIndex + 1] = offb;
			
			Individual newCandidate = offa.Fitness > offb.Fitness ? offa : offb;
			globalOptimum.Evaluate();
			if (newCandidate.Fitness > globalOptimum.Fitness) {
				globalOptimum = newCandidate.Clone ();
				currentBest.transform.position = globalOptimum.Position;
				currentBest.transform.SetSphericalRotation (globalOptimum.Rotation);
			}
			currentBestFitness = globalOptimum.Fitness;
			
			currentIndex += 2;
			if (currentIndex >= population.Count) {
				currentIndex = 0;
				population.Sort (Individual.Compare);
			}
		}
		

		public GeneticAlgorithm (float crossoverProbability, float mutationProbability, int popSize, Vector3 min, Vector3 max, Controller co) : base(co)
		{
			this.crossoverProbability = crossoverProbability;
			this.mutationProbability = mutationProbability;
			this.populationSize = popSize;
			population = new List<Individual> ();
			for (int i=0; i<popSize; i++) {
				population.Add (Individual.Random (min, max, this));
			}
		}
	}
}

