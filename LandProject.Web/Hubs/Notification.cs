using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Web.Hubs
{
	public class Notification:Hub
	{
		public static void GetNotifys()
		{
			var notifyHub = GlobalHost.ConnectionManager.GetHubContext<Notification>();
			notifyHub.Clients.All.notify("AddLandNews");
		}
	}
}