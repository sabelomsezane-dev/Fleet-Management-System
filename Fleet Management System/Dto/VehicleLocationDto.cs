namespace Fleet_Management_System.Dto
{
    public class VehicleLocationDto
    {
        public int VehicleID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string VehicleRegistrationNumber { get; set; }
    }
}