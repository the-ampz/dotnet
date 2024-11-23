using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class KidDetailedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Birthdate { get; set; }
        public int TotalScore { get; set; }
        public decimal TotalEnergySaved { get; set; }

        public static KidDetailedResponse From(Kid kid)
        {
            return new KidDetailedResponse
            {
                Id = kid.Id,
                Name = kid.Name,
                Birthdate = kid.Birthdate,
                TotalScore = kid.TotalScore,
                TotalEnergySaved = kid.TotalEnergySaved
            };
        }
    }
}
