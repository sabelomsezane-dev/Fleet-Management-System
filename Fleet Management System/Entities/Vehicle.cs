using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management_System.Model
{
    [Table("tb_Vehicle", Schema = "dbo")]
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleID { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string RegistrationNumber { get; set; }

        public ICollection<VehicleLocation> VehicleLocations { get; set; }
    }
}