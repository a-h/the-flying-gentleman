using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Runtime.Serialization;
using System.Reflection;

namespace FlyingGentleman.Library.InstallationReport
{
	/// <summary>
	/// The Report Builder builds a HTML report of the installation process.
	/// </summary>
	public class ReportBuilder
	{
		/// <summary>
		/// Creates the HTML report of the installation.
		/// </summary>
		/// <param name="package">The package which was installed.</param>
		/// <returns></returns>
		public string CreateReport(SystemPackage package)
		{
			XDocument sourceData = SerializePackage(package);

			sourceData = StripNamespaces(sourceData);

			XslCompiledTransform template = LoadTemplate(new CultureInfo("en-GB"), "ReportTemplate");
			using (MemoryStream ms = new MemoryStream())
			{
				XmlWriterSettings settings = new XmlWriterSettings()
				{
					ConformanceLevel = ConformanceLevel.Auto,
					Encoding = new UTF8Encoding(false)
				};

				using (XmlWriter results = XmlWriter.Create(ms, settings))
				{
					using (XmlReader content = sourceData.CreateReader())
					{
						template.Transform(content, results);
					}
				}

				ms.Position = 0;

				return UnicodeEncoding.UTF8.GetString(ms.ToArray());
			}
		}

		/// <summary>
		/// Loads an embedded resource XSLT template from the assembly.
		/// </summary>
		/// <param name="displayCulture">The culture</param>
		/// <param name="name">The name of the template.</param>
		/// <returns>A transform to use to transform the report XML into HTML.</returns>
		private static XslCompiledTransform LoadTemplate(CultureInfo displayCulture, string name)
		{
			XslCompiledTransform returnValue = new XslCompiledTransform();

			string resourceName = "FlyingGentleman.Library.InstallationReport.Templates." + displayCulture.Name.Replace("-", "_") + "." + name + ".xslt";
			Stream xsltStream = typeof(ReportBuilder).Assembly.GetManifestResourceStream(resourceName);

			// If we couldn't find a localized version, fallback to the en-GB version.
			if (xsltStream == null)
			{
				resourceName = "FlyingGentleman.Library.InstallationReport.Templates.en_GB." + name + ".xslt";
				xsltStream = typeof(ReportBuilder).Assembly.GetManifestResourceStream(resourceName);
			}

			if (xsltStream == null)
			{
				throw new ArgumentException(
					string.Format("A report template with the resource name {0} could not be found.", resourceName));
			}
			else
			{
				using (XmlReader reader = XmlReader.Create(xsltStream))
				{
					returnValue.Load(reader);
				}
			}

			return returnValue;
		}

		private XDocument StripNamespaces(XDocument document)
		{			
			// Read the XSLT.
			string resourceName = "FlyingGentleman.Library.InstallationReport.Templates.StripNamespaces.xslt";
			Stream xsltStream = typeof(ReportBuilder).Assembly.GetManifestResourceStream(resourceName);

			var xt = new XslCompiledTransform();

			using (XmlReader reader = XmlReader.Create(xsltStream))
			{
				xt.Load(reader);
			}
			
			var returnValue = new XDocument();

			using (XmlWriter writer = returnValue.CreateWriter())
			{
				xt.Transform(document.CreateReader(), writer);
			}

			return returnValue;
		}

		private XDocument SerializePackage(SystemPackage package)
		{
			var document = new XDocument();

			var knownTypes = new List<Type>();

			knownTypes.AddRange(package.Servers.Select(s => s.GetType()));
			foreach (var typeArray in package.Servers.Select(s
				=> s.Roles.Select(r => r.GetType())))
			{
				knownTypes.AddRange(typeArray);
			}

			var serializer = new DataContractSerializer(package.GetType(), knownTypes.ToArray());

			using (XmlWriter writer = document.CreateWriter())
			{
				serializer.WriteObject(writer, package);
			}

			return document;
		}
	}
}
