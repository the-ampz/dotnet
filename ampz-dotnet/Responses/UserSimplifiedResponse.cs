using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class UserSimplifiedResponse
    {
            public int Id { get; set; }
            public string Name { get; set; }

            public static UserSimplifiedResponse From(User user)
            {
                return new UserSimplifiedResponse
                {
                    Id = user.Id,
                    Name = user.Name
                };
            }
        }
}
