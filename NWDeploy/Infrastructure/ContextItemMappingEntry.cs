using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharePoint.NintexDeployment.Infrastructure
{
	public class ContextItemMappingEntry
	{

		public string TypeName { get; set; }

		public string AssemblyName { get; set; }

		public string DisplayName { get; set; }

		public string Description { get; set; }

		public string Data { get; set; }
	}


	[Serializable()]
	[XmlType(TypeName = "ContextItemMappingEntries")]
	public class ContextItemMappingEntries : List<ContextItemMappingEntry>
	{

	}
}
