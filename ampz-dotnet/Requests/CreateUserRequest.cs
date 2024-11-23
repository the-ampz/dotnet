namespace ampz_dotnet.Requests
{
    public class CreateUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required DateOnly Birthdate { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public List<CreateKidRequest>? Kids { get; set; }
    }
}
