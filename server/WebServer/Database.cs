using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Server.Models;

namespace Server
{
    delegate T Execute<out T>(SqlDataReader reader);

    class QueryExecutor<T>
    {
        public string Query { get; set; }
        public Execute<T> Execute;
    }

    public static class Database
    {
        private const string ServerName = "ecom.core.sql";
        private const string DbName = "project";
        private const string UserName = "ctes_user";
        private const string Password = "ctes_user";

        private const string ConnectionString = "" +
                                "Data Source=" + ServerName + ";" +
                                "Initial Catalog=" + DbName + ";" +
                                "User ID=" + UserName + ";" +
                                "Password=" + Password + ";" +
                                "";

        private static T ExecuteQuery<T>(QueryExecutor<T> queryExecutor)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var command = new SqlCommand
            {
                CommandText = queryExecutor.Query,
                CommandType = CommandType.Text,
                Connection = connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                try
                {
                    if (queryExecutor.Execute != null)
                    {
                        return queryExecutor.Execute(reader);
                    }

                    return default(T);
                }
                catch (Exception e)
                {
                    if (e is ServerException)
                    {
                        throw;
                    }
                    throw new ServerException("Не вдалося виконати запит до бази даних", e);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void ExecuteQuery(string query)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var command = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
            }
        }

        public static void Create()
        {
            const string path = @"..\..\..\..\db.sql";
            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open);
                var streamReader = new StreamReader(fileStream);
                string query = streamReader.ReadToEnd();

                ExecuteQuery(query);

                Console.WriteLine("Ok");
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        private static bool IsUserExist(string name)
        {
            return ExecuteQuery(new QueryExecutor<bool>()
            {
                Query = "SELECT COUNT(*) AS count " +
                        "FROM users " +
                        "WHERE name LIKE '" + name + "'",
                Execute = (reader) =>
                {
                    if (reader.Read())
                    {
                        return (int)reader["count"] != 0;
                    }
                    return false;
                }
            });
        }

        public static User FindUser(string name, string password)
        {
            return ExecuteQuery(new QueryExecutor<User>()
            {
                Query = "SELECT * " +
                        "FROM users " +
                        "WHERE name LIKE '" + name + "'",
                Execute = (reader) =>
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

                        throw new ServerException("Невірний пароль");
                    }

                    throw new ServerException("Користувач " + name + " не знайдений");
                }
            });
        }

        public static User CreateUser(string name, string password)
        {
            if (IsUserExist(name))
            {
                throw new ServerException("Користувач " + name + " вже зареєстрований");
            }

            return ExecuteQuery(new QueryExecutor<User>()
            {
                Query = "INSERT INTO users(role, name, password) " +
                        "OUTPUT INSERTED.id " +
                        "VALUES ('user', '" + name + "', '" + password + "')",
                Execute = (reader) =>
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

                    throw new ServerException("Не вдалося створити користувача");
                }
            });
        }

        public static EntityType[] FindAllEntityTypes()
        {
            return ExecuteQuery(new QueryExecutor<EntityType[]>()
            {
                Query = "SELECT * " +
                        "FROM entityTypes",
                Execute = (reader) =>
                {
                    var entityTypes = new List<EntityType>();

                    while (reader.Read())
                    {
                        var entityType = new EntityType()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Type = (string)reader["type"]
                        };

                        entityTypes.Add(entityType);
                    }

                    return entityTypes.ToArray();
                }
            });
        }

        public static EntityType FindEntityTypeById(int id)
        {
            return ExecuteQuery(new QueryExecutor<EntityType>()
            {
                Query = "SELECT * " +
                        "FROM entityTypes " +
                        "WHERE id = " + id,
                Execute = (reader) =>
                {
                    if (reader.Read())
                    {
                        return new EntityType()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Type = (string)reader["type"]
                        };
                    }

                    return null;
                }
            });
        }

        public static EntityModel[] FindAllEntityModels()
        {
            return ExecuteQuery(new QueryExecutor<EntityModel[]>()
            {
                Query = "SELECT * " +
                        "FROM entityModels",
                Execute = (reader) =>
                {
                    var entityTypes = new List<EntityModel>();

                    while (reader.Read())
                    {
                        var entityModel = new EntityModel()
                        {
                            Id = (int)reader["id"],
                            Model = (string)reader["model"],
                            EntityTypes = (string)reader["entityTypes"]
                        };

                        entityTypes.Add(entityModel);
                    }

                    return entityTypes.ToArray();
                }
            });
        }

        public static EntityModel FindEntityModelById(int id)
        {
            return ExecuteQuery(new QueryExecutor<EntityModel>()
            {
                Query = "SELECT * " +
                        "FROM entityModels " +
                        "WHERE id = " + id,
                Execute = (reader) =>
                {
                    if (reader.Read())
                    {
                        return new EntityModel()
                        {
                            Id = (int)reader["id"],
                            Model = (string)reader["model"],
                            EntityTypes = (string)reader["entityTypes"]
                        };
                    }

                    return null;
                }
            });
        }

        public static Problem[] FindAllProblems()
        {
            return ExecuteQuery(new QueryExecutor<Problem[]>()
            {
                Query = "SELECT * " +
                        "FROM problems",
                Execute = (reader) =>
                {
                    var problems = new List<Problem>();

                    while (reader.Read())
                    {
                        var problem = new Problem()
                        {
                            Id = (int)reader["id"],
                            Description = (string)reader["description"],
                            EntityTypes = (string)reader["entityTypes"]
                        };

                        problems.Add(problem);
                    }

                    return problems.ToArray();
                }
            });
        }


        public static Problem FindProblemById(int id)
        {
            return ExecuteQuery(new QueryExecutor<Problem>()
            {
                Query = "SELECT * " +
                        "FROM problems " +
                        "WHERE id = " + id,
                Execute = (reader) =>
                {
                    if (reader.Read())
                    {
                        return new Problem()
                        {
                            Id = (int)reader["id"],
                            Description = (string)reader["description"],
                            EntityTypes = (string)reader["entityTypes"]
                        };
                    }

                    return null;
                }
            });
        }

        public static Solution FindSolution(int entityTypeId, int entityModelId, int problemId)
        {
            return ExecuteQuery(new QueryExecutor<Solution>()
            {
                Query = "SELECT * " +
                        "FROM clusters " +
                        "WHERE entityTypeId = " + entityTypeId + " AND entityModelId = " + entityModelId + " AND problemId = " + problemId,
                Execute = (reader) =>
                {
                    var suggestions = new List<string>();

                    while (reader.Read())
                    {
                        var solution = (string)reader["solution"];
                        suggestions.Add(solution);
                    }

                    return new Solution(suggestions)
                    {
                        EntityType = FindEntityTypeById(entityTypeId).Name,
                        EntityModel = FindEntityModelById(entityModelId).Model,
                        Problem = FindProblemById(problemId).Description
                    };
                }
            });
        }

        public static void AddSolution(int entityTypeId, int entityModelId, int problemId, string solution)
        {
            var query = "INSERT INTO clusters(entityTypeId, entityModelId, problemId, solution)" +
                        "VALUES(" + entityTypeId + ", " + entityModelId + ", " + problemId + ", N'" + solution + "')";

            ExecuteQuery(query);
        }
    }
}
