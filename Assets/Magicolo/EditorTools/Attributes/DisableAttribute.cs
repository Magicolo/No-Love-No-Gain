using System;
using Magicolo.EditorTools;

namespace Magicolo {
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class DisableAttribute : CustomAttributeBase {
	
		public DisableAttribute() {
			DisableOnPlay = true;
			DisableOnStop = true;
		}
	}
}