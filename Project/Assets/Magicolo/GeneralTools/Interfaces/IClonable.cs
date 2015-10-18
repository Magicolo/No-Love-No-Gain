using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magicolo
{
	public interface IClonable<T>
	{
		T Clone();
	}
}
