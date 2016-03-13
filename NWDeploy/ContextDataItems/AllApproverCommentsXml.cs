using Nintex.Workflow;
using Nintex.Workflow.HumanApproval;
using SharePoint.NintexDeployment.Infrastructure;

namespace SharePoint.NintexDeployment.ContextDataItems
{
	/// <summary>
	/// Context DataItem for All Approvers in Xml format
	/// </summary>
	public class AllApproverCommentsXml : ContextDataItemBase
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <returns></returns>
		public override object GetValueObject(NWWorkflowContext ctx)
		{
			return GetLastApproverCommentsAsXml(ctx);
		}


		internal string GetLastApproverCommentsAsXml(NWWorkflowContext ctx)
		{
			return new NintexTaskCollection(ctx.InstanceID, ctx.Web).GetAllApprovalTaskCommentsAsXml();
		}


		/// <summary>
		/// Internal Name
		/// </summary>
		public override string InternalName
		{
			get { return "AllApproverCommentsXml"; }
		}


		
	}
}
