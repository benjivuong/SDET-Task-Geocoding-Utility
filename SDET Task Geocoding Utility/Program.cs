using Newtonsoft.Json;
using SDET_Task_Geocoding_Utility;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UtilityIntegrationTests")]
namespace SDETTaskGeocodingUtility
{
    /// <summary>
    /// Geocoding utility class.
    /// </summary>
    public class SDETTaskGeocodingUtility
    {
        private const string API_KEY = "f897a99d971b5eef57be6fafa0d83239";
        private readonly List<string> LocationNames = new();
        private readonly List<string> LocationZips = new();
        private readonly HttpClient client = new();

        public SDETTaskGeocodingUtility()
        {
            // no-op
        }

        /// <summary>
        /// Prompts the user for a list of locations that they want to geocode
        /// </summary>
        private void PromptLocations()
        {
            bool endRunning = false;
            while (!endRunning)
            {
                Console.WriteLine("Enter a location name or a zip code (Enter QUIT to stop): ");
                string userInput = Console.ReadLine()!;
                if (string.IsNullOrEmpty(userInput) || userInput == "QUIT")
                {
                    endRunning = true;
                }
                else
                {
                    if (int.TryParse(userInput, out int numericZip))
                    {
                        LocationZips.Add(userInput);
                    }
                    else
                    {
                        // remove spaces
                        userInput = userInput.Replace(" ", "");
                        LocationNames.Add(userInput);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the Open Weather APIs for location name based geocoding
        /// </summary>
        private async Task<bool> ProcessLocationNameList()
        {
            foreach (string locationName in LocationNames)
            {
                Console.WriteLine($"\nResult for location {locationName}:");
                string URI = $"http://api.openweathermap.org/geo/1.0/direct?q={locationName},US&limit={1}&appid={API_KEY}";
                try
                {
                    var json = await client.GetStringAsync(URI);
                    json = json.Substring(1, json.Length - 2);
                    LocationInformation locationInformation = JsonConvert.DeserializeObject<LocationInformation>(json)!;
                    Console.WriteLine(locationInformation);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Not Found"); 
                    }
                    else
                    {
                        Console.WriteLine("Error Occured");
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Calls the Open Weather APIs for zip code based geocoding
        /// </summary>
        private async Task<bool> ProcessZipCodeList()
        {
            foreach (string zip in LocationZips)
            {
                Console.WriteLine($"\nResult for zip {zip}:");
                string URI = $"http://api.openweathermap.org/geo/1.0/zip?zip={zip}&appid={API_KEY}";
                try
                {
                    var json = await client.GetStringAsync(URI);
                    LocationInformation locationInformation = JsonConvert.DeserializeObject<LocationInformation>(json)!;
                    Console.WriteLine(locationInformation);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Not Found");
                    }
                    else
                    {
                        Console.WriteLine("Error Occured");
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Runs the utility, and prints out results
        /// </summary>
        public async Task Run()
        {
            PromptLocations();
            await ProcessLocationNameList();
            await ProcessZipCodeList();
        }
    }
    internal class Program
    {
        public static void Main()
        {
            SDETTaskGeocodingUtility util = new SDETTaskGeocodingUtility();
            Task t = util.Run();
            t.Wait();
        }
    }
}