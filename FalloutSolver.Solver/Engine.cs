using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutSolver.Solver
{
    public class Engine
    {
        public class Entry
        {
            public string Text { get; set; } = string.Empty;
            public int? Count { get; set; } = null;

            public Entry(string text, int? count = null)
            {
                Text = text;
                Count = count;
            }
        }

        public IList<Entry> Entries { get; set; } = new List<Entry>();

        static int CommonCharacters(string left, string right) =>
            Enumerable.Zip(left, right).Where((pair) => pair.First == pair.Second).Count();

        public IList<string>? Solve()
        {
            if (Entries.Count == 0)
                return null;

            var haveCount = Entries.Where(e => e.Count is not null).ToList();
            if (haveCount.Count == 0) // need at least one count to solve.
                return null;

            var candidates = Entries.Where(e => e.Count is null).ToList();

            return null;
        }

    }
}
