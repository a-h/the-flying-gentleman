using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace FlyingGentleman.Library.ExtensionMethods
{
	/// <summary>
	/// Provides extension methods to assist with JSON serialization of objects.
	/// </summary>
	public static class JsonExtensions
	{
		/// <summary>
		/// Returns a JSON string from the object.
		/// </summary>
		/// <param name="o">The object to serialize.</param>
		/// <returns></returns>
		public static string ToJson(this object o)
		{
			var serializer = new DataContractJsonSerializer(o.GetType());
			var ms = new MemoryStream();

			if (o != null)
			{
				serializer.WriteObject(ms, o);
			}

			return UnicodeEncoding.UTF8.GetString(ms.ToArray());
		}
	}
}
