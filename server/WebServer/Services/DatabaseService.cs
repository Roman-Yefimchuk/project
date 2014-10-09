using Server.Models;

namespace Server.Services
{
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
                case "find_all_problems":
                    {
                        return Problem.FindAll();
                    }
                case "find_problem_by_id":
                    {
                        var id = (int)request.data["id"];
                        return Problem.FindById(id);
                    }
                case "find_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityModelId = (int)request.data["entityModelId"];
                        var problemId = (int)request.data["problemId"];

                        return Solution.Find(entityTypeId, entityModelId, problemId);
                    }
                case "add_solution":
                    {
                        var entityTypeId = (int)request.data["entityTypeId"];
                        var entityModelId = (int)request.data["entityModelId"];
                        var problemId = (int)request.data["problemId"];
                        var solution = (string)request.data["solution"];

                        Database.AddSolution(entityTypeId, entityModelId, problemId, solution);

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
