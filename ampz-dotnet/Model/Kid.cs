using ampz_dotnet.Requests;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ampz_dotnet.Model
{
    [Table("TB_AMZ_KID")]
    public class Kid
    {
        [Key]
        [Column("id_kid")]
        public int Id { get; set; }

        [Column("ds_name")]
        public string Name { get; set; }

        [Column("dt_birth")]
        public DateOnly Birthdate { get; set; }

        [Column("total_score")]
        public int TotalScore { get; set; }

        [Column("total_energy_saved")]
        public decimal TotalEnergySaved { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Community> Communities { get; set; } = new List<Community>();

        public void UpdateInformation(UpdateKidRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                Name = request.Name;
            }

            if (request.Birthdate.HasValue)
            {
                Birthdate = request.Birthdate.Value;
            }
        }
    }
}
