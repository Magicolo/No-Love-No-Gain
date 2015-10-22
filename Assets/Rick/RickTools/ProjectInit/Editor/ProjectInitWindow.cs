using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

using Magicolo.EditorTools;
using RickEditor.Editor;

namespace RickTools.MapLoader{
	public class ProjectInitWindow : CustomWindowBase<ProjectInitWindow> {
	
		[MenuItem("Rick's Tools/Init/InitProject")]
		public static void Create() {
			makeFolder("GUI");
			makeFolder("Effects");
			makeFolder("Resources");
			makeFolder("Scenes");
			makeFolder("Scenes/Gym");
			makeFolder("Scenes/Game");
			makeFolder("Scripts");
			makeFolder("Scripts/Debug");
			makeFolder("Scripts/UI");
			makeFolder("Prefabs");
			makeFolder("Textures");
			
		}
		static void makeFolder(string folder) {
			Directory.CreateDirectory(Application.dataPath + "/" + folder);
		}
	}
}
	