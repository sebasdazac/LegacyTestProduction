using LegacyTest.Models;
using System.Text.Json.Serialization;

public class CharacterizationByCompany
{
    public long Id { get; set; }
    public long IdCompany { get; set; }
    public long IdPlanCompany { get; set; }
    public long IdForm { get; set; }

    public string? NameForm { get; set; }
    public long IdCharacterization { get; set; }

    public string? Characterization { get; set; }
    public long? IdCriterio1 { get; set; }
    public string? CriterionText1 { get; set; }
    public string? IdClasification1 { get; set; }
    public double? AverageCriterion1 { get; set; }
    public long? IdCriterio2 { get; set; }
    public string? CriterionText2 { get; set; }
    public string? IdClasification2 { get; set; }
    public double? AverageCriterion2 { get; set; }
    public DateTime Date { get; set; }

    [JsonIgnore]
    public virtual CriterionCharacterization? IdCharacterizationNavigation { get; set; }

    public string NameFormValue => NameForm ?? "";
    public string CharacterizationValue => Characterization ?? "";
    public string CriterionText1Value => CriterionText1 ?? "";
    public string IdClasification1Value => IdClasification1 ?? "";
    public string CriterionText2Value => CriterionText2 ?? "";
    public string IdClasification2Value => IdClasification2 ?? "";
    public double AverageCriterion1Value => AverageCriterion1 ?? 0;
    public double AverageCriterion2Value => AverageCriterion2 ?? 0;
    public long IdCriterio1Value => IdCriterio1 ?? 0;
    public long IdCriterio2Value => IdCriterio2 ?? 0;

  
}
