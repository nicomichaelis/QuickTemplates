namespace Michaelis.QuickTemplates.Meta;

enum LinePostition { Undefined, Head, PreNamespace, PreClass, Bottom }

class Line : MetaData
{
    public string Text { get; set; }
    public bool Indented { get; set; } = true;
    public LinePostition Position { get; set; } = LinePostition.Undefined;
}
