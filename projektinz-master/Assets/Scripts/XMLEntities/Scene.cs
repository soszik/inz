using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace XMlParser
{
    public class Scene
    {
        public Scene() { }
        public int GroupCount { get; set; }
        public String Type { get; set; }
        public String Name { get; set; }
        public List<Puzzle> Puzzles { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public float PuzzleSize { get; set; }
        public List<Audio> AudioItems { get; set; }
        public List<SmallObject> SmallObjects { get; set; }

    }

}
