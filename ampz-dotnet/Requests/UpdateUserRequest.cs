namespace ampz_dotnet.Requests
{
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
