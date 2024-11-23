using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class DeviceDetailedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string OperatingSystem { get; set; }

        public static DeviceDetailedResponse From(Device device)
        {
            return new DeviceDetailedResponse
            {
                Id = device.Id,
                Name = device.Name,
                Type = device.Type,
                OperatingSystem = device.OperatingSystem
            };
        }
    }
}
