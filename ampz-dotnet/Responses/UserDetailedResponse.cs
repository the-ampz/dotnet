using ampz_dotnet.Model;
using System.DirectoryServices.Protocols;

namespace ampz_dotnet.Responses
{
    public class UserDetailedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateOnly Birthdate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<KidDetailedResponse>? Kids { get; set; } 

        public static UserDetailedResponse From(User user)
        {
            return new UserDetailedResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Birthdate = user.Birthdate,
                City = user.City,
                State = user.State,
                Kids = user.Kids?.Select(KidDetailedResponse.From).ToList() 
            };
        }
    }
}
