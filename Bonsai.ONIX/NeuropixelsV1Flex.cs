using System;

namespace Bonsai.ONIX
{
    public class NeuropixelsV1Flex : I2CConfiguration
    {
        public NeuropixelsV1Flex(ONIController controller, uint? device_index)
            : base(controller, device_index, 0x50)
        {
            if (controller != null)
                Update();
        }

        public void Update()
        {
            ProbeSN = BitConverter.ToUInt64(ReadBytes((uint)EEPROM.OFFSET_ID, 8), 0);
            Version = ReadByte((uint)EEPROM.OFFSET_VERSION).ToString() + "." + ReadByte((uint)EEPROM.OFFSET_REVISION).ToString();
            PartNo = ReadASCIIString((uint)EEPROM.OFFSET_FLEXPN, 20);
            ProbePartNo = ReadASCIIString((uint)EEPROM.OFFSET_PROBEPN, 20);
        }

        public enum EEPROM : uint
        {
            OFFSET_ID = 0,
            OFFSET_VERSION = 10,
            OFFSET_REVISION = 11,
            OFFSET_FLEXPN = 20,
            OFFSET_PROBEPN = 40,
            OFFSET_BIST = 0xFC,
        };

        [System.Xml.Serialization.XmlIgnore]
        public ulong? ProbeSN { get; private set; }

        [System.Xml.Serialization.XmlIgnore]
        public string Version { get; private set; }

        [System.Xml.Serialization.XmlIgnore]
        public string PartNo { get; private set; }

        [System.Xml.Serialization.XmlIgnore]
        public string ProbePartNo { get; private set; }
    }
}
