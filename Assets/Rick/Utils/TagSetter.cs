using UnityEngine;
using Magicolo;

public class TagSetter : MonoBehaviour {

	public string tagToSet;
	
	[ButtonAttribute("Set tag to childs","setTag")]
	public bool setTagToogle;
	
	public void setTag(){
		foreach (var child in this.GetChildrenRecursive()) {
			child.transform.tag = tagToSet;
		}
		gameObject.transform.tag = tag;
	}
	
}
