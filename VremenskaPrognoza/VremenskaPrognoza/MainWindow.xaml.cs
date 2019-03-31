using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace VremenskaPrognoza
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const string APPID = "66382f7d97c7e80060fb8451446e6ffc";
		string cityName = "Belgrade";
		public MainWindow()
		{
			InitializeComponent();
			getWeather(cityName, APPID);
		}
		void getWeather(string city, string appid)
		{
			using (WebClient web = new WebClient())
			{
				string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q="+city+"&appid="+ appid + "&units=metric&cnt=6");

				var json = web.DownloadString(url);
				var result = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

				WeatherInfo.Root output = result;

				lbl_cityName.Content = string.Format("{0}", output.name);
				lbl_country.Content = string.Format("{0}", output.sys.country);
				lbl_temp.Content = string.Format("{0} \u00B0"+ "C", output.main.temp);

			}
				
		}
	}
}
