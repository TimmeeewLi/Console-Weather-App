using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Weather_App
{
    public class WeatherResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("weather")]
        public Weather[]? Weather { get; set; }

        [JsonPropertyName("main")]
        public Main? Main { get; set; }

        [JsonPropertyName("wind")]
        public Wind? Wind { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }

    [JsonSerializable(typeof(WeatherResponse))]
    [JsonSerializable(typeof(Weather))]
    [JsonSerializable(typeof(Main))]
    [JsonSerializable(typeof(Wind))]
    public partial class WeatherAppJsonContext : JsonSerializerContext
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Weather application V0.0.1");

            var client = new HttpClient();

            Console.WriteLine("Please enter your OpenWeather API Key: ");
            string? api_key = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(api_key))
            {
                Console.WriteLine("Please enter a valid OpenWeather API Key: ");
                api_key = Console.ReadLine();
            }

            Console.WriteLine("Please enter your current city name: ");
            string? city_name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(city_name))
            {
                Console.WriteLine("Please enter a valid city name: ");
                city_name = Console.ReadLine();
            }
            city_name = city_name.ToLower();

            string userURL = $"https://api.openweathermap.org/data/2.5/weather?q={city_name}&appid={api_key}&units=metric";

            try
            {
                var weatherResponse = client.GetStringAsync(userURL).Result;

                var formattedResponse = JsonSerializer.Deserialize(
                    weatherResponse, 
                    WeatherAppJsonContext.Default.WeatherResponse
                );

                if (formattedResponse != null)
                {
                    Console.WriteLine($"\nWeather in {formattedResponse.Name}:");
                    Console.WriteLine($"Description: {formattedResponse.Weather[0].Description}");
                    Console.WriteLine($"Temperature: {formattedResponse.Main.Temp}°C");
                    Console.WriteLine($"Humidity: {formattedResponse.Main.Humidity}%");
                    Console.WriteLine($"Wind Speed: {formattedResponse.Wind.Speed} m/s");
                }
                else
                {
                    Console.WriteLine("Failed to retrieve weather data. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
