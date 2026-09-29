using GVDEditor.Domain.Entities;
using System.Globalization;
using ToolsCore.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;

namespace GVDEditor.Formats;

/// <summary>
/// Texty na tabuliach (TTexts.txt).
/// </summary>
internal static class TTextsFile
{
    /// <summary>
    /// Nainicializuje texty do tabul
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="trains">vlaky</param>
    /// <param name="catalogs">katalogove tabule grafikonu - texty sa na ne odkazuju</param>
    public static List<TableText> Read(string path, IList<Train> trains, IEnumerable<TableCatalog> catalogs)
    {
        var fileTTexts = CombinePath(path, FILE_TTEXTS)!;

        var ttexts = new List<TableText>();

        var ttextsF = new TxtPropsAreasFields(fileTTexts);

        var countt = int.Parse(ttextsF.Get("MAIN", "COUNT"), CultureInfo.InvariantCulture);

        for (var i = 0; i < countt; i++)
        {
            var ttext = new TableText();
            var ti = i + 1;

            var area = $"TEXT_{ti.PadZeros()}";
            ttext.Comment = ttextsF.GetComment(area);

            ttext.Key = ttextsF.Get(area, "KEY").ANSItoUTF();
            ttext.Name = ttextsF.Get(area, "NAME").ANSItoUTF();
            for (var j = 0; j < ParseIntOrDefault(ttextsF.Get(area, "REALIZE_COUNT", false)); j++)
            {
                var realization = new TableTextRealization();
                var tj = j + 1;

                var catname = ttextsF.Get(area, $"REALIZE_{tj.PadZeros()}_CATALOG_KEY").ANSItoUTF();
                foreach (var catalog in catalogs)
                    if (catalog.Key == catname)
                    {
                        realization.Table = catalog;
                        break;
                    }

                if (realization.Table == null)
                {
                    throw new FormatException($"Súbor s textami pre tabule {ttext.Key} obsahuje neexistujúcu katalógovú tabuľu {catname}.");
                }

                var realname = ttextsF.Get(area, $"REALIZE_{tj.PadZeros()}_TYPEITEM_KEY").ANSItoUTF();
                foreach (var item in realization.Table.Items)
                    if (item.Key == realname)
                    {
                        realization.Item = item;
                        break;
                    }

                if (realization.Item == null)
                    throw new FormatException(
                        $"Súbor s textami pre tabule {ttext.Key} obsahuje neexistujúci stĺpec pre realizáciu {realname}, ktorý by mal patriť katalógovej tabuli {realization.Table.Key}.");

                ttext.Realizations.Add(realization);
            }

            for (var j = 0; j < ParseIntOrDefault(ttextsF.Get(area, "COUNT", false)); j++)
            {
                var ttrain = new TableTrain();
                var tj = j + 1;

                var id = ParseIntOrDefault(ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_ID", false));
                foreach (var train in trains)
                    if (train.ID == id)
                        ttrain.Train = train;
                if (ttrain.Train == null)
                    throw new FormatException($"Súbor s textami pre tabule obsahuje ID neexistujúceho vlaku ({id}).");

                ttrain.FontID = ParseIntOrDefault(ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_IDX_FONT", false));
                ttrain.Text = ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_TEXT").ANSItoUTF();

                ttext.Trains.Add(ttrain);
            }

            ttexts.Add(ttext);
        }

        return ttexts;
    }

    /// <summary>
    /// Zapise texty do tabul do suboru
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="ttexts">texty do tabul</param>
    public static void Write(string path, IList<TableText> ttexts)
    {
        var fileTTexts = CombinePath(path, FILE_TTEXTS)!;

        var ttextsF = new TxtPropsAreasFields(fileTTexts, true);

        ttextsF.Set("MAIN", "COUNT", ttexts.Count);

        for (var i = 0; i < ttexts.Count; i++)
        {
            var ttext = ttexts[i];
            var ti = i + 1;

            var area = $"TEXT_{ti.PadZeros()}";

            ttextsF.SetComment(area, ttext.Comment);

            ttextsF.Set(area, "KEY", ttext.Key, WriteType.WriteStringANSI);
            ttextsF.Set(area, "NAME", ttext.Name, WriteType.WriteStringANSI);

            ttextsF.Set(area, "REALIZE_COUNT", ttext.Realizations.Count);
            for (var j = 0; j < ttext.Realizations.Count; j++)
            {
                var realization = ttext.Realizations[j];
                var tj = j + 1;

                ttextsF.Set(area, $"REALIZE_{tj.PadZeros()}_CATALOG_KEY", realization.Table.Key, WriteType.WriteStringANSI);
                ttextsF.Set(area, $"REALIZE_{tj.PadZeros()}_TYPEITEM_KEY", realization.Item.Key, WriteType.WriteStringANSI);
            }

            ttextsF.Set(area, "COUNT", ttext.Trains.Count);
            for (var j = 0; j < ttext.Trains.Count; j++)
            {
                var ttrain = ttext.Trains[j];
                var tj = j + 1;

                ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_ID", ttrain.Train.ID);
                ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_IDX_FONT", ttrain.FontID);
                ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_TEXT", ttrain.Text, WriteType.WriteStringANSI);
            }
        }

        ttextsF.Save();
    }
}
