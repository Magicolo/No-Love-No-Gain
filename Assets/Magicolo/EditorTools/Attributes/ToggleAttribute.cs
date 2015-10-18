using System;
using UnityEngine;
using Magicolo.EditorTools;

namespace Magicolo {
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ToggleAttribute : CustomAttributeBase {
	
		public GUIContent trueLabel;
		public GUIContent falseLabel;
	
		public ToggleAttribute() {
		}
	
		public ToggleAttribute(string trueLabel, string falseLabel) {
			this.trueLabel = trueLabel.ToGUIContent();
			this.falseLabel = falseLabel.ToGUIContent();
		}
	
		public ToggleAttribute(GUIContent trueLabel, GUIContent falseLabel) {
			this.trueLabel = trueLabel;
			this.falseLabel = falseLabel;
		}
	}
}