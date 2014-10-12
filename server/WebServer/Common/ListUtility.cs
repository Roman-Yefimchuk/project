using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Common
{
    public class ListUtility
    {
        public static List<List<T>> SplitList<T>(List<T> items, int groupCount)
        {
            var allGroups = new List<List<T>>();

            int startIndex = 0;
            var groupLength = (int)Math.Round((double)items.Count / (double)groupCount, 0);
            while (startIndex < items.Count)
            {
                var group = new List<T>();
                group.AddRange(items.GetRange(startIndex, groupLength));
                startIndex += groupLength;

                if (startIndex + groupLength > items.Count)
                {
                    groupLength = items.Count - startIndex;
                }

                allGroups.Add(group);
            }

            if (allGroups.Count > groupCount && allGroups.Count > 2)
            {
                allGroups[allGroups.Count - 2].AddRange(allGroups.Last());
                allGroups.RemoveAt(allGroups.Count - 1);
            }

            return allGroups;
        }
    }
}
