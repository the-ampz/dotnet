using ampz_dotnet.Requests;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ampz_dotnet.Model
{
    [Table("TB_AMP_DEVICE")]
    public class Device
    {
        [Key]
        [Column("id_device")]
        public int Id { get; set; }

        [Column("ds_name")]
        public string Name { get; set; }

        [Column("ds_type")]
        public string Type { get; set; }

        [Column("ds_operating_system")]
        public string OperatingSystem { get; set; }

        [ForeignKey("Kid")]
        public int KidId { get; set; }
        public Kid Kid { get; set; }

        public void UpdateInformation(UpdateDeviceRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Type))
            {
                Type = request.Type;
            }

            if (!string.IsNullOrEmpty(request.OperatingSystem))
            {
                OperatingSystem = request.OperatingSystem;
            }
        }
    }
}
