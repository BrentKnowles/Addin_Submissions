using System;
using CoreUtilities;
using Layout;
using System.Collections.Generic;

namespace CoreUtilities
{
	public class PublishTypeListConverter : GenericTypeListConverter
	{
		protected override List<string> Strings
		{
			get { 


				return LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_PUBLISHTYPES,1);
			}
		}
	}
}

