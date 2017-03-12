using System.Collections.Generic;
using System.Linq;

namespace Glob
{

    class StringWildcard : GlobNode
    {
        public static readonly StringWildcard Default = new StringWildcard();

        private StringWildcard()
            : base(GlobNodeType.StringWildcard)
        {
        }
    }

    class CharacterWildcard : GlobNode
    {
        public static readonly CharacterWildcard Default = new CharacterWildcard();

        private CharacterWildcard()
            : base(GlobNodeType.CharacterWildcard)
        {
        }
    }

    class DirectoryWildcard : PathSegment
    {
        public static readonly DirectoryWildcard Default = new DirectoryWildcard();

        private DirectoryWildcard()
            : base(GlobNodeType.DirectoryWildcard, new GlobNode[0])
        {
        }
    }

    class Root : PathSegment
    {
        public string Text { get; set; }

        public Root(string text = "")
            : base(GlobNodeType.Root, new GlobNode[0])
        {
            Text = text;
        }
    }

    class CharacterSet : GlobNode
    {
        public bool Inverted { get; set; }
        public Identifier Characters { get; set; }

        public CharacterSet(Identifier characters, bool inverted)
            : base(GlobNodeType.CharacterSet)
        {
            Characters = characters;
            Inverted = inverted;
        }
    }

    class Identifier : GlobNode
    {
        public string Value { get; set; }

        public Identifier(string value) 
            : base(GlobNodeType.Identifier)
        {
            Value = value;
        }
    }

    class LiteralSet : GlobNode
    {
        public IEnumerable<Identifier> Literals { get; set; }

        public LiteralSet(IEnumerable<Identifier> literals) 
            : base(GlobNodeType.Identifier)
        {
            Literals = literals.ToList();
        }
    }

    class PathSegment : GlobNode 
    {
        public IEnumerable<GlobNode> SubSegments { get; set; }

        protected PathSegment(GlobNodeType type, IEnumerable<GlobNode> parts)
            : base(type)
        {
            SubSegments = parts.ToList();
        }

        public PathSegment(IEnumerable<GlobNode> parts) 
            : base(GlobNodeType.PathSegment)
        {
            SubSegments = parts.ToList();
        }
    }

    class Tree : GlobNode
    {
        public IEnumerable<PathSegment> Segments { get; set; }

        public Tree(IEnumerable<PathSegment> segments) 
            : base(GlobNodeType.Tree)
        {
            Segments = segments.ToList();
        }
    }

    class GlobNode 
    {
        protected GlobNode(GlobNodeType type)
        {
            this.Type = type;
        }

        public GlobNodeType Type { get; private set; }
    }

    enum GlobNodeType
    {
        CharacterSet, //string, no children
        Tree, // children
        Identifier, //string
        LiteralSet, //children
        PathSegment, //children
        Root, //string 
        StringWildcard, //string
        CharacterWildcard, //string
        DirectoryWildcard,
    }
}