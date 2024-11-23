using ampz_dotnet.Model;

namespace ampz_dotnet.Builders
{
    public class CommunityBuilder
    {
        private readonly Community _community;

        public CommunityBuilder()
        {
            _community = new Community();
        }

        public CommunityBuilder Name(string name)
        {
            _community.Name = name;
            return this;
        }

        public CommunityBuilder Description(string description)
        {
            _community.Description = description;
            return this;
        }

        public Community Build()
        {
            return _community;
        }
    }
}
