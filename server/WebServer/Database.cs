using System;
using System.Data;
using System.Data.SqlClient;

namespace WebServer
{

    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class Database
    {
        private static Database _instance;
        private const string ServerName = "ecom.core.sql";
        private const string DbName = "project";
        private const string UserName = "ctes_user";
        private const string PasswordName = "ctes_user";

        private readonly SqlConnection _connection;

        private Database()
        {
            const string connectionString = "" +
                                            "Data Source=" + ServerName + ";" +
                                            "Initial Catalog=" + DbName + ";" +
                                            "User ID=" + UserName + ";" +
                                            "Password=" + PasswordName + ";" +
                                            "";

            _connection = new SqlConnection(connectionString);
            try
            {
                _connection.Open();
                Console.WriteLine("Connection Opened!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ~Database()
        {
            if (_connection != null)
            {
                try
                {
                    _connection.Close();
                    Console.WriteLine("Connection Closed!");
                }
                catch
                {
                    Console.WriteLine("Connection Closed!");
                }
            }
        }

        private bool IsUserExist(string name)
        {
            var command = new SqlCommand
            {
                CommandText = "SELECT COUNT(*) AS count " +
                              "FROM users " +
                              "WHERE name LIKE '" + name + "'",
                CommandType = CommandType.Text,
                Connection = _connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (int)reader["count"] != 0;
                }
                return false;
            }
        }

        public User FindUser(string name, string password)
        {

            var command = new SqlCommand
            {
                CommandText = "SELECT * " +
                              "FROM users " +
                              "WHERE name LIKE '" + name + "'",
                CommandType = CommandType.Text,
                Connection = _connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (((string)reader["password"]).Equals(password))
                    {
                        return new User
                        {
                            Id = (int)reader["id"],
                            Role = (string)reader["role"],
                            Name = (string)reader["name"],
                            Password = (string)reader["password"]
                        };
                    }

                    throw new Exception("Невірний пароль");
                }
            }

            throw new Exception("Користувач " + name + " не знайдений");
        }

        public User CreateUser(string name, string password)
        {
            if (IsUserExist(name))
            {
                throw new Exception("Користувач " + name + " вже зареєстрований");
            }

            var command = new SqlCommand
            {
                CommandText = "INSERT INTO users(role, name, password) " +
                              "OUTPUT INSERTED.id " +
                              "VALUES ('user', '" + name + "', '" + password + "')",
                CommandType = CommandType.Text,
                Connection = _connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = (int)reader["id"],
                        Role = "user",
                        Name = name,
                        Password = password
                    };
                }
            }

            return null;
        }

        public static Database GetInstance()
        {
            return _instance ?? (_instance = new Database());
        }
    }
}
