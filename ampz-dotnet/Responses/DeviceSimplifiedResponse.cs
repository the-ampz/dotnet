using ampz_dotnet.Model;

namespace ampz_dotnet.Responses
{
    public class DeviceSimplifiedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DeviceSimplifiedResponse From(Device device)
        {
            return new DeviceSimplifiedResponse
            {
                Id = device.Id,
                Name = device.Name
            };
        }
    }
}
