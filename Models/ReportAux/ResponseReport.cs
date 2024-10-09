namespace LegacyTest.Models.ReportAux
{
    public class ResponseReport
    {
        public long IdCompany { get; set; }

        public int RowsPerPage { get; set; }
        public int CardsPerSet { get; set; }
        public FormReport? Form { get; set; }
        public List<ReportScale>? Scales { get; set; }        
        public List <DataMatrix>? Matrix { get; set; }
        public List<CombinedResult>? CombinedResults { get; set; }
    }

    public class ResponseReport2
    {
        public List<FormReport>? Form { get; set; }
        public List<ReportScale>? Scales { get; set; }
        public List<DataMatrix>? Matrix { get; set; }
        public List<CombinedResult>? CombinedResults { get; set; }
    }


    public class FormReport
    {
        public long Id { get; set; }
        public string NameForm { get; set; } = string.Empty;
        public string DescriptionReport { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class EffectReport
    {
        public long Id { get; set; }
        public long IdCharacterization { get; set; }
        public string EffectDescription { get; set; } = string.Empty;
        public string EffectType { get; set; } = string.Empty;
    }   

    public class RecommendationReport
    {
        public long Id { get; set; }
        public long IdCharacterization { get; set; }     
        public string RecommendationDescription { get; set; } = string.Empty;
        public string RecommendationType { get; set; } = string.Empty;
    }


    public class CombinedResult
    {

        public long IdCharacterization { get; set; }
        public string Characterization { get; set; } = string.Empty;
        public List<EffectReport> Effects { get; set; }
        public List<RecommendationReport> Recommendations { get; set; }

    }



    public class DataMatrix
    {
        public string Label { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Characterization { get; set; } = string.Empty;        
        public long Criterion1 { get; set; }
        public long? Criterion2 { get; set; }
    }



}
