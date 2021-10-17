using System.Collections.Generic;
using TabTabCompiler.Compilation;
using TabTabCompiler.Components.AST;
using TabTabCompiler.Tools;

namespace TabTabCompiler.Components
{
    public class Position : Statement
    {
        public static readonly Position PozV = new ("Poz_V", "Pozícia Vychádzajúci");
        public static readonly Position PozP = new("Poz_P", "Pozícia Prechádzajuci");
        public static readonly Position PozK = new("Poz_K", "Pozícia končiaci");

        public string Name { get; }

        public string Description { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private Position(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public Position Parse(string name)
        {
            if (PozV.Name.EqualsIgnoreCase(name))
                return PozV;
            if (PozP.Name.EqualsIgnoreCase(name))
                return PozP;
            if (PozK.Name.EqualsIgnoreCase(name))
                return PozK;

            return null;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }

    public class Priznak : Statement
    {
        public static readonly Priznak PriznVyl = new("Prizn_Vyl", "Príznak Vlak vo výluke");
        public static readonly Priznak PriznVylP = new("Prizn_VylP", "Príznak Vlak vo výluke na príchode");
        public static readonly Priznak PriznVylO = new("Prizn_VylO", "Príznak Vlak vo výluke na odchode");
        public static readonly Priznak PriznM = new("Prizn_M", "Vlak má príznak Medzištátny");
        public static readonly Priznak PriznO = new("Prizn_O", "Vlak má príznak XXX");
        public static readonly Priznak PriznD = new("Prizn_D", "Vlak má príznak Diaľkový");
        public static readonly Priznak PriznX = new("Prizn_X", "Vlak má príznak Mimoriadny");
        public static readonly Priznak PriznR = new("Prizn_R", "Vlak má príznak Miestenkový");
        public static readonly Priznak PriznL = new("Prizn_L", "Vlak má príznak Iba lôžkový");
        public static readonly Priznak PriznN = new("Prizn_N", "Vlak má príznak Nízkopodlažný");
        public static readonly Priznak PriznPre = new("Prizn_Pre", "Prizn_Pre"); //?

        public string Name { get; }

        public string Description { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private Priznak(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public Priznak Parse(string name)
        {
            if (PriznVyl.Name.EqualsIgnoreCase(name))
                return PriznVyl;
            if (PriznVylP.Name.EqualsIgnoreCase(name))
                return PriznVylP;
            if (PriznVylO.Name.EqualsIgnoreCase(name))
                return PriznVylO;
            if (PriznM.Name.EqualsIgnoreCase(name))
                return PriznM;
            if (PriznO.Name.EqualsIgnoreCase(name))
                return PriznO;
            if (PriznD.Name.EqualsIgnoreCase(name))
                return PriznD;
            if (PriznX.Name.EqualsIgnoreCase(name))
                return PriznX;
            if (PriznR.Name.EqualsIgnoreCase(name))
                return PriznR;
            if (PriznL.Name.EqualsIgnoreCase(name))
                return PriznL;
            if (PriznN.Name.EqualsIgnoreCase(name))
                return PriznN;
            if (PriznPre.Name.EqualsIgnoreCase(name))
                return PriznPre;

            return null;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Name;
    }

    public class State : Statement
    {
        public static readonly State StavOdbaveny = new("Stav_Odbaven", "Stav_Odbaven");
        public static readonly State StavStoji = new("Stav_Stoji", "Stav_Stoji");

        public string Name { get; }

        public string Description { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private State(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public static State Parse(string name)
        {
            if (StavOdbaveny.Name.EqualsIgnoreCase(name))
                return StavOdbaveny;
            if (StavStoji.Name.EqualsIgnoreCase(name))
                return StavStoji;
            return null;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Name;
    }

    public class TrType : Statement
    {
        public static List<TrType> TrTypes { get; private set; }

        /// <summary>
        ///     Train type (pri custom typoch je to X1,...)
        /// </summary>
        public string Key { get; }

        public string Name { get; }

        public string AltName { get; }

        public string Description { get; }

        public bool IsCustom { get; }

        /// <summary>
        ///     Konstruktor (pre zakladne typy vlakov)
        /// </summary>
        /// <param name="key">skratka typu vlaku</param>
        private TrType(string key)
        {
            Name = $"Typ_{key}";
            AltName = null;
            Key = key;
            Description = $"Typ vlaku {key}";
            IsCustom = false;
        }

        /// <summary>
        ///     Konstruktor (pre custom typy vlakov)
        /// </summary>
        /// <param name="key">kluc typu vlaku (pre custom je to X1,...)</param>
        /// <param name="name">skratka custom typu vlaku</param>
        private TrType(string key, string name)
        {
            Name = $"Typ_{name}";
            AltName = $"Typ_{key}";
            Key = key;
            Description = $"Typ vlaku {name}";
            IsCustom = true;
        }

        public static TrType Parse(string text)
        {
            foreach (var trType in TrTypes)
            {
                if (trType.Name == text || trType.AltName == text)
                    return trType;
            }
            return null;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Key;

        public static void Prepare()
        {
            TrTypes = new List<TrType>();
            foreach (var trtype in Compiler.TrainTypes)
            {
                TrTypes.Add(trtype.IsCustom ? new TrType(trtype.Key, trtype.CategoryTrain) : new TrType(trtype.Key));
            }
        }
    }
}
