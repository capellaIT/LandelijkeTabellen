using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Capella.LandelijkeTabellen.Landentabel.Data;

namespace Capella.LandelijkeTabellen.Landentabel
{
    public class LandenService
    {
        private static readonly CultureInfo _nlCulture = new CultureInfo("nl-NL");
        private static readonly List<Land> _landen;

        protected static List<Land> Landen = _landen ?? (_landen = LoadAlleLanden());

        public List<Land> GetAllWithHistory(bool includeUnknown = true)
        {
            return Landen
                .Where(x => includeUnknown || x.LandCode != "0000")
                .ToList();
        }

        public virtual List<Land> GetAllAsList(bool includeUnknown = true)
        {
            return GetAllAsList(DateTime.Today, includeUnknown);
        }

        public List<Land> GetAllAsList(DateTime forDate, bool includeUnknown = true)
        {
            return GetLandenInternal(forDate, includeUnknown).ToList();
        }

        public Dictionary<string, string> GetAllAsDictionary(DateTime forDate, bool includeUnknown = true)
        {
            return GetLandenInternal(forDate, includeUnknown)
                .ToDictionary(x => x.LandCode, x => x.Omschrijving);
        }

        public Land GetByCode(int landCode)
        {
            if (landCode < 0 || landCode > 9999) throw new ArgumentOutOfRangeException();
            return GetByCode(landCode.ToString("D4"));
        }

        public Land GetByCode(string landCode)
        {
            if (string.IsNullOrEmpty(landCode)) throw new ArgumentNullException(nameof(landCode));
            return Landen.SingleOrDefault(x => x.LandCode == landCode);
        }

        private IEnumerable<Land> GetLandenInternal(DateTime forDate, bool includeUnknown)
        {
            return Landen
                .Where(x => (includeUnknown || x.LandCode != "0000") 
                            && (x.DatumIngang ?? DateTime.MinValue) <= forDate
                            && (x.DatumEinde ?? DateTime.MaxValue) >= forDate);
        }

        private static List<Land> LoadAlleLanden()
        {
            string landentabel =
                EmbeddedResources.ReadFileAsString("Data.Tabel34 Landentabel (gesorteerd op code).csv");

            var landen = new List<Land>();

            if (!string.IsNullOrEmpty(landentabel))
            {
                var lines = landentabel
                             .Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                             .Skip(1);

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length == 5)
                    {
                        landen.Add(new Land
                        {
                            LandCode = data[0].Trim('"'),
                            Omschrijving = data[1].Trim('"'),
                            DatumIngang = GetDate(data[2].Trim('"')),
                            DatumEinde = GetDate(data[3].Trim('"')),
                            IsDatumEindeFictief = !string.IsNullOrEmpty(data[4].Trim('"'))
                        });
                    }
                }
            }

            return landen;
        }

        private static DateTime? GetDate(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            if (value.Length != 8) throw new ArgumentOutOfRangeException();
            DateTime date = DateTime.ParseExact(value, "yyyyMMdd", _nlCulture);
            return date;
        }
    }
}
