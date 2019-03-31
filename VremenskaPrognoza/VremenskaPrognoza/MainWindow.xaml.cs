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
		string cityName = "New York";
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
				string desc = output.weather[0].description;
				if (desc == "clear sky") weatherImage.Source = new BitmapImage(new Uri("/images/clear sky.png", UriKind.Relative));
				else if (desc == "few clouds") weatherImage.Source = new BitmapImage(new Uri("/images/few clouds.png", UriKind.Relative));
				else if (desc == "scattered clouds") weatherImage.Source = new BitmapImage(new Uri("/images/scattered clouds.png", UriKind.Relative));
				else if (desc == "thunderstorm") weatherImage.Source = new BitmapImage(new Uri("/images/thunderstorm.png", UriKind.Relative));
				else if (desc == "rain" || desc == "moderate rain") weatherImage.Source = new BitmapImage(new Uri("/images/rain.png", UriKind.Relative));
				else if (desc == "broken clouds") weatherImage.Source = new BitmapImage(new Uri("/images/broken clouds.png", UriKind.Relative));
				else if (desc == "mist") weatherImage.Source = new BitmapImage(new Uri("/images/mist.png", UriKind.Relative));
				else if (desc == "shower rain") weatherImage.Source = new BitmapImage(new Uri("/images/shower rain.png", UriKind.Relative));
				else if (desc == "snow") weatherImage.Source = new BitmapImage(new Uri("/images/snow.png", UriKind.Relative));


				lbl_description.Content = string.Format("{0}", desc);

			}
				
		}
	}
}
