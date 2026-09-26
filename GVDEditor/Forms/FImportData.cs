using System.Data;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;
using ToolsCore.XML;
using TableFileReader = ToolsCore.Tools.TableFileReader;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Import dát zo súborov resp. schránky.
/// </summary>
public partial class FImportData : Form
{
    private readonly GVDInfo Gvd;
    private DataTable? DataTable;
    private object?[]? firstRow;

    private List<ImportTrainColumnType> selectedColumnTypes = new();

    // naposledy nacitany CSV subor - pri zmene kodovania sa nacita znova
    private string? _lastCsvPath;

    /// <summary>
    ///     Naimportovane vlaky; do grafikonu ich prida hlavne okno.
    /// </summary>
    public List<Train> ImportedTrains { get; private set; } = new();

    /// <summary>
    ///     Ci sa maju existujuce vlaky pred pridanim naimportovanych odstranit.
    /// </summary>
    public bool ReplaceTrains { get; private set; }


    /// <summary>
    ///     Vytvori novy formular typu <see cref="FGlobalSettings"/>.
    /// </summary>
    /// <param name="gvd">aktulne vybrany grafikon.</param>
    public FImportData(GVDInfo gvd)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        Gvd = gvd;
        cbDataType.SelectedIndex = 0;
        cbEncoding.SelectedIndex = 0;

