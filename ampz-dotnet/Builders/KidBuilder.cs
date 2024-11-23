using ampz_dotnet.Model;

namespace ampz_dotnet.Builders
{
    public class KidBuilder
    {
        private readonly Kid _kid;

        public KidBuilder()
        {
            _kid = new Kid();
        }

        public KidBuilder Name(string name)
        {
            _kid.Name = name;
            return this;
        }

        public KidBuilder Birthdate(DateOnly birthdate)
        {
            _kid.Birthdate = birthdate;
            return this;
        }

        public KidBuilder TotalScore(int totalScore)
        {
            _kid.TotalScore = totalScore;
            return this;
        }

        public KidBuilder TotalEnergySaved(decimal totalEnergySaved)
        {
            _kid.TotalEnergySaved = totalEnergySaved;
            return this;
        }

        public KidBuilder UserId(int userId)
        {
            _kid.UserId = userId;
            return this;
        }

        public Kid Build()
        {
            return _kid;
        }
    }
}
