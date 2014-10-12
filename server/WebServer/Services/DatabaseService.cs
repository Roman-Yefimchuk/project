using System;
using System.Collections.Generic;
using System.Linq;
using Server.Common;
using Server.Core;
using Server.Models;

namespace Server.Services
{
    internal class Utils
    {
        /*        public static Point ToPoint(dynamic voltage)
                {
                    return ToPoint(null, voltage);
                }

                public static Point ToPoint(string id, dynamic voltage)
                {
                    var coordinates = ((object[])voltage).Length;
                    return new Point(id, coordinates);
                }*/
    }

    public class DatabaseService : IRestService
    {
        public string Path
        {
            get { return "/api/database/"; }
        }

        public dynamic ResponderMethod(dynamic request)
        {
            var action = (string)request.action;
            var token = (string)request.token;

            SessionManager sessionManager = SessionManager.GetInstance();

            if (sessionManager.GetSession(token) == null)
            {
                throw new ServerException("Ви не авторизовані");
            }

            Console.WriteLine("DatabaseService: " + action);

            switch (action)
            {
                case "find_all_entity_types":
                    {
                        return EntityType.FindAll();
                    }
                case "find_entity_type_by_id":
                    {
                        var id = (int)request.data["id"];
                        return EntityType.FindById(id);
                    }
                case "find_all_entity_models":
                    {
                        return EntityModel.FindAll();
                    }
                case "find_entity_model_by_id":
                    {
                        var id = (int)request.data["id"];
                        return EntityModel.FindById(id);
                    }
                case "find_all_accessories":
                    {
                        return Accessory.FindAll();
                    }
                case "find_accessory_by_id":
                    {
                        var id = (int)request.data["id"];
                        return Accessory.FindById(id);
                    }
                case "find_all_problems":
                    {
                        return Problem.FindAll();
                    }
                case "find_problem_by_id":
                    {
                        var id = (int)request.data["id"];
                        return Problem.FindById(id);
                    }
                case "find_user_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityModelId = (int)request.data["entityModelId"];
                        var problemId = (int)request.data["problemId"];

                        return UserSolution.Find(entityTypeId, entityModelId, problemId);
                    }
                case "add_user_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityModelId = (int)request.data["entityModelId"];
                        var problemId = (int)request.data["problemId"];
                        var solution = (string)request.data["solution"];

                        UserSolution.Add(entityTypeId, entityModelId, problemId, solution);

                        return new { };
                    }
                case "find_master_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityAccessoryId = (int)request.data["entityAccessoryId"];
                        var dimension = (int)request.data["dimension"];
                        var clustersNumber = (int)request.data["clustersNumber"];

                        var masterSolutions = MasterSolution.FindAll(entityTypeId, entityAccessoryId);

                        var points = new PointCollection(dimension);

                        foreach (var solution in masterSolutions)
                        {
                            var coordinates = new double[dimension];
                            for (int index = 0; index < dimension; index++)
                            {
                                coordinates[index] = solution.Ports[index].VoltageValue;
                            }

                            var point = new Point(solution.Id, coordinates);
                            points.Add(point);
                        }

                        var mainCoordinates = new double[dimension];
                        for (int index = 0; index < dimension; index++)
                        {
                            mainCoordinates[index] = (double)request.data["ports"][index]["value"];
                        }
                        var mainPoint = new Point(-1, mainCoordinates);

                        points.Add(mainPoint);

                        return new
                        {
                            entityTypeId = entityTypeId,
                            entityAccessoryId = entityAccessoryId,
                            entityType = EntityType.FindById(entityTypeId).Name,
                            entityAccessory = Accessory.FindById(entityAccessoryId).Name,
                            ports = request.data["ports"],
                            suggestions = MasterSolution.GetSuggestions(masterSolutions, points, mainPoint, clustersNumber, dimension)
                        };
                    }
                case "add_master_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityAccessoryId = (int)request.data["entityAccessoryId"];
                        var ports = (string)request.data["ports"];
                        var problem = (string)request.data["problem"];
                        var solution = (string)request.data["solution"];

                        MasterSolution.Add(entityTypeId, entityAccessoryId, ports, problem, solution);

                        return new { };
                    }
                default:
                    {
                        throw new ServerException("Невідома дія: " + action);
                    }
            }
        }
    }
}
