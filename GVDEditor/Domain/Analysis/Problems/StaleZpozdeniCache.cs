using System.Globalization;
using GVDEditor.Properties;
using Serilog;

namespace GVDEditor.Domain.Analysis.Problems;

internal class StaleZpozdeniCache : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="StaleZpozdeniCache" /> class.</summary>
    public StaleZpozdeniCache(string cachePath)
    {
        CachePath = cachePath;
    }

    private string CachePath { get; }

    public string Text =>
        string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ZpozdeniCacheOld, GvdFileConsts.FileZpozdeni, GvdFileConsts.FileZpozdeniDat);

    public string Solution => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ZpozdeniCacheOld_Fix, GvdFileConsts.FileZpozdeniDat);

    public bool ChangesGrafikon => false;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        try
        {
            File.Delete(CachePath);
            return FixResult.Done;
        }
        catch (Exception e)
        {
            Log.Warning(e, "Vyrovnavaciu pamat meskani {Path} sa nepodarilo zmazat", CachePath);
            return FixResult.Error;
        }
    }
}
