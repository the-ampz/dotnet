namespace ampz_dotnet.Requests
{
    public class CreateKidRequest
    {
        public required string Name { get; set; }
        public required DateOnly Birthdate { get; set; }
        public List<CreateDeviceRequest>? Devices { get; set; } 

    }
}
