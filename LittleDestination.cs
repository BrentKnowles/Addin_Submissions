using System;

namespace Submissions
{
	public class LittleDestination : IComparable 
	{
		public string Market;
		public float Priority;
		
		public int CompareTo(object obj)
		{
			LittleDestination u = (LittleDestination)obj;
			return this.Priority.CompareTo(u.Priority);
			
			
		} 
	}
}

