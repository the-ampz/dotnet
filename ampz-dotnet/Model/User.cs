using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ampz_dotnet.Requests;

namespace ampz_dotnet.Model
{
    [Index(nameof(Email), IsUnique = true)]
    [Table("TB_AMZ_USER")]
    public class User
    {
        [Key]
        [Column("id_user")]
        public int Id { get; set; }

        [Column("ds_name")]
        public string Name { get; set; }

        [Column("ds_email")]
        public string Email { get; set; }

        [Column("ds_password")]
        public string Password { get; set; }

        [Column("dt_birth")]
        public DateOnly Birthdate { get; set; }

        [Column("ds_city")]
        public string City { get; set; }

        [Column("ds_state")]
        public string State { get; set; }

        public ICollection<Kid> Kids { get; set; } = new List<Kid>();

        public void UpdateInformation(UpdateUserRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                Password = request.Password;
            }

            if (request.Birthdate.HasValue)
            {
                Birthdate = request.Birthdate.Value;
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                City = request.City;
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                State = request.State;
            }
        }
    }
}
