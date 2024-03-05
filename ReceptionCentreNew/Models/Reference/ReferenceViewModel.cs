using ReceptionCentreNew.Data.Context.App;

namespace ReceptionCentreNew.Models;
public class ReferenceViewModel
{
    public IEnumerable<SprCategory> SprCaseCategoryList { get; set; }
    public IEnumerable<SprSubjectTreatment> SprCaseSubjectList { get; set; }
    public IEnumerable<SprType> SprCaseTypeList { get; set; }
    public IEnumerable<SprTypeDifficulty> SprCaseTypeDifficultyList { get; set; }
    public IEnumerable<DataChangeLog> DataChangeLogList { get; set; }
    public IEnumerable<DataSystemErrors> ErrorsList { get; set; }
    public IEnumerable<SprQuestion> SprQuestionList { get; set; }
    public IEnumerable<SprSurveyQuestion> SprSurveyQuestionList { get; set; }
    public IEnumerable<SprSurveyAnswer> SprSurveyAnswerList { get; set; }
    public PageInfo PageInfo { get; set; }
}