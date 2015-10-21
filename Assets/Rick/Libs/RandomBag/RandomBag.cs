using System;
using System.Collections.Generic;

namespace Rick.RandomBags{
	public class RandomBag<t> : Bag<t> {
	
		Random random;
		
		List<t> bag;
	
		public RandomBag(Random random){
			this.random = random;
			bag = new List<t>();
		}
		
		
		public void add(t toAdd){
			bag.Add(toAdd);
		}
		
		public t next() {
			int randomIndex = (int)(random.NextDouble() * bag.Count);
			return bag[randomIndex];
		}
	
		public void reset() {
			
		}
	}
}
