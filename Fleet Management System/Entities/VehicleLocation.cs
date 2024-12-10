using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management_System.Model
{
    public class VehicleLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public double Latitude { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public double Longitude { get; set; }

        public DateTime Timestamp { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}