namespace GVDEditor.Entities
{
    /// <summary>
    ///     Spojenie vlastností <see cref="DirList" /> a <see cref="GVDInfo" /> s ďalšími vlastnosťami
    ///     (používa sa len pre potreby GUI)
    /// </summary>
    public sealed class GVDDirectory
    {
        /// <summary>
        ///     Vytvori novu instanciu triedy <see cref="GVDDirectory"/>.
        /// </summary>
        /// <param name="dir">informácie o priečinku s grafikonom</param>
        /// <param name="gvd">informácie grafiku</param>
        public GVDDirectory(DirList dir, GVDInfo gvd)
        {
            Period = gvd.StartValidTimeTable.Year + "/" + gvd.EndValidTimeTable.Year;
            Dir = dir;
            GVD = gvd;
        }

        /// <summary>
        ///     Vrati obdobie platnosti ako text (napr. 2020/2021).
        /// </summary>
        public string Period { get; }

        /// <summary>
        ///     Formátovaný reťazec obdobia (názov stanice + obdobie)
        /// </summary>
        public string PeriodFormatted => GVD.ThisStation.Name + " " + Period;

        /// <summary>
        ///     Priečinok s dátami grafikonu
        /// </summary>
        public DirList Dir { get; }

        /// <summary>
        ///     Informácie o grafikone
        /// </summary>
        public GVDInfo GVD { get; }

        /// <inheritdoc />
        public override string ToString() => Period;
    }
}