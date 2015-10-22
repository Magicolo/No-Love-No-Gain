using System;
using System.Collections.Generic;
using System.Linq;

namespace Rick.RandomBags{
	public class WeightedBag<T> : Bag<T> {
	
		Random random;
		
		float totalWeight;
		Dictionary<float, T> bag;
	
		public WeightedBag(Random random){
			this.random = random;
			bag = new Dictionary<float, T>();
		}
		
		public void add(float weight, T toAdd){
			bag.Add(totalWeight + weight, toAdd);
			totalWeight += weight;
		}
		
		public T next() {
			float randomNumber = (float)(random.NextDouble() * totalWeight);
			foreach (var key in bag.Keys) {
				if(randomNumber < key){
					return bag[key];
				}
			}
			UnityEngine.Debug.LogError("WeightedBag pas trouvé de chiffre... BUG!? !");
			return bag.First().Value;
		}
	
		public void reset() {
			
		}
	}
}
