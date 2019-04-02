using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VremenskaPrognoza
{
	class weatherForecast
	{
		public city city {get; set; }
		public List<list> list { get; set; } // forecast list
    }

	public class weather
	{
		public string main { get; set; }
		public string description { get; set; }

	}

	public class city
	{
		public string name { get; set; }
	}

	public class wind
	{
		public double speed { get; set; }
	}

	public class main
	{
		public double temp { get; set; }
	}

	public class list
	{
		public double dt { get; set; } // day in milliseconds
		public double pressure { get; set; } // pressure hpa
		public double humidity { get; set; } // humidity %
		public wind wind { get; set; }
		public main main { get; set; }
		public List<weather> weather { get; set; } // weather list
		public string dt_txt; // date and time
	}
}
