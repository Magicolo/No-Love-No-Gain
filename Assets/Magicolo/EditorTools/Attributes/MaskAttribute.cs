using System;
using Magicolo.EditorTools;

namespace Magicolo {
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MaskAttribute : CustomAttributeBase {

		public int filter = -1;
	
		public MaskAttribute() {
		}
	
		public MaskAttribute(object filter) {
			this.filter = (int)filter;
		}
	}
}