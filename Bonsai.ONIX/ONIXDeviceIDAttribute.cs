namespace Bonsai.ONIX
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class ONIXDeviceIDAttribute : System.Attribute
    {
        public ONIXDevices.ID deviceID;

        public ONIXDeviceIDAttribute(ONIXDevices.ID id)
        {
            deviceID = id;
        }
    }
}
