using System.Xml.Linq;
using Microsoft.SharePoint.Utilities;
using Nintex.Workflow.HumanApproval;

namespace SharePoint.NintexDeployment.Infrastructure
{
	/// <summary>
	/// Nintex Tasks Helper
	/// </summary>
	public static class NintexTaskExtensions
	{
		/// <summary>
		/// For demonstation purposes only
		/// </summary>
		/// <param name="tasks"></param>
		/// <returns></returns>
		public static string GetAllApprovalTaskCommentsAsXml(this NintexTaskCollection tasks)
		{
			var comments = new XElement("Comments");
			foreach (var nintexTask in tasks)
			{
				foreach (var approver in nintexTask.Approvers)
				{
					var comment = new XElement("Comment",
										   new XElement("Approver",approver.DisplayName),
										   new XElement("Outcome", approver.ApprovalOutcome),
										   new XElement("Comment",approver.Comments),
										   new XElement("EntryTime", SPUtility.FormatDate(nintexTask.WFContext.Web, approver.EntryTime, SPDateFormat.DateTime))
					);
					comments.Add(comment);
				}
			}
			return comments.ToString();
		}
	}
}
