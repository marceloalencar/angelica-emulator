using SQLite;

namespace AngelLib.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        [MaxLength(32), NotNull]
        public string name { get; set; }
        [NotNull]
        public string passwd { get; set; }
    }
}
