using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fleet_Management_System.RequestModels
{
    public class VehicleLocationRequestModel
    {
        [JsonPropertyName("Registration_Number")]
        public string RegistrationNumber { get; set; }

        [JsonPropertyName("Latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("Longitude")]
        public double Longitude { get; set; }
    }
}