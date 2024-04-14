using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using static UOM_Ontology_API.Program;
//"UOM Ontology API.exe" efo human-readable

namespace UOM_Ontology_API
{
    internal class Program
    {
        //Api URL
        private const string baseURL = "http://www.ebi.ac.uk/ols4/api/ontologies/";
        public static async Task Main(string[] args)
        {
            //check if correct number of arguments are provided
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: UOM_Ontology_API.exe {ontologyId} {human-readable || machine-readable}");
                return;
            }
            else 
            {
                var ontologyId = args[0];
                var format = args[1];

                //call the OntologyApi method for api data
                var result = await OntologyApi(ontologyId);
                
                //process result
                if (result != null)
                {
                    //display in human readable format
                    if (format == "human-readable")
                    {
                        Console.WriteLine($"Ontology ID: {result.ontologyId}");
                        Console.WriteLine($"Title: {result.config.title}");
                        Console.WriteLine($"Description: {result.config.description}");
                        Console.WriteLine($"Number Of Terms: {result.numberOfTerms}");
                        Console.WriteLine($"Current Status: {result.status}");
                    }
                    else if (format == "machine-readable")  //display in machine readable format - binary
                    {
                        //serialises data to binary
                        byte[] binaryData = SerializeResultToBinary(result);
                        Console.WriteLine("Binary data:");
                        foreach (byte b in binaryData)
                        {
                            Console.Write($"{b:X2} "); //hexadecimal
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid output format specified. Please specify 'human-readable' or 'machine-readable'.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Unable to retrieve ontology details.");
                }
            }
        }
        //HTTP client for making api requests
        public static HttpClient client = new HttpClient();

        //function to fetch ontology api data
        public static async Task<OntologyData> OntologyApi(string Id)
        {
            try
            {
                var response = await client.GetAsync(string.Format(baseURL + Id));

                //check if request is successful
                if (response.IsSuccessStatusCode)
                {
                    //deserialises json and stores it in the OntologyData class
                    OntologyData ontologyData = await response.Content.ReadFromJsonAsync<OntologyData>();

                    return ontologyData;
                }
                else
                {
                    Console.WriteLine("Error: Unable to retrieve ontology details.");
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Error: Unable to connect to the Ontology Lookup Service.");
                return null;
            }
        }
        //function to serialize OntologyData object to binary format
        public static byte[] SerializeResultToBinary(OntologyData data)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, data);
                return memoryStream.ToArray();
            }
        }
            
    }
}
