using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0



namespace WeatherApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class DataReceiver { 
    public static async Task<string> GetWeatherDataFrom(this HttpClient httpClient, string url)
        {

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {

                var response =await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                //var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                //MessageBox.Show(responseBody);

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);


            }


        }
    }

    
public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public class WeatherData
        {
            public Dictionary<string, Coordinations> coord { get; set; }
            public IList<string> weather { get; set; }
            public string base_ { get; set; }
            public Dictionary<string, MainWeatherData> main { get; set; }
            public int visibility { get; set; }
            public Dictionary<string, WindClass> wind { get; set; }

            public Dictionary<string, CloudsClass> clouds { get; set; }
            public DateTimeOffset dt { get; set; }
            public Dictionary<string, SysClass> sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }

        }

        public class SysClass
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public DateTimeOffset sunrise { get; set; }
            public DateTimeOffset lsunsetat { get; set; }
        }
        public class Coordinations
        {
            public float lon { get; set; }
            public float lat { get; set; }
        }
        public class WindClass
        {
            public string speed { get; set; }
            public string deg { get; set; }
        }
        public class CloudsClass
        {
            public string all { get; set; }

        }


        public class MainWeatherData
        {
            public string temp { get; set; }
            public string feels_like { get; set; }
            public string temp_min { get; set; }
            public string temp_max { get; set; }
            public string pressure { get; set; }
            public string humidity { get; set; }
    }
        private const string API_KEY = "cb51fb61c7a262cc87d24cd071ad6c02";
        private const string url = "http://api.openweathermap.org/data/2.5/weather?q=berlin&mode=json&appid=cb51fb61c7a262cc87d24cd071ad6c02";
        public readonly HttpClient httpClient = new HttpClient();



        public void AppendWeatherData(object sender, EventArgs e)
        {
            var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                JsonNumberHandling.WriteAsString
                    };

            var response = DataReceiver.GetWeatherDataFrom(httpClient, url).Result;
            response = response.Replace("base", "base_");
            MessageBox.Show(response);
            //response = response.Replace(".", "");
            WeatherData dataObject = JsonSerializer.Deserialize<WeatherData>(response, options);

        

            ContentBox.Text += dataObject?.name+"\n";



        }

    }
}
