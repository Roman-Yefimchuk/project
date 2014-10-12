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
        private string _query;
        private readonly object[] _arguments;

        public string Query
        {
            get { return string.Format(_query, _arguments); }
            set { _query = value; }
        }
        public Execute<T> Execute;

        public QueryExecutor(params object[] arguments)
        {
            _arguments = arguments;
        }
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

        private static readonly IDictionary<string, string> ScriptsCache;

        static Database()
        {
            ScriptsCache = new Dictionary<string, string>();
        }

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
                    return queryExecutor.Execute != null ? queryExecutor.Execute(reader) : default(T);
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

        private static void ExecuteQuery(string query, params object[] arguments)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var command = new SqlCommand
            {
                CommandText = string.Format(query, arguments),
                CommandType = CommandType.Text,
                Connection = connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
            }
        }

        private static string LoadQueryScript(string queryScriptName)
        {
            if (ScriptsCache.ContainsKey(queryScriptName))
            {
                return ScriptsCache[queryScriptName];
            }

            var path = @"..\..\SqlQueries\" + queryScriptName + ".sql";

            var queryScript = ResourcesManager.GetStringFromFile(path);
            ScriptsCache.Add(queryScriptName, queryScript);

            return queryScript;
        }

        private static bool IsUserExist(string name)
        {
            return ExecuteQuery(new QueryExecutor<bool>(name)
            {
                Query = LoadQueryScript("is-user-exist"),
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
            return ExecuteQuery(new QueryExecutor<User>(name)
            {
                Query = LoadQueryScript("find-user"),
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

            return ExecuteQuery(new QueryExecutor<User>(name, password)
            {
                Query = LoadQueryScript("create-user"),
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
                Query = LoadQueryScript("find-all-entity-types"),
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
            return ExecuteQuery(new QueryExecutor<EntityType>(id)
            {
                Query = LoadQueryScript("find-entity-type-by-id"),
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
                Query = LoadQueryScript("find-all-entity-models"),
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
            return ExecuteQuery(new QueryExecutor<EntityModel>(id)
            {
                Query = LoadQueryScript("find-entity-model-by-id"),
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
                Query = LoadQueryScript("find-all-problems"),
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
            return ExecuteQuery(new QueryExecutor<Problem>(id)
            {
                Query = LoadQueryScript("find-problem-by-id"),
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

        public static UserSolution FindUserSolution(int entityTypeId, int entityModelId, int problemId)
        {
            return ExecuteQuery(new QueryExecutor<UserSolution>(entityTypeId, entityModelId, problemId)
            {
                Query = LoadQueryScript("find-user-solution"),
                Execute = (reader) =>
                {
                    var suggestions = new List<string>();

                    while (reader.Read())
                    {
                        var solution = (string)reader["solution"];
                        suggestions.Add(solution);
                    }

                    return new UserSolution(suggestions)
                    {
                        EntityType = FindEntityTypeById(entityTypeId).Name,
                        EntityModel = FindEntityModelById(entityModelId).Model,
                        Problem = FindProblemById(problemId).Description
                    };
                }
            });
        }

        public static void AddUserSolution(int entityTypeId, int entityModelId, int problemId, string solution)
        {
            var query = LoadQueryScript("add-user-solution");
            ExecuteQuery(query, entityTypeId, entityModelId, problemId, solution);
        }

        public static Accessory[] FindAllEntityAccessories()
        {
            return ExecuteQuery(new QueryExecutor<Accessory[]>()
            {
                Query = LoadQueryScript("find-all-entity-accessories"),
                Execute = (reader) =>
                {
                    var accessories = new List<Accessory>();

                    while (reader.Read())
                    {
                        var problem = new Accessory()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            EntityTypes = (string)reader["entityTypes"],
                            Ports = (string)reader["ports"]
                        };

                        accessories.Add(problem);
                    }

                    return accessories.ToArray();
                }
            });
        }

        public static Accessory FindEntityAccessoryById(int id)
        {
            return ExecuteQuery(new QueryExecutor<Accessory>(id)
            {
                Query = LoadQueryScript("find-entity-accessory-by-id"),
                Execute = (reader) =>
                {
                    if (reader.Read())
                    {
                        return new Accessory()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            EntityTypes = (string)reader["entityTypes"],
                            Ports = (string)reader["ports"]
                        };
                    }

                    return null;
                }
            });
        }

        public static MasterSolution[] FindAllMasterSolutions(int entityTypeId, int entityAccessoryId)
        {
            return ExecuteQuery(new QueryExecutor<MasterSolution[]>(entityTypeId, entityAccessoryId)
            {
                Query = LoadQueryScript("find-all-master-solutions"),
                Execute = (reader) =>
                {
                    var solutions = new List<MasterSolution>();

                    while (reader.Read())
                    {
                        var masterSolution = new MasterSolution((string)reader["voltageValues"])
                        {
                            Id = (int)reader["id"],
                            EntityTypeId = (int)reader["entityTypeId"],
                            EntityAccessoryId = (int)reader["entityAccessoryId"],
                            Problem = (string)reader["problem"],
                            Solution = (string)reader["solution"]
                        };
                        solutions.Add(masterSolution);
                    }

                    return solutions.ToArray();
                }
            });
        }

        public static void AddMasterSolution(int entityTypeId, int entityAccessoryId, string ports, string problem, string solution)
        {
            var query = LoadQueryScript("add-master-solution");
            ExecuteQuery(query, entityTypeId, entityAccessoryId, ports, problem, solution);
        }
    }
}
