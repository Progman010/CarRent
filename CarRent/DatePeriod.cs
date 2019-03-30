using System;
using System.Collections.Generic;

namespace CarRent
{
	public class DatePeriod
    {
		public DatePeriod(DateTimeOffset start, DateTimeOffset end)
		{
			Start = start.Date;
			End = end.Date;
			if (Start > End)
			{
				throw new ArgumentException("Start date should not be later than end date");
			}
		}

		public DateTimeOffset Start { get; }
		public DateTimeOffset End { get; }

		public override bool Equals(object obj)
		{
			var period = obj as DatePeriod;
			return period != null &&
				   Start.Equals(period.Start) &&
				   End.Equals(period.End);
		}

		public bool IntersectsWith(DatePeriod anotherPeriod)
		{
			return !(Start > anotherPeriod.End || End < anotherPeriod.Start);
		}

		public static bool operator ==(DatePeriod period1, DatePeriod period2)
		{
			return EqualityComparer<DatePeriod>.Default.Equals(period1, period2);
		}

		public static bool operator !=(DatePeriod period1, DatePeriod period2)
		{
			return !(period1 == period2);
		}
	}
}
