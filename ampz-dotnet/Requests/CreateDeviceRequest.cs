namespace ampz_dotnet.Requests
{
    public class CreateDeviceRequest
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string OperatingSystem { get; set; }
    }
}
