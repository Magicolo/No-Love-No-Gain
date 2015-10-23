using System;
using System.Collections.Generic;
using Rick;

namespace Rick.RandomBags{
	public class RoundRobinBag<t> : Bag<t>  {

		readonly List<t> bag;
		int currentIndex = -1;
		
		bool suffleOnReset;
		bool suffleOnAdd;
		bool suffleOnEnd;
		bool resetOnAdd;
	
		public RoundRobinBag(params RoundRobinBagOptions[] options){
			bag = new List<t>();
			
			foreach (var option in options) {
				switch(option){
					case RoundRobinBagOptions.SUFFLE_ON_ADD : suffleOnAdd = true; break;
					case RoundRobinBagOptions.SUFFLE_ON_RESET : suffleOnReset = true; break;
					case RoundRobinBagOptions.SUFFLE_ON_END : suffleOnEnd = true; break;
					case RoundRobinBagOptions.RESET_ON_ADD : resetOnAdd = true; break;
				}
			}
		}
		
		
		public void add(t toAdd){
			bag.Add(toAdd);
			if(resetOnAdd) reset();
			if(suffleOnAdd) suffle();
		}
		
		public t next() {
			currentIndex = ++currentIndex;
			
			if(currentIndex >= bag.Count){
				if(suffleOnEnd) suffle();
				currentIndex = 0;
			}
				
			
			return bag[currentIndex];
		}
	
		public void reset() {
			currentIndex = -1;
			if(suffleOnReset) suffle();
		}
		
		void suffle(){
			bag.Shuffle();
		}
	}
	
	public enum RoundRobinBagOptions{SUFFLE_ON_RESET, SUFFLE_ON_ADD, SUFFLE_ON_END, RESET_ON_ADD}
}