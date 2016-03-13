using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SharePoint;
using Nintex.Workflow;
using Nintex.Workflow.Common;
using Nintex.Workflow.PartialWorkflows;
using Nintex.Workflow.Publishing;
using SharePoint.NintexDeployment.Utilities;

namespace SharePoint.NintexDeployment.Infrastructure
{
	/// <summary>
	/// Nintex artifacts feature adapter
	/// </summary>
	public class NWFAdapter
	{
		#region ctors

		public NWFAdapter(SPFeatureReceiverProperties properties)
		{
			this.properties = properties;
			this.web = (SPWeb)properties.Feature.Parent;
		}

		#endregion


		#region Workflows

		/// <summary>
		/// Publish Reusable Workflow
		/// </summary>
		/// <param name="mapping"></param>
		public void PublishReusableWorkflow(NWFMappingEntry mapping)
		{
			SPContentType ct = web.ContentTypes[mapping.BindingName];
			string workflowName = mapping.WorkflowName;
			string pathToNWF = Path.Combine(properties.Definition.RootDirectory, mapping.WorkflowFileName);
			byte[] workflowData = File.ReadAllBytes(pathToNWF);
			string workflowFile = Utility.ConvertByteArrayToString(workflowData);
			

			while ((int)workflowFile[0] != (int)char.ConvertFromUtf32(60)[0])
				workflowFile = workflowFile.Remove(0, 1);
			

			ExportedWorkflowWithListMetdata workflowWithListMetdata = ExportedWorkflowWithListMetdata.Deserialize(workflowFile);
			string xmlMessage = workflowWithListMetdata.ExportedWorkflowSeralized;
			SPListCollection lists = web.Lists;
			Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>(lists.Count);
			foreach (SPList spList in lists)
			{
				if (!dictionary.ContainsKey(spList.Title.ToUpper()))
					dictionary.Add(spList.Title.ToUpper(), spList.ID);
			}
			foreach (var listReference in workflowWithListMetdata.ListReferences)
			{
				string key = listReference.ListName.ToUpper();
				if (dictionary.ContainsKey(key) && !dictionary.ContainsValue(listReference.ListId))
					xmlMessage = xmlMessage.Replace(Utility.FormatGuid(listReference.ListId), Utility.FormatGuid(dictionary[key]));
			}
			var exportedWorkflow = WorkflowPart.Deserialize<ExportedWorkflow>(xmlMessage);
			foreach (var config in exportedWorkflow.Configurations.ActionConfigs)
				WorkflowRenderer.ProcessActionConfig(config);
			

			Guid listId = Guid.Empty;
			bool validateWorkflow = true;
			Publish publish = new Publish(web);
			publish.PublishAWorkflow(workflowName, exportedWorkflow.Configurations, listId, web, (ImportContext)null, validateWorkflow, ct.Id, string.Empty);
		}





		/// <summary>
		/// Retrieve Workflow Mappings
		/// </summary>
		/// <param name="mappingFileName"></param>
		/// <returns></returns>
		public NWFMappingEntries RetrieveWorkflowMappings(string mappingFileName)
		{
			var mappingFullPath = Path.Combine(properties.Definition.RootDirectory, mappingFileName);
			return Serializer.DeserializeObject<NWFMappingEntries>(File.ReadAllText(mappingFullPath));
		}

		
	


		#endregion


		#region Context Data


		/// <summary>
		/// Retrieve Context Data Mappings
		/// </summary>
		/// <param name="mappingFileName"></param>
		/// <returns></returns>
		public ContextItemMappingEntries RetrieveContextDataMappings(string mappingFileName)
		{
			var mappingFullPath = Path.Combine(properties.Definition.RootDirectory, mappingFileName);
			return Serializer.DeserializeObject<ContextItemMappingEntries>(File.ReadAllText(mappingFullPath));
		}

		#endregion


		#region Members

		private SPFeatureReceiverProperties properties;
		private SPWeb web;



		#endregion

	}
}
