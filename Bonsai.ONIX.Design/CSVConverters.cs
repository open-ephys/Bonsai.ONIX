using TinyCsvParser.Mapping;

namespace Bonsai.ONIX.Design
{
    public class CsvNeuropixelsADCMapping : CsvMapping<NeuropixelsV1ADC>
    {
        public CsvNeuropixelsADCMapping()
            : base()
        {
            // [0] is ADC number
            MapProperty(1, x => x.CompP);
            MapProperty(2, x => x.CompN);
            MapProperty(3, x => x.Slope);
            MapProperty(4, x => x.Coarse);
            MapProperty(5, x => x.Fine);
            MapProperty(6, x => x.Cfix);
            MapProperty(7, x => x.Offset);
            MapProperty(8, x => x.Threshold);

        }
    }

    public class CsvNeuropixelsElectrodeMapping : CsvMapping<NeuropixelsV1GainCorrection>
    {
        public CsvNeuropixelsElectrodeMapping()
            : base()
        {
            // [0] is channel number
            MapProperty(1, x => x.APx50Correction);
            MapProperty(2, x => x.APx125Correction);
            MapProperty(3, x => x.APx250Correction);
            MapProperty(4, x => x.APx500Correction);
            MapProperty(5, x => x.APx1000Correction);
            MapProperty(6, x => x.APx1500Correction);
            MapProperty(7, x => x.APx2000Correction);
            MapProperty(8, x => x.APx3000Correction);
            MapProperty(9, x => x.LFPx50Correction);
            MapProperty(10, x => x.LFPx125Correction);
            MapProperty(11, x => x.LFPx250Correction);
            MapProperty(12, x => x.LFPx500Correction);
            MapProperty(13, x => x.LFPx1000Correction);
            MapProperty(14, x => x.LFPx1500Correction);
            MapProperty(15, x => x.LFPx2000Correction);
            MapProperty(16, x => x.LFPx3000Correction);
        }
    }
}