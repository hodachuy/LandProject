using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class NotifyViewModel
	{
		public int ID { set; get; }
		[MaxLength(256)]
		public string Message { set; get; }
		public int? SenderID { set; get; }
		public int? RecipientID { set; get; }
		public bool? IsRead { set; get; }
		[MaxLength(50)]
		public string TableName { set; get; }
		public int? TableItemID { set; get; }
		public DateTime? CreatedDate { set; get; }

		[MaxLength(256)]
		public string URL { set; get; }
	}
}