using SQLite;

namespace AngelLib.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public uint ID { get; set; }
        [MaxLength(32), Unique, NotNull]
        public string name { get; set; }
        [NotNull]
        public string passwd { get; set; }
    }
}
