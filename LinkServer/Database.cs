using AngelLib.Model;
using SQLite;

namespace LinkServer
{
    public class Database
    {
        private SQLiteConnection _connection;

        public Database()
        {
            _connection = new SQLiteConnection("AngelEmu.db");
            _connection.CreateTable<User>();
        }

        public string GetUserPasswd(string username, string hash)
        {
            TableQuery<User> found = _connection.Table<User>().
                Where(v => v.name == username && v.passwd == hash);
            if (found.Count() == 1)
                return found.First().passwd;
            else
                return string.Empty;
        }

        public int GetIdByUsername(string username)
        {
            TableQuery<User> found = _connection.Table<User>().
                Where(v => v.name == username);
            return found.First().ID;
        }
    }
}
