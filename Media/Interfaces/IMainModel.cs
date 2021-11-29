
using System.Collections.Generic;
using Passpot.Business;


namespace Media.Interfaces
{
	public interface IMainModel
	{
		void Report(string message);
	    void Indicator(bool show);
		void FirePropertyChanged(string propertyname);
        PassportDetailOpenParam NeedOpenPassportDetail { get; set; }
        Stack<Navigation> sBack { get; set; }
        Stack<Navigation> sForward { get; set; }
	    
	}
}
