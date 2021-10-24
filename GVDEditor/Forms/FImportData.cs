using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;
using ToolsCore.XML;
using TableFileReader = ToolsCore.Tools.TableFileReader;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Import dát zo súborov resp. schránky.
    /// </summary>
    public partial class FImportData : Form
    {
        private readonly GVDInfo Gvd;
        private DataTable DataTable;
        private object[] firstRow;

        private List<ImportTrainColumnType> selectedColumnTypes;


        /// <summary>
        ///     Vytvori novy formular typu <see cref="FGlobalSettings"/>.
        /// </summary>
        /// <param name="gvd">aktulne vybrany grafikon.</param>
        public FImportData(GVDInfo gvd)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            Gvd = gvd;
            cbDataType.SelectedIndex = 0;
            cbEncoding.SelectedIndex = 0;
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
                var text =
                    "Nie sú zadané všetky povinné stĺpce pre import. Povinné stĺpce sú:\r\nSmerovanie vlaku a/alebo stanice vlaku,\r\n";
                for (var i = 0; i < required.Count; i++)
                    if (i == required.Count - 1)
                        text += required[i] + ".";
                    else
                        text += required[i] + ",\r\n";

                Utils.ShowError(text);
                DialogResult = DialogResult.None;
                return;
            }

            var trains = new List<Train>(DataTable.Rows.Count);

            if (GlobData.Config.DebugModeGUI == Config.DebugMode.AppCrash)
                Deserialize();
            else
                try
                {
                    Deserialize();
                }
                catch (Exception ex)
                {
                    Utils.ShowError(GlobData.Config.DebugModeGUI == Config.DebugMode.OnlyMessage ? ex.Message : ex.ToString());
                    DialogResult = DialogResult.None;
                    return;
                }

            if (rbRemoveAndInsert.Checked)
            {
                GlobData.Trains.Clear();

                foreach (var train in trains) GlobData.Trains.Add(train);
            }
            else if (rbAppend.Checked)
            {
                foreach (var train in trains) GlobData.Trains.Add(train);
            }

            DialogResult = DialogResult.OK;

            void Deserialize()
            {
                for (var i = 0; i < DataTable.Rows.Count; i++)
                {
                    var train = new Train();

                    for (var j = 0; j < selectedColumnTypes.Count; j++)
                    {
                        var data = DataTable.Rows[i][j].ToString();

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
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, j, selectedColumnTypes[j], typeof(TrainType)));
                        }
                        else if (selectedColumnTypes[j] == ImportTrainColumnType.Variant)
                        {
                            if (!int.TryParse(data, out var num))
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, j, selectedColumnTypes[j], typeof(int)));

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
                                    throw new ArgumentException(string.Format(fmtException, data, i + 1, j,
                                        selectedColumnTypes[j], typeof(Operator)));

                                var oper = Operator.GetFromID(GlobData.Operators, num);

                                if (oper == null)
                                    throw new ArgumentException(string.Format(fmtException, data, i + 1, j,
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
                                train.Operator = oper ?? throw new ArgumentException(string.Format(fmtException, data, i + 1, j,
                                    selectedColumnTypes[j], typeof(Operator)));
                            }
                        }
                        else if (selectedColumnTypes[j] == ImportTrainColumnType.Track)
                        {
                            var trk = Track.GetFromID(GlobData.Tracks, data);
                            train.Track = trk ?? throw new ArgumentException(string.Format(fmtException, data, i + 1, j,
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
                            var langs = new List<Language>();

                            data = data.Trim().Replace(" ", "");
                            var langsArrayS = data.Split(',');

                            foreach (var s in langsArrayS)
                            {
                                var language = Language.GetLanguageFromKey(GlobData.LocalLanguages, s);

                                if (language == null)
                                    throw new ArgumentException(string.Format(fmtException, data, i + 1, j,
                                        selectedColumnTypes[j], typeof(Language)));

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
                        }
                    }

                    //stlpce, ktore potrebuju data predchadzajucich nadobudnutych hodnot

                    if (selectedColumnTypes.Contains(ImportTrainColumnType.AllStationsID))
                    {
                        var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.AllStationsID);
                        var data = DataTable.Rows[i][index].ToString();

                        var allStations = Station.GetStationsFromIDListString(data);

                        var skok = 0;

                        for (var j = 0; j < allStations.Count; j++)
                        {
                            if (allStations[j].ID == Gvd.ThisStation.ID)
                            {
                                skok = j + 1;
                                break;
                            }

                            train.StaniceZoSmeru.Add(allStations[j]);
                        }

                        for (var j = skok; j < allStations.Count; j++) train.StaniceDoSmeru.Add(allStations[j]);

                        if (selectedColumnTypes.Contains(ImportTrainColumnType.StationsShortID))
                        {
                            var indexS = selectedColumnTypes.IndexOf(ImportTrainColumnType.StationsShortID);
                            var dataS = DataTable.Rows[i][indexS].ToString();

                            var stationsS = Station.GetStationsFromIDListString(dataS);

                            foreach (var stZo in train.StaniceZoSmeru.Where(stZo => stationsS.Contains(stZo)))
                                stZo.IsInShortReport = true;

                            foreach (var stDo in train.StaniceDoSmeru.Where(stDo => stationsS.Contains(stDo)))
                                stDo.IsInShortReport = true;
                        }

                        if (selectedColumnTypes.Contains(ImportTrainColumnType.StationsLongID))
                        {
                            var indexL = selectedColumnTypes.IndexOf(ImportTrainColumnType.StationsLongID);
                            var dataL = DataTable.Rows[i][indexL].ToString();

                            var stationsL = Station.GetStationsFromIDListString(dataL);

                            foreach (var stZo in train.StaniceZoSmeru.Where(stZo => stationsL.Contains(stZo)))
                                stZo.IsInLongReport = true;

                            foreach (var stDo in train.StaniceDoSmeru.Where(stDo => stationsL.Contains(stDo)))
                                stDo.IsInLongReport = true;
                        }

                        SetSmerovanie();
                    }
                    else if (selectedColumnTypes.Contains(ImportTrainColumnType.AllStationsName))
                    {
                        var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.AllStationsName);
                        var data = DataTable.Rows[i][index].ToString();

                        List<Station> allStations;
                        try
                        {
                            allStations = Station.GetStationsFromNameListString(data);
                        }
                        catch (Exception exception)
                        {
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, index,
                                                            selectedColumnTypes[index], typeof(Station)) + " " +
                                                        exception.Message);
                        }

                        var skok = 0;

                        for (var j = 0; j < allStations.Count; j++)
                        {
                            if (allStations[j].ID == Gvd.ThisStation.ID)
                            {
                                skok = j + 1;
                                break;
                            }

                            train.StaniceZoSmeru.Add(allStations[j]);
                        }

                        for (var j = skok; j < allStations.Count; j++) train.StaniceDoSmeru.Add(allStations[j]);

                        if (selectedColumnTypes.Contains(ImportTrainColumnType.StationsShortName))
                        {
                            var indexS = selectedColumnTypes.IndexOf(ImportTrainColumnType.StationsShortName);
                            var dataS = DataTable.Rows[i][indexS].ToString();

                            List<Station> stationsS;
                            try
                            {
                                stationsS = Station.GetStationsFromNameListString(dataS);
                            }
                            catch (Exception exception)
                            {
                                throw new ArgumentException(string.Format(fmtException, data, i + 1, index,
                                                                selectedColumnTypes[index], typeof(Station)) + " " +
                                                            exception.Message);
                            }

                            foreach (var stZo in train.StaniceZoSmeru.Where(stZo => stationsS.Contains(stZo)))
                                stZo.IsInShortReport = true;

                            foreach (var stDo in train.StaniceDoSmeru.Where(stDo => stationsS.Contains(stDo)))
                                stDo.IsInShortReport = true;
                        }

                        if (selectedColumnTypes.Contains(ImportTrainColumnType.StationsLongName))
                        {
                            var indexL = selectedColumnTypes.IndexOf(ImportTrainColumnType.StationsLongName);
                            var dataL = DataTable.Rows[i][indexL].ToString();

                            var stationsL = Station.GetStationsFromIDListString(dataL);

                            foreach (var stZo in train.StaniceZoSmeru.Where(stZo => stationsL.Contains(stZo)))
                                stZo.IsInLongReport = true;

                            foreach (var stDo in train.StaniceDoSmeru.Where(stDo => stationsL.Contains(stDo)))
                                stDo.IsInLongReport = true;
                        }

                        if (selectedColumnTypes.Contains(ImportTrainColumnType.StationsShortName))
                        {
                            var indexS = selectedColumnTypes.IndexOf(ImportTrainColumnType.StationsShortName);
                            var dataS = DataTable.Rows[i][indexS].ToString();

                            var stationsS = Station.GetStationsFromIDListString(dataS);

                            foreach (var stZo in train.StaniceZoSmeru.Where(stZo => stationsS.Contains(stZo)))
                                stZo.IsInShortReport = true;

                            foreach (var stDo in train.StaniceDoSmeru.Where(stDo => stationsS.Contains(stDo)))
                                stDo.IsInShortReport = true;
                        }

                        SetSmerovanie();
                    }
                    else if (selectedColumnTypes.Contains(ImportTrainColumnType.Routing))
                    {
                        var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.Routing);
                        var data = DataTable.Rows[i][index].ToString();

                        if (!Routing.TryParse(data, out var routing))
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, index,
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
                    }

                    var iPrichod = selectedColumnTypes.IndexOf(ImportTrainColumnType.Prichod);
                    var dataPrichod = DataTable.Rows[i][iPrichod].ToString();
                    var iOdchod = selectedColumnTypes.IndexOf(ImportTrainColumnType.Odchod);
                    var dataOdchod = DataTable.Rows[i][iOdchod].ToString();

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
                        var data = DataTable.Rows[i][index].ToString();

                        if (!Utils.TryParseDateAlts(data, out var date))
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index],
                                typeof(DateTime)));

                        train.ZaciatokPlatnosti = date;
                    }

                    if (selectedColumnTypes.Contains(ImportTrainColumnType.PlatnostDo))
                    {
                        var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.PlatnostDo);
                        var data = DataTable.Rows[i][index].ToString();

                        if (!Utils.TryParseDateAlts(data, out var date))
                            throw new ArgumentException(string.Format(fmtException, data, i + 1, index, selectedColumnTypes[index],
                                typeof(DateTime)));

                        train.KoniecPlatnosti = date;
                    }

                    train.Operator ??= Operator.None;

                    train.Languages ??= new List<Language>();

                    if (train.ZaciatokPlatnosti == default) train.ZaciatokPlatnosti = Gvd.StartValidTimeTable;

                    if (train.KoniecPlatnosti == default) train.KoniecPlatnosti = Gvd.EndValidTimeTable;

                    if (selectedColumnTypes.Contains(ImportTrainColumnType.DateRemText))
                    {
                        var index = selectedColumnTypes.IndexOf(ImportTrainColumnType.DateRemText);
                        var data = DataTable.Rows[i][index].ToString();

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
                        var data = DataTable.Rows[i][index].ToString();

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
        }

        private void bStorno_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void bClipboard_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText()) return;

            var reader = new CSVStringReader(Clipboard.GetText());

            if (reader.RowCount == 0) return;

            SetTable(reader);
        }

        private void bCSV_Click(object sender, EventArgs e)
        {
            var result = ofDialogCSV.ShowDialog();

            if (result == DialogResult.Cancel) return;

            string text;
            using (var readerFile = new StreamReader(ofDialogCSV.OpenFile()))
            {
                text = readerFile.ReadToEnd();
            }

            var reader = new CSVStringReader(text);

            if (reader.RowCount == 0) return;

            SetTable(reader);
        }

        private void bXLS_Click(object sender, EventArgs e)
        {
            var result = ofDialogXLS.ShowDialog();

            if (result == DialogResult.Cancel) return;

            var reader = new XLSReader(ofDialogXLS.FileName);

            if (reader.RowCount == 0) return;

            SetTable(reader);
        }

        private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataTable == null) return;

            switch (cbEncoding.SelectedIndex)
            {
                case 0:
                {
                    for (var i = 0; i < DataTable.Rows.Count; i++)
                    for (var j = 0; j < DataTable.Columns.Count; j++)
                        DataTable.Rows[i][j] = ((string)DataTable.Rows[i][j]).ANSItoUTF();

                    break;
                }
                case 1:
                {
                    for (var i = 0; i < DataTable.Rows.Count; i++)
                    for (var j = 0; j < DataTable.Columns.Count; j++)
                        DataTable.Rows[i][j] = ((string)DataTable.Rows[i][j]).UTFtoANSI();

                    break;
                }
            }
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
                        var type = ImportTrainColumnType.ParseColumnName((string)firstRow[i]);
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
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(files[0]).ToLower())
            {
                case ".xls":
                case ".xlsx":
                    var readerXLS = new XLSReader(files[0]);

                    if (readerXLS.RowCount == 0) return;
                    SetTable(readerXLS);
                    break;
                case ".txt":
                case ".csv":
                    string text;
                    using (var readerFile = new StreamReader(files[0]))
                    {
                        text = readerFile.ReadToEnd();
                    }

                    var readerCSV = new CSVStringReader(text);

                    if (readerCSV.RowCount == 0) return;

                    SetTable(readerCSV);
                    break;
            }
        }

        private void FImportData_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }
    }
}