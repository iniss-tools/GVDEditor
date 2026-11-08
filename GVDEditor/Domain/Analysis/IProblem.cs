namespace GVDEditor.Domain.Analysis;

internal interface IProblem
{
    public string Text { get; }

    public string Solution { get; }

    public ProblemType ProblemType { get; }

    public FixType FixType { get; }

    public FixResult FixProblem();

    /// <summary>
    /// Ci oprava meni grafikon v pamati (a treba ho potom ulozit). Oprava mimo grafikonu, napr. zmazanie
    /// vyrovnavacej pamate INISSu, sa zapise hned.
    /// </summary>
    public bool ChangesGrafikon => true;
}
