using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class MenuViewModel
	{
		public int ID { set; get; }

		public string Name { set; get; }

		public string URL { set; get; }
        public string Alias { set; get; }

        public int? DisplayOrder { set; get; }

		public string Image { set; get; }

		public int? ParentID { set; get; }

		public int MenuGroupID { set; get; }
		public string NameMenuGroup { set; get; }

		public virtual MenuGroupViewModel MenuGroup { set; get; }

		public string Target { set; get; }

		public bool Status { set; get; }
	}
}