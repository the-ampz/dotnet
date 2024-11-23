using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class KidSimplifiedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalEnergySaved { get; set; }
        public int TotalScore { get; set; }

        public static KidSimplifiedResponse From(Kid kid)
        {
            return new KidSimplifiedResponse
            {
                Id = kid.Id,
                Name = kid.Name,
                TotalEnergySaved = kid.TotalEnergySaved,
                TotalScore = kid.TotalScore
            };
        }
    }
}
