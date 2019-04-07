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
using System.Device.Location;

namespace VremenskaPrognoza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string APPID = "66382f7d97c7e80060fb8451446e6ffc";
        string cityName;

        public MainWindow()
        {
            InitializeComponent();
            GetLocation();
            getWeather(cityName, APPID);
            getForecast(cityName, APPID);
        }

        void getWeather(string city, string appid)
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + appid + "&units=metric&cnt=6");

                var json = web.DownloadString(url);
                var result = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

                WeatherInfo.Root output = result;


                lbl_citycountry.Content = string.Format("{0}, {1}", output.name, output.sys.country);

                lbl_temp.Content = string.Format("Temperature: {0} \u00B0" + "C", output.main.temp);
                string desc = output.weather[0].description;


                if (desc == "clear sky") weatherImage.Source = new BitmapImage(new Uri("/images/clear sky.png", UriKind.Relative));
                else if (desc == "few clouds") weatherImage.Source = new BitmapImage(new Uri("/images/few clouds.png", UriKind.Relative));
                else if (desc == "scattered clouds") weatherImage.Source = new BitmapImage(new Uri("/images/scattered clouds.png", UriKind.Relative));
                else if (desc == "thunderstorm") weatherImage.Source = new BitmapImage(new Uri("/images/thunderstorm.png", UriKind.Relative));
                else if (desc == "rain" || desc == "moderate rain" || desc == "light rain") weatherImage.Source = new BitmapImage(new Uri("/images/rain.png", UriKind.Relative));
                else if (desc == "broken clouds") weatherImage.Source = new BitmapImage(new Uri("/images/broken clouds.png", UriKind.Relative));
                else if (desc == "mist") weatherImage.Source = new BitmapImage(new Uri("/images/mist.png", UriKind.Relative));
                else if (desc == "shower rain") weatherImage.Source = new BitmapImage(new Uri("/images/shower rain.png", UriKind.Relative));
                else if (desc == "snow") weatherImage.Source = new BitmapImage(new Uri("/images/snow.png", UriKind.Relative));


                lbl_description.Content = string.Format("Weather conditions: {0}", desc);

            }

        }

        void getForecast(string city, string appid)
        {

            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&APPID={1}&units=metric", city, APPID);
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(url);
                var Object = JsonConvert.DeserializeObject<weatherForecast>(json);

                weatherForecast forecast = Object;

                Label currentDate = new Label();
                currentDate.Name = "lbl_date0";
                Label currentMinTemp = new Label();
                currentMinTemp.Name = "lbl_min_temp0";
                Label currentMaxTemp = new Label();
                currentMaxTemp.Name = "lbl_max_temp0";
                Label currentWeather = new Label();
                currentWeather.Name = "lbl_weather0";
				Label currentDay = new Label();
				currentDay.Name = "lbl_day0";

                List<string> conditionsForDay = new List<string>(5);

                double minTempForDay = 100;
                double maxTempForDay = -100;
                double tempForCurrentTime = 0;


                // +1 in index = +3 hours
                for (int forecastIndex = 0; forecastIndex < 40; forecastIndex++)
                {

                    //lbl_cond_2.Content = string.Format("{0}", forecast.list[forecastIndex].weather[0].main); // weather conditions

                    string dateTime = forecast.list[forecastIndex].dt_txt;
                    string date = dateTime.Split(' ')[0];
                    string dayOfMonth = date.Substring(date.Length - 2);

					
					// converting month from number to text
					string month = date.Substring(date.Length - 5, 2);
                    string dateTxt = "JAN";
                    if (month == "02") dateTxt = "FEB";
                    else if (month == "03") dateTxt = "MAR";
                    else if (month == "04") dateTxt = "APR";
                    else if (month == "05") dateTxt = "MAY";
                    else if (month == "06") dateTxt = "JUN";
                    else if (month == "07") dateTxt = "JUL";
                    else if (month == "08") dateTxt = "AUG";
                    else if (month == "09") dateTxt = "SEP";
                    else if (month == "10") dateTxt = "OCT";
                    else if (month == "11") dateTxt = "NOV";
                    else if (month == "12") dateTxt = "DEC";

                    string year = date.Substring(0, 4);
                    string time = dateTime.Split(' ')[1].Substring(0, 5);
					
					// day of the week parsing
					DateTime myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
									   System.Globalization.CultureInfo.InvariantCulture);
					string dayOTW = myDate.DayOfWeek.ToString();
					string dayOfTheWeek = "";

					if (dayOTW == "Monday") dayOfTheWeek = "MON";
					else if (dayOTW == "Tuesday") dayOfTheWeek = "TUE";
					else if (dayOTW == "Wednesday") dayOfTheWeek = "WED";
					else if (dayOTW == "Thursday") dayOfTheWeek = "THU";
					else if (dayOTW == "Friday") dayOfTheWeek = "FRI";
					else if (dayOTW == "Saturday") dayOfTheWeek = "SAT";
					else if (dayOTW == "Sunday") dayOfTheWeek = "SUN";
					

					if (time == "00:00")
                    {

                        if (currentDate.Name == "lbl_date0")
                        {
                            currentDate = lbl_date1;
							currentDay = lbl_day1;
                            currentMinTemp = lbl_min_temp_1;
                            currentMaxTemp = lbl_max_temp_1;
                            if (forecast.list[forecastIndex].weather[0].main.ToUpper().Contains("RAIN"))
                            {
                                img1.Source = new BitmapImage(new Uri("/images/umbrella.png", UriKind.Relative));
                            }

                        }
                        else if (currentDate.Name == "lbl_date1")
                        {
                            currentDate = lbl_date2;
							currentDay = lbl_day2;
							currentMinTemp = lbl_min_temp_2;
                            currentMaxTemp = lbl_max_temp_2;
                            if (forecast.list[forecastIndex].weather[0].main.ToUpper().Contains("RAIN"))
                            {
                                img2.Source = new BitmapImage(new Uri("/images/umbrella.png", UriKind.Relative));
                            }
                        }
                        else if (currentDate.Name == "lbl_date2")
                        {
                            currentDate = lbl_date3;
							currentDay = lbl_day3;
							currentMinTemp = lbl_min_temp_3;
                            currentMaxTemp = lbl_max_temp_3;
                            if (forecast.list[forecastIndex].weather[0].main.ToUpper().Contains("RAIN"))
                            {
                                img3.Source = new BitmapImage(new Uri("/images/umbrella.png", UriKind.Relative));
                            }
                        }
                        else if (currentDate.Name == "lbl_date3")
                        {
                            currentDate = lbl_date4;
							currentDay = lbl_day4;
							currentMinTemp = lbl_min_temp_4;
                            currentMaxTemp = lbl_max_temp_4;
                            if (forecast.list[forecastIndex].weather[0].main.ToUpper().Contains("RAIN"))
                            {
                               img4.Source = new BitmapImage(new Uri("/images/umbrella.png", UriKind.Relative));
                            }
                        }
                        else if (currentDate.Name == "lbl_date4") continue;


                        currentDate.Content = dayOfMonth + "-" + dateTxt + "-" + year;
						currentDay.Content = dayOfTheWeek;

                        // reset min/max temperature
                        minTempForDay = 100;
                        maxTempForDay = -100;

                        conditionsForDay.Clear();

                    }
                    else if (time == "21:00")
                    {
                        currentMinTemp.Content = string.Format("{0} \u00B0" + "C", minTempForDay);
                        currentMaxTemp.Content = string.Format("{0} \u00B0" + "C", maxTempForDay);
                    }
                    else
                    {
                        tempForCurrentTime = forecast.list[forecastIndex].main.temp;
                        if (tempForCurrentTime < minTempForDay) minTempForDay = tempForCurrentTime;
                        if (tempForCurrentTime > maxTempForDay) maxTempForDay = tempForCurrentTime;
                    }

                }
            }
        }

        private void searchEvent(object sender, RoutedEventArgs e)
        {
            var query = txtBox.Text;

            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q=" + query + "&appid=" + APPID + "&units=metric&cnt=6");
            using (WebClient web = new WebClient())
            {
                try
                {
                    var json = web.DownloadString(url);
                    var result = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);
                    WeatherInfo.Root output = result;
                    cityName = output.name;

                    getWeather(cityName, APPID);
                    getForecast(cityName, APPID);
                } catch (WebException)
                {
                    MessageBox.Show("City does not exists!");
                }
            }
        }

        private void locationEvent(object sender, RoutedEventArgs e)
        {
            GetLocation();
            getWeather(cityName, APPID);
            getForecast(cityName, APPID);
        }

        private void refreshEvent(object sender, RoutedEventArgs e)
        {
            getWeather(cityName, APPID);
            getForecast(cityName, APPID);
        }

        private void GetLocation()
        {
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString("http://api.ipstack.com/check?access_key=db073982e42ac25d6ee1260237a7e1f6");
            
                var result = JsonConvert.DeserializeObject<locationInfo.Root>(json);
                cityName = result.city;
				for (int i = 0; i < cityName.Length; i++)
				{
					if (cityName[i] == 'š')
					{
						cityName = cityName.Substring(0, i) + 's' + cityName.Substring(i+1);
					} else if (cityName[i] == 'č' || cityName[i] == 'ć')
					{
						cityName = cityName.Substring(0, i) + 'c' + cityName.Substring(i + 1);
					} else if (cityName[i] == 'ž')
					{
						cityName = cityName.Substring(0, i) + 'z' + cityName.Substring(i + 1);
					} 
				}
            }
        }
    }
}
