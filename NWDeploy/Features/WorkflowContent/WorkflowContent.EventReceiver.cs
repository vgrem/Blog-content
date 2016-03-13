using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Nintex.Workflow;
using SharePoint.NW2010.Deployment.Diagnostics;
using SharePoint.NintexDeployment.Infrastructure;
using SharePoint.NintexDeployment.Utilities;

namespace SharePoint.NW2010.Deployment.Features.WorkflowContent
{
	/// <summary>
	/// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
	/// </summary>
	

	[Guid("96884020-9aa0-42f5-bbbf-093509be5b69")]
	public class WorkflowContentEventReceiver : SPFeatureReceiver
	{
		// Uncomment the method below to handle the event raised after a feature has been activated.

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
			DeployWorkflows(properties);
			RegisterContextDataItems(properties);
		}



		private void DeployWorkflows(SPFeatureReceiverProperties properties)
		{
			var workflowAdapter = new NWFAdapter(properties);
			//Workflows
			var workflowMappings = workflowAdapter.RetrieveWorkflowMappings("NWMappings.xml");
			foreach (var nwMappingEntry in workflowMappings)
			{
				try
				{
					workflowAdapter.PublishReusableWorkflow(nwMappingEntry);
					WorkflowUtilities.BindWorkflow((SPWeb)properties.Feature.Parent, nwMappingEntry.BindingName, nwMappingEntry.WorkflowName);
				}
				catch (Exception ex)
				{
					NWLoggingService.WriteError(ex);
				}
			}
		}


		private void RegisterContextDataItems(SPFeatureReceiverProperties properties)
		{
			var workflowAdapter = new NWFAdapter(properties);
			var ctxItemsMappings = workflowAdapter.RetrieveContextDataMappings("ContextItemsMappings.xml");
			foreach (var mapping in ctxItemsMappings)
			{
				try
				{
					CustomCommonDataCollection.Add(mapping.TypeName, mapping.AssemblyName, mapping.DisplayName, mapping.Description, string.Empty);
				}
				catch (Exception ex)
				{
					NWLoggingService.WriteError(ex);
				}
			}
		}

		
	}
}
