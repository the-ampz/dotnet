using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class CommunitySimplifiedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public static CommunitySimplifiedResponse From(Community community)
        {
            return new CommunitySimplifiedResponse
            {
                Id = community.Id,
                Name = community.Name,
                Description = community.Description
            };
        }
    }
}
