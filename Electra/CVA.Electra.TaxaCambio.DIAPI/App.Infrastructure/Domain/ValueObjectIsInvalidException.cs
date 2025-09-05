using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Infrastructure.Domain
{
	public class ValueObjectIsInvalidException : Exception
	{
		public ValueObjectIsInvalidException(string message)
			: base(message)
		{}
	}
}
