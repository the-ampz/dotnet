using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using ampz_dotnet.Requests;

namespace ampz_dotnet.Model
{
    [Table("TB_AMP_COMMUNITY")]
    public class Community
    {
        [Key]
        [Column("id_community")]
        public int Id { get; set; }

        [Column("ds_name")]
        public string Name { get; set; }

        [Column("ds_description")]
        public string Description { get; set; }

        public ICollection<Kid> Kids { get; set; } = new List<Kid>();

        public void UpdateInformation(UpdateCommunityRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                Description = request.Description;
            }
        }
    }
}
