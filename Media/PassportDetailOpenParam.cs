namespace Media
{
	public class PassportDetailOpenParam
	{
		public string PassportKey { get; set; }
		public string EntityKey { get; set; }
		public string ParentKey { get; set; }
		public int IsEditedPassport { get; set; }
        public int IsVisibleButtonTransit { get; set; }
        public int IsChildPassport { get; set; }
        public string ParentNameKeyOnAdmin { get; set; }
        public bool isFindTreeonkey { get; set; }
        
        
	}
}
