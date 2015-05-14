using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FlyingGentleman.Library.FileSystem
{
	/// <summary>
	/// A wrapper around a regular expression.
	/// </summary>
	[DataContract]
	public class IgnoreExpression
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IgnoreExpression"/> class.
		/// </summary>
		public IgnoreExpression()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IgnoreExpression"/> class.
		/// </summary>
		/// <param name="expression">The regular expression.</param>
		/// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
		public IgnoreExpression(string expression, bool isCaseSensitive)
		{
			this.Expression = expression;
			this.IsCaseSensitive = isCaseSensitive;
		}

		/// <summary>
		/// The regular expression.
		/// </summary>
		[DataMember]
		public string Expression { get; set; }

		/// <summary>
		/// Whether the expression is case sensitive or not.
		/// </summary>
		[DataMember]
		public bool IsCaseSensitive { get; set; }

		private Regex _Regex = null;

		/// <summary>
		/// Converts the Ignore Expression to a Regular Expression.
		/// </summary>
		/// <returns>The regular expression.</returns>
		public Regex ToRegex()
		{
			if (_Regex == null)
			{
				if (this.IsCaseSensitive)
				{
					_Regex = new Regex(this.Expression);
				}
				else
				{
					_Regex = new Regex(this.Expression, RegexOptions.IgnoreCase);
				}
			}

			return _Regex;
		}

		/// <summary>
		/// Determines whether the specified input is a match against this expression.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		///   <c>true</c> if the specified input is match; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMatch(string input)
		{
			return this.ToRegex().IsMatch(input);
		}
	}
}
