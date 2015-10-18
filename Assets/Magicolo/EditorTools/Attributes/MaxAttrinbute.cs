using System;
using Magicolo.EditorTools;

namespace Magicolo {
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MaxAttribute : CustomAttributeBase {
	
		public float max = 1;
	
		public MaxAttribute() {
		}
	
		public MaxAttribute(float max) {
			this.max = max;
		}
	}
}