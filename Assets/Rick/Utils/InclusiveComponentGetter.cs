using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rick{
	[System.Serializable]
	public class InclusiveComponentGetter<T>  where T : Object{
	
		public GameObject root;
		public List<T> components = new List<T>();
		
		public InclusiveComponentGetter(GameObject root = null){
			this.root = root;
		}
		
		public T[] getComponents(){
			removeDeletedComponents();
			addNewComponents();
			
			return components.ToArray();
		}

		void removeDeletedComponents() {
			List<T> toRemove = new List<T>();
			foreach (var component in components) {
				if(component == null){
					toRemove.Add(component);
				}
			}
			
			foreach (var remove in toRemove) {
				components.Remove(remove);
			}
		}

		void addNewComponents() {
			T[] foundComponents = getChildComponents(); 
			
			foreach (var found in foundComponents) {
				if(!components.Contains(found)){
					components.Add(found);
				}
			}
		}
		
		T[] getChildComponents() {
			if(root == null){
				return Object.FindObjectsOfType<T>();
			}else{
				return root.GetComponentsInChildren<T>();
			}
		}
	}
}