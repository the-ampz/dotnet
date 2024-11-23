using ampz_dotnet.Model;

namespace ampz_dotnet.Builders
{
    public class DeviceBuilder
    {
        private readonly Device _device;

        public DeviceBuilder()
        {
            _device = new Device();
        }

        public DeviceBuilder Name(string name)
        {
            _device.Name = name;
            return this;
        }

        public DeviceBuilder Type(string type)
        {
            _device.Type = type;
            return this;
        }

        public DeviceBuilder OperatingSystem(string operatingSystem)
        {
            _device.OperatingSystem = operatingSystem;
            return this;
        }

        public DeviceBuilder KidId(int kidId)
        {
            _device.KidId = kidId;
            return this;
        }

        public Device Build()
        {
            return _device;
        }
    }
}
