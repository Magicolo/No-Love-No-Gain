using System;
using System.Collections.Generic;
using System.Collections;

namespace Rick{
	public static class ListExtentions  {
	
		
		//Fisher-Yates shuffle
		public static void Shuffle<T>(this IList<T> list) {  
			Random rng = new Random();  
			int n = list.Count;  
			while (n > 1) {  
			    n--;  
			    int k = rng.Next(n + 1);  
			    T value = list[k];  
			    list[k] = list[n];  
			    list[n] = value;  
			}  
		}


        public static void AddArray<T>(this IList<T> list, T[] array) {
            foreach (var item in array)
            {
                list.Add(item);
            }
        }
	}
}
