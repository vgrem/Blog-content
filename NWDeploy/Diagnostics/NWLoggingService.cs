using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;

namespace SharePoint.NW2010.Deployment.Diagnostics
{
	/// <summary>
	/// Simple ULS Logging Service for VSK
	/// </summary>
	public class NWLoggingService : SPDiagnosticsServiceBase
	{

		static NWLoggingService()
		{
			s_syncRoot = new object();
		}


		private NWLoggingService()
			: base("NW Logging Service", SPFarm.Local)
		{
		}


		protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
		{
			var areas = new List<SPDiagnosticsArea>
                            {
                                new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
                                                                                 {
                                                                                     new SPDiagnosticsCategory(
                                                                                         DefaultCategoryName,
                                                                                         TraceSeverity.Unexpected,
                                                                                         EventSeverity.Error)
                                                                                 })
                            };

			return areas;

		}



		/// <summary>
		/// Write error message To ULS
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="exception"></param>
		public static void WriteError(Exception exception)
		{
			SPDiagnosticsCategory category = NWLoggingService.Local.Areas[DiagnosticAreaName].Categories[DefaultCategoryName];
			NWLoggingService.Local.WriteTrace(0, category, TraceSeverity.Unexpected, exception.Message + exception.StackTrace);
		}


		/// <summary>
		/// Write exception to ULS
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="message"></param>
		public static void WriteError(string message)
		{
			SPDiagnosticsCategory category = NWLoggingService.Local.Areas[DiagnosticAreaName].Categories[DefaultCategoryName];
			NWLoggingService.Local.WriteTrace(0, category, TraceSeverity.Unexpected, message);
		}



		public static NWLoggingService Local
		{
			get
			{
				if (_local == null)
				{
					lock (s_syncRoot)
					{
						_local = new NWLoggingService();
					}
				}
				return _local;
			}
		}








		private static readonly object s_syncRoot;
		private static NWLoggingService _local;
		public static string DiagnosticAreaName = "NW";
		public static string DefaultCategoryName = "NW Deployment";


	}
}
