using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Capella.LandelijkeTabellen.Landentabel
{
    [DataContract]
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Land
    {
        private string DebuggerDisplay =>
            $"{{{nameof(Land)}: [{LandCode}] {Omschrijving} ['{DatumIngang?.ToString("yyyy-MM-dd") ?? "-"}' - '{DatumEinde?.ToString("yyyy-MM-dd") ?? "-"}']}}";

        public string LandCode { get; set; }
        public string Omschrijving { get; set; }
        public DateTime? DatumIngang { get; set; }
        public DateTime? DatumEinde { get; set; }
        public bool? IsDatumEindeFictief { get; set; }
    }
}