        // predvolene doplnit - nahradenie by zmazalo vsetky vlaky grafikonu
        rbAppend.Checked = true;
    }

    private void bImport_Click(object sender, EventArgs e)
    {
        const string fmtException =
            "Hodnota \"{0}\" na riadku {1}, stĺpec {2} ({3}), nemohla byť prevedená na {4}.";

        if (DataTable == null || DataTable.Rows.Count == 0 || DataTable.Columns.Count == 0)
        {
            Utils.ShowError(Resources.FImportData_Nie_sú_zadané_údaje_pre_import);
            DialogResult = DialogResult.None;
            return;
        }

        var required = ImportTrainColumnType.GetRequiredValues();
        if (!selectedColumnTypes.ContainsAllItems(required))
        {
            var text = new StringBuilder(
                "Nie sú zadané všetky povinné stĺpce pre import. Povinné stĺpce sú:\r\nSmerovanie vlaku a/alebo stanice vlaku,\r\n");
            for (var i = 0; i < required.Count; i++)
                if (i == required.Count - 1)
                    text.Append(required[i] + ".");
                else
                    text.Append(required[i] + ",\r\n");

            Utils.ShowError(text.ToString());
            DialogResult = DialogResult.None;
            return;
        }

        var trains = new List<Train>(DataTable.Rows.Count);

        if (GlobData.Config.DebugModeGUI == DebugMode.AppCrash)
            Deserialize();
        else
            try
            {
                Deserialize();
            }
            catch (Exception ex)
            {
                Utils.ShowError(GlobData.Config.DebugModeGUI == DebugMode.OnlyMessage ? ex.Message : ex.ToString());
                DialogResult = DialogResult.None;
                return;
            }

        ImportedTrains = trains;
        ReplaceTrains = rbRemoveAndInsert.Checked;
        DialogResult = DialogResult.OK;

        void Deserialize()
        {
            for (var i = 0; i < DataTable.Rows.Count; i++)
            {
                var train = new Train { Variant = -1 };

                for (var j = 0; j < selectedColumnTypes.Count; j++)
                {
                    var data = DataTable.Rows[i][j].ToString()!;

                    if (selectedColumnTypes[j] == ImportTrainColumnType.Number)
                    {
                        train.Number = data;
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Type)
                    {
                        foreach (var typ in GlobData.TrainsTypes)
                            if (data == typ.Key)
                                train.Type = typ;

                        if (train.Type == null)
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1, selectedColumnTypes[j], typeof(TrainType)));
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Variant)
                    {
                        // prazdna bunka = vlak bez varianty
                        if (string.IsNullOrWhiteSpace(data))
                        {
                            train.Variant = -1;
                            continue;
                        }

                        if (!int.TryParse(data, out var num))
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1, selectedColumnTypes[j], typeof(int)));

                        train.Variant = num;
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Nazov)
                    {
                        train.Name = data.Trim();
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.DopravcaId)
                    {
                        if (string.IsNullOrEmpty(data))
                        {
                            train.Operator = Operator.None;
                        }
                        else
                        {
                            if (!int.TryParse(data, out var num))
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1,
                                    selectedColumnTypes[j], typeof(Operator)));

                            var oper = Operator.GetFromID(GlobData.Operators, num);

                            if (oper == null)
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1,
                                    selectedColumnTypes[j], typeof(Operator)));

                            train.Operator = oper;
                        }
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.DopravcaName)
                    {
                        if (string.IsNullOrEmpty(data))
                        {
                            train.Operator = Operator.None;
                        }
                        else
                        {
                            var oper = Operator.GetFromName(GlobData.Operators, data);
                            train.Operator = oper ?? throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1,
                                selectedColumnTypes[j], typeof(Operator)));
                        }
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Track)
                    {
                        var trk = Track.GetFromID(GlobData.Tracks, data);
                        train.Track = trk ?? throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1,
                            selectedColumnTypes[j], typeof(Track)));
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.LinkaOdchod)
                    {
                        train.LineDeparture = data.Trim();
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.LinkaPrichod)
                    {
                        train.LineArrival = data.Trim();
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Languages)
                    {
                        var langs = new List<FyzLanguage>();

                        data = data.Trim().Replace(" ", "");
                        var langsArrayS = data.Split(',');

                        foreach (var s in langsArrayS)
                        {
                            var language = FyzLanguage.GetLanguageFromKey(GlobData.LocalLanguages, s);

                            if (language == null)
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, j + 1,
                                    selectedColumnTypes[j], typeof(FyzLanguage)));

                            langs.Add(language);
                        }

                        train.Languages = langs;
                    }
                    else if (selectedColumnTypes[j] == ImportTrainColumnType.Attributes)
                    {
                        train.IsMedzistatny = data.Contains("M");
                        train.IsMiestenkovy = data.Contains("R");
                        train.IsMimoriadny = data.Contains("X");
                        train.IsDialkovy = data.Contains("D");
                        train.IsIbaLozkovy = data.Contains("L");
                        train.IsNizkopodlazny = data.Contains("N");
                        train.IsPrestupovy = data.Contains("P");
                        train.IsPriznakO = data.Contains("O");
                    }
                }

                //stlpce, ktore potrebuju data predchadzajucich nadobudnutych hodnot

                if (selectedColumnTypes.Contains(ImportTrainColumnType.AllStationsID) ||
                    selectedColumnTypes.Contains(ImportTrainColumnType.AllStationsName))
                {
                    var byId = selectedColumnTypes.Contains(ImportTrainColumnType.AllStationsID);
                    var allStations = ReadStations(i, byId ? ImportTrainColumnType.AllStationsID : ImportTrainColumnType.AllStationsName, byId)!;
                    var shortStations = ReadStations(i, byId ? ImportTrainColumnType.StationsShortID : ImportTrainColumnType.StationsShortName, byId);
                    var longStations = ReadStations(i, byId ? ImportTrainColumnType.StationsLongID : ImportTrainColumnType.StationsLongName, byId);

                    var (zo, @do) = BuildRoute(allStations, Gvd.ThisStation.ID, shortStations, longStations);
                    train.StaniceZoSmeru.AddRange(zo);
                    train.StaniceDoSmeru.AddRange(@do);

                    SetSmerovanie();
                }
                else if (selectedColumnTypes.Contains(ImportTrainColumnType.Routing))
                {
                    var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.Routing);
                    var data = DataTable.Rows[i][index].ToString()!;

                    if (!Routing.TryParse(data, out var routing))
                        throw new ArgumentException(string.Format(fmtException, data, i + 1, index + 1,
                            selectedColumnTypes[index], typeof(Routing)));

                    train.Routing = routing;
                }
                else
                {
                    throw new Exception("Chýba smerovanie vlaku alebo stanice vlaku.");
                }

                void SetSmerovanie()
                {
                    if (train.StaniceZoSmeru.Count != 0 && train.StaniceDoSmeru.Count != 0)
                        train.Routing = Routing.Prechadzajuci;
                    else if (train.StaniceZoSmeru.Count != 0)
                        train.Routing = Routing.Konciaci;
                    else if (train.StaniceDoSmeru.Count != 0) train.Routing = Routing.Vychadzajuci;
                    else throw new ArgumentException($"Vlak na riadku {i + 1} nemá v trase žiadnu stanicu okrem tejto.");
                }

                var iPrichod = selectedColumnTypes.IndexOf(ImportTrainColumnType.Prichod);
                var dataPrichod = DataTable.Rows[i][iPrichod].ToString()!;
                var iOdchod = selectedColumnTypes.IndexOf(ImportTrainColumnType.Odchod);
                var dataOdchod = DataTable.Rows[i][iOdchod].ToString()!;

                if (train.Routing == Routing.Prechadzajuci)
                {
                    if (!Utils.TryParseTime(dataPrichod, out var timePrichod))
                        throw new ArgumentException(string.Format(fmtException, dataPrichod, i + 1, iPrichod,
                            selectedColumnTypes[iPrichod], typeof(DateTime)));

                    if (!Utils.TryParseTime(dataOdchod, out var timeOdchod))
                        throw new ArgumentException(string.Format(fmtException, dataOdchod, i + 1, iOdchod,
                            selectedColumnTypes[iOdchod], typeof(DateTime)));

                    train.Arrival = timePrichod;
                    train.Departure = timeOdchod;
                }
                else if (train.Routing == Routing.Vychadzajuci)
                {
                    if (!Utils.TryParseTime(dataOdchod, out var timeOdchod))
                        throw new ArgumentException(string.Format(fmtException, dataOdchod, i + 1, iOdchod,
                            selectedColumnTypes[iOdchod], typeof(DateTime)));

                    train.Departure = timeOdchod;
                }
                else
                {
                    if (!Utils.TryParseTime(dataPrichod, out var timePrichod))
                        throw new ArgumentException(string.Format(fmtException, dataPrichod, i + 1, iPrichod,
                            selectedColumnTypes[iPrichod], typeof(DateTime)));

                    train.Arrival = timePrichod;
                }

                if (selectedColumnTypes.Contains(ImportTrainColumnType.PlatnostOd))
                {
                    var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.PlatnostOd);
                    var data = DataTable.Rows[i][index].ToString()!;

                    if (!Utils.TryParseDateAlts(data, out var date))
                        throw new ArgumentException(string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index],
                            typeof(DateTime)));

                    train.ZaciatokPlatnosti = date;
                }

                if (selectedColumnTypes.Contains(ImportTrainColumnType.PlatnostDo))
                {
                    var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.PlatnostDo);
                    var data = DataTable.Rows[i][index].ToString()!;

                    if (!Utils.TryParseDateAlts(data, out var date))
                        throw new ArgumentException(string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index],
                            typeof(DateTime)));

                    train.KoniecPlatnosti = date;
                }

                train.Operator ??= Operator.None;

                train.Languages ??= new List<FyzLanguage>();

                if (train.ZaciatokPlatnosti == default) train.ZaciatokPlatnosti = Gvd.StartValidTimeTable;

                if (train.KoniecPlatnosti == default) train.KoniecPlatnosti = Gvd.EndValidTimeTable;

                if (selectedColumnTypes.Contains(ImportTrainColumnType.DateRemText))
                {
                    var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.DateRemText);
                    var data = DataTable.Rows[i][index].ToString()!;

                    try
                    {
                        var test = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti);
                        test.TextToBitArray(data);
                    }
                    catch (Exception exception)
                    {
                        throw new ArgumentException(
                            string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index], typeof(DateTime)) + " " +
                            exception.Message);
                    }

                    train.DateLimitText = data;
                }

                if (selectedColumnTypes.Contains(ImportTrainColumnType.DateRemBitArray))
                {
                    var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.DateRemBitArray);
                    var data = DataTable.Rows[i][index].ToString()!;

                    string dateRemText;
                    try
                    {
                        var dateRem = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti, insertMarks: false);
                        dateRemText = dateRem.BitArrayToText(Utils.StringToBitArray(data));
                    }
                    catch (Exception exception)
                    {
                        throw new ArgumentException(
                            string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index], typeof(DateTime)) + " " +
                            exception.Message);
                    }

                    train.DateLimitText = dateRemText;
                }

                trains.Add(train);
            }
        }

        // stanice zo stlpca daneho typu; null, ak stlpec nie je vybrany
        List<Station>? ReadStations(int row, ImportTrainColumnType type, bool byId)
        {
            var index = selectedColumnTypes.IndexOf(type);
            if (index == -1) return null;

            var data = DataTable!.Rows[row][index].ToString()!;
            try
            {
                return byId ? Station.GetStationsFromIDListString(data) : Station.GetStationsFromNameListString(data);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(string.Format(fmtException, data, row + 1, index + 1, type, typeof(Station)) + " " +
                                            exception.Message);
            }
        }
    }

    /// <summary>
    ///     Rozdeli trasu na stanice pred touto stanicou (zo smeru) a za nou (do smeru) a nastavi, v ktorom hlaseni
    ///     sa stanice hlasia.
    /// </summary>
    /// <param name="allStations">vsetky stanice trasy vratane tejto stanice</param>
    /// <param name="homeId">cislo stanice grafikonu</param>
    /// <param name="shortStations">stanice kratkeho hlasenia, <see langword="null" /> = ziadne</param>
    /// <param name="longStations">stanice dlheho hlasenia, <see langword="null" /> = vsetky (ako pri pridani stanice do trasy)</param>
    internal static (List<Station> zo, List<Station> @do) BuildRoute(IReadOnlyList<Station> allStations, string homeId,
        IReadOnlyCollection<Station>? shortStations, IReadOnlyCollection<Station>? longStations)
    {
        var zo = new List<Station>();
        var @do = new List<Station>();
        var afterHome = false;

        foreach (var station in allStations)
        {
            if (!afterHome && station.ID == homeId)
            {
                afterHome = true;
                continue;
            }

            // porovnanie podla cisla - Station je record a porovnaval by aj priznaky hlaseni
            var copy = station with
            {
                IsInShortReport = shortStations?.Any(st => st.ID == station.ID) ?? false,
                IsInLongReport = longStations?.Any(st => st.ID == station.ID) ?? true
            };

            if (afterHome) @do.Add(copy);
            else zo.Add(copy);
        }

        return (zo, @do);
    }

    private void bStorno_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void bClipboard_Click(object sender, EventArgs e)
    {
        if (!Clipboard.ContainsText()) return;

        _lastCsvPath = null;
        LoadText(Clipboard.GetText());
    }

    private void bCSV_Click(object sender, EventArgs e)
    {
        var result = ofDialogCSV.ShowDialog();

        if (result == DialogResult.Cancel) return;

        LoadCsv(ofDialogCSV.FileName);
    }

    private void LoadCsv(string path)
    {
        _lastCsvPath = path;
        var encoding = cbEncoding.SelectedIndex == 1 ? Encodings.Win1250 : Encoding.UTF8;
        LoadText(File.ReadAllText(path, encoding));
    }

    private void LoadText(string text)
    {
        var reader = new CsvStringReader(text, rowsep: DetectSeparator(text));

        if (reader.RowCount == 0) return;

        SetTable(reader);
    }

    /// <summary>
    ///     Oddelovac buniek podla prveho riadku: tabulator (kopia z Excelu), inak bodkociarka, inak ciarka.
    /// </summary>
    internal static char DetectSeparator(string text)
    {
        var end = text.IndexOf('\n');
        var firstLine = end == -1 ? text : text[..end];
        if (firstLine.Contains('\t')) return '\t';
        return firstLine.Contains(';') || !firstLine.Contains(',') ? ';' : ',';
    }

    private void bXLS_Click(object sender, EventArgs e)
    {
        var result = ofDialogXLS.ShowDialog();

        if (result == DialogResult.Cancel) return;

        var reader = new XlsReader(ofDialogXLS.FileName);
        _lastCsvPath = null;

        if (reader.RowCount == 0) return;

        SetTable(reader);
    }

    private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cbEncoding_SelectedIndexChanged(object sender, EventArgs e)
    {
        // kodovanie sa tyka len CSV suboru - schranka aj XLS uz text maju; subor sa nacita znova
        if (_lastCsvPath != null && File.Exists(_lastCsvPath))
            LoadCsv(_lastCsvPath);
    }

    private void cboxFirstHeader_CheckedChanged(object sender, EventArgs e)
    {
        if (DataTable != null && DataTable.Rows.Count != 0)
        {
            if (cboxFirstHeader.Checked)
            {
                firstRow = DataTable.Rows[0].ItemArray;
                selectedColumnTypes.Clear();

                for (var i = 0; i < dgvData.Columns.Count; i++)
                {
                    var type = ImportTrainColumnType.ParseColumnName((string)firstRow[i]!);
                    dgvData.Columns[i].HeaderText = type.Name;
                    selectedColumnTypes.Add(type);
                }

                DataTable.Rows.RemoveAt(0);
            }
            else
            {
                if (firstRow != null)
                {
                    selectedColumnTypes.Clear();

                    for (var i = 0; i < DataTable.Columns.Count; i++)
                    {
                        dgvData.Columns[i].HeaderText = ImportTrainColumnType.None.Name;
                        selectedColumnTypes.Add(ImportTrainColumnType.None);
                    }

                    var row = DataTable.NewRow();
                    row.ItemArray = firstRow;
                    DataTable.Rows.InsertAt(row, 0);
                }
            }
        }
    }

    private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        var index = e.ColumnIndex;

        var fcts = new FColumnTypeSelect();
        var result = fcts.ShowDialog();
        if (result == DialogResult.OK)
        {
            var type = fcts.SelectedType;
            selectedColumnTypes[index] = fcts.SelectedType;

            dgvData.Columns[index].HeaderText = type.Name;
        }
        else if (result == DialogResult.No)
        {
            dgvData.Columns[index].HeaderText = ImportTrainColumnType.None.Name;
            selectedColumnTypes[index] = ImportTrainColumnType.None;
        }
    }

    private void SetTable(TableFileReader reader)
    {
        DataTable = new DataTable();
        selectedColumnTypes = new List<ImportTrainColumnType>();

        for (var i = 0; i < reader.ColumnCount; i++)
        {
            var ct = cboxFirstHeader.Checked ? ImportTrainColumnType.ParseColumnName(reader[0, i]) : ImportTrainColumnType.None;

            var dc = new DataColumn { Caption = ct.Name, DefaultValue = "" };

            selectedColumnTypes.Add(ct);
            DataTable.Columns.Add(dc);
        }

        for (var i = cboxFirstHeader.Checked ? 1 : 0; i < reader.RowCount; i++)
        {
            var row = DataTable.NewRow();
            for (var j = 0; j < reader.ColumnCount; j++) row[j] = reader[i, j];
            DataTable.Rows.Add(row);
        }

        reader.Dispose();

        dgvData.DataSource = null;
        dgvData.DataSource = DataTable;

        for (var i = 0; i < dgvData.Columns.Count; i++)
        {
            var column = dgvData.Columns[i];
            column.HeaderText = DataTable.Columns[i].Caption;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            column.AutoSizeMode = i == dgvData.Columns.Count - 1
                ? DataGridViewAutoSizeColumnMode.Fill
                : DataGridViewAutoSizeColumnMode.AllCells;
        }

        for (var i = 0; i < dgvData.Columns.Count; i++)
            if (i != dgvData.Columns.Count - 1)
            {
                var column = dgvData.Columns[i];

                var widthCol = column.Width;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                column.Width = widthCol;
            }
    }

    private void FImportData_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data == null) return;

        var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
        switch (Path.GetExtension(files[0]).ToLower())
        {
            case ".xls":
            case ".xlsx":
                var readerXLS = new XlsReader(files[0]);
                _lastCsvPath = null;

                if (readerXLS.RowCount == 0) return;
                SetTable(readerXLS);
                break;
            case ".txt":
            case ".csv":
                LoadCsv(files[0]);
                break;
        }
    }

    private void FImportData_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect = e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
    }
}