using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class CommunityDetailedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static CommunityDetailedResponse From(Community community)
        {
            return new CommunityDetailedResponse
            {
                Id = community.Id,
                Name = community.Name,
                Description = community.Description
            };
        }
    }
}
