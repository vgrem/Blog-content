using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using SharePoint.NintexDeployment.Infrastructure;

namespace SharePoint.NintexDeployment.Utilities
{
	public class WorkflowUtilities
	{
		/// <summary>
		/// Bind workflow
		/// </summary>
		/// <param name="web"></param>
		/// <param name="contentTypeName"></param>
		/// <param name="workflowName"></param>
		public static void BindWorkflow(SPWeb web, string contentTypeName, string workflowName)
		{
			SPContentType siteContentType = web.ContentTypes[contentTypeName];
			string taskListTitle = ApplicationConfig.TaskListName;
			string historyListTitle = ApplicationConfig.HistoryListName;
			SPWorkflowAssociation wfAssoc = null;

			// Get a template.
			SPWorkflowTemplate workflowTemplate = web.WorkflowTemplates.Cast<SPWorkflowTemplate>().FirstOrDefault(wft => wft.Name == workflowName);


			// Add the association to the content type or update it if it already exists.
			if ((wfAssoc = siteContentType.WorkflowAssociations.GetAssociationByName(workflowName, web.Locale)) == null)
			{
				wfAssoc = SPWorkflowAssociation.CreateWebContentTypeAssociation(workflowTemplate,
																	 workflowName,
																	 taskListTitle,
																	 historyListTitle);
				siteContentType.WorkflowAssociations.Add(wfAssoc);
			}
			else
			{
				siteContentType.WorkflowAssociations.Update(wfAssoc);
			}

			// Propagate to children of this content type.
			siteContentType.UpdateWorkflowAssociationsOnChildren(false,  // Do not generate full change list
																 true,   // Push down to derived content types
																 true,   // Push down to list content types
																 false); // Do not throw exception if sealed or readonly  




		}
	}
}
