using System;
using System.IO;
using System.Xml.Serialization;

namespace AngelLib.Configuration.LinkServer
{
    [Serializable]
    public class Settings
    {
        private static Settings _settings;
        private string _clientSignature = "33303030303063633932356330653161656535623634303763373561393530343136";
        private int _clientListenPort = 29000;
        private bool _isPvP = false;
        private bool _moneyBonus = true;
        private bool _dropBonus = true;
        private bool _spiritBonus = true;
        private double _expMultiplier = 1.5;

        public string ClientSignature { get => _clientSignature; set => _clientSignature = value; }
        public int ClientListenPort { get => _clientListenPort; set => _clientListenPort = value; }
        public bool IsPvP { get => _isPvP; set => _isPvP = value; }
        public bool MoneyBonus { get => _moneyBonus; set => _moneyBonus = value; }
        public bool DropBonus { get => _dropBonus; set => _dropBonus = value; }
        public bool SpiritBonus { get => _spiritBonus; set => _spiritBonus = value; }
        public double ExpMultiplier { get => _expMultiplier; set => _expMultiplier = value; }

        public static Settings LoadSettings()
        {
            if (_settings == null)
            {
                try
                {
                    Deserialize();
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Settings file not found. Creating a new file...");
                    _settings = new Settings();
                    Serialize();
                }
            }
            return _settings;
        }

        private static void Deserialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            Directory.CreateDirectory("Configuration");
            using (TextReader textReader = new StreamReader("Configuration\\LinkServer.xml"))
            {
                _settings = (Settings)xmlSerializer.Deserialize(textReader);
            }
        }

        private static void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            using (TextWriter textWriter = new StreamWriter("Configuration\\LinkServer.xml"))
            {
                xmlSerializer.Serialize(textWriter, _settings);
            }
        }
    }
}
