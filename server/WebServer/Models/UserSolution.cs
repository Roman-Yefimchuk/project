using System.Collections.Generic;

namespace Server.Models
{
    public class UserSolution
    {
        public class Suggestion
        {
            public string Text { get; set; }
            public int Weight { get; set; }
        }

        public class Counter
        {
            public int Count;
        }

        public string EntityType { get; set; }
        public string EntityModel { get; set; }
        public string Problem { get; set; }
        public Suggestion[] Suggestions { get; set; }

        public UserSolution(IEnumerable<string> suggestions)
        {
            var suggestionGroups = new Dictionary<string, Counter>();

            foreach (var suggestion in suggestions)
            {
                if (suggestionGroups.ContainsKey(suggestion))
                {
                    suggestionGroups[suggestion].Count++;
                }
                else
                {
                    var counter = new Counter();
                    counter.Count++;

                    suggestionGroups.Add(suggestion, counter);
                }
            }

            var result = new List<Suggestion>();
            foreach (var key in suggestionGroups.Keys)
            {
                var suggestion = new Suggestion()
                {
                    Text = key,
                    Weight = suggestionGroups[key].Count
                };

                result.Add(suggestion);
            }

            Suggestions = result.ToArray();
        }

        public static UserSolution Find(int entityTypeId, int entityModelId, int problemId)
        {
            return Database.FindUserSolution(entityTypeId, entityModelId, problemId);
        }

        public static void Add(int entityTypeId, int entityModelId, int problemId, string solution)
        {
            Database.AddUserSolution(entityTypeId, entityModelId, problemId, solution);
        }
    }
}
