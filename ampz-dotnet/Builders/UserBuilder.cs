using ampz_dotnet.Model;
namespace ampz_dotnet.Builders
{
    public class UserBuilder
    {
        private readonly User _user;

        public UserBuilder()
        {
            _user = new User();
        }

        public UserBuilder Name(string name)
        {
            _user.Name = name;
            return this;
        }

        public UserBuilder Email(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder Password(string password)
        {
            _user.Password = password;
            return this;
        }

        public UserBuilder Birthdate(DateOnly birthdate)
        {
            _user.Birthdate = birthdate;
            return this;
        }
        public UserBuilder City(string city)
        {
            _user.City = city;
            return this;
        }

        public UserBuilder State(string state)
        {
            _user.State = state;
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}
