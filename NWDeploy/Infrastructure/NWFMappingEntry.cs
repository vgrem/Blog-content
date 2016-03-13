using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Nintex.Workflow;

namespace SharePoint.NintexDeployment.Infrastructure
{
    /// <summary>
    /// Nintex WF Mapping Entry
    /// </summary>
    public class NWFMappingEntry
    {

        public NWFMappingEntry()
        {
            Category = WorkflowCategory.Reusable; //default
        }

        public string WorkflowName { get; set; }

        public string WorkflowFileName { get; set; }

        public string BindingName { get; set; }

        [XmlAttribute()]
        public WorkflowCategory Category {
            get
            {
                return WorkflowCategory.Reusable;
            }
            set
            {
                if (value != WorkflowCategory.Reusable)
                    throw new NotSupportedException(value.ToString());
            }
        }
    }


    [Serializable()]
    [XmlType(TypeName = "NWFMappingEntries")]
    public class NWFMappingEntries : List<NWFMappingEntry>
    {

    }
}
