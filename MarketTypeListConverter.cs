using System;
using System.Collections.Generic;
using CoreUtilities;
using Layout;
namespace CoreUtilities
{
	public class MarketTypeListConverter : GenericTypeListConverter
	{
		protected override List<string> Strings
		{
			get { 
				
			return	LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_MARKETTYPES,1);
			
			}
		}
	}
}

