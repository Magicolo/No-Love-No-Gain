using System.Collections.Generic;
using UnityEngine;


namespace RickTools.MapLoader{
	public class MapLoaderStatistics  {
	
		public Dictionary<string,int> warningStats = new Dictionary<string, int>();
		public Dictionary<string,int> errorStats = new Dictionary<string, int>();

		
		public void addWarning(string key){
			if(warningStats.ContainsKey(key)){
				int value = warningStats[key];
				warningStats.Remove(key);
				warningStats.Add(key,value+1);
			}else{
				warningStats.Add(key,1);
				
			}
		}

		public void addError(string key) {
			if(errorStats.ContainsKey(key)){
				int value = errorStats[key];
				errorStats.Remove(key);
				errorStats.Add(key,value+1);
			}else{
				errorStats.Add(key,1);
				
			}
		}
		public void showToConsole(){
			foreach (var element in errorStats) {
				Debug.LogError(element.Key + " : " + element.Value);
			}
			foreach (var element in warningStats) {
				Debug.LogWarning(element.Key + " : " + element.Value);
			}
		}
	}
}