using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace ReceptionCentreNew.Data.Context.App.Abstract;
public interface IRepository
{
    IQueryable<SprCategory> SprCategory { get; }
    IQueryable<SprEmployees> SprEmployees { get; }
    IQueryable<SprEmployeesDepartment> SprEmployeesDepartment { get; }
    IQueryable<SprEmployeesJobPos> SprEmployeesJobPos { get; }
    IQueryable<SprEmployeesRole> SprEmployeesRole { get; }
    IQueryable<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; }
    IQueryable<SprNotification> SprNotification { get; }
    IQueryable<SprRoutesStage> SprRoutesStage { get; }
    IQueryable<SprRoutesStageNext> SprRoutesStageNext { get; }
    IQueryable<SprSetting> SprSetting { get; }
    IQueryable<SprStatus> SprStatus { get; }
    IQueryable<SprSubjectTreatment> SprSubjectTreatment { get; }
    IQueryable<SprType> SprType { get; }
    IQueryable<SprTypeDifficulty> SprTypeDifficulty { get; }
    IQueryable<SprEmployeesMessageTemplate> SprEmployeesMessageTemplate { get; }
    IQueryable<SprEmployeesTextAppealTemplate> SprEmployeesTextAppealTemplate { get; }
    IQueryable<SprMfc> SprMfc { get; }
    IQueryable<SprQuestion> SprQuestion { get; }
    IQueryable<SprSurveyQuestion> SprSurveyQuestion { get; }
    IQueryable<SprSurveyAnswer> SprSurveyAnswer { get; }

    IQueryable<DataAppeal> DataAppeal { get; }
    IQueryable<DataAppealCall> DataAppealCall { get; }
    IQueryable<DataAppealCommentt> DataAppealCommentt { get; }
    IQueryable<DataAppealCommenttRecipients> DataAppealCommenttRecipients { get; }
    IQueryable<DataAppealMessage> DataAppealMessage { get; }
    IQueryable<DataAppealEmail> DataAppealEmail { get; }
    IQueryable<DataAppealRoutesStage> DataAppealRoutesStage { get; }
    IQueryable<DataCalendar> DataCalendar { get; }
    IQueryable<DataChangeLog> DataChangeLog { get; }
    IQueryable<DataEmployeesNotification> DataEmployeesNotification { get; }
    //IQueryable<data_incoming_call> DataIncomingCall { get; }
    IQueryable<DataSystemErrors> DataSystemErrors { get; }
    IQueryable<DataQuestion> DataQuestion { get; }
    IQueryable<DataAppealFile> DataAppealFile { get; }
    IQueryable<DataCallback> DataCallback { get; }
    IQueryable<DataCallbackCalls> DataCallbackCalls { get; }

    IEnumerable<DataAppealSelect> FuncDataAppealSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, Guid? in_spr_type_id, Guid? in_SprTypeDifficulty_id, Guid? in_spr_category_id, Guid? in_spr_subject_treatment_id, int? in_spr_status_id);
    DataAppealSelect FuncDataAppealInfo(string number);
    IEnumerable<DataAppealRouteStageNext> FuncDataAppealRouteStageNext(Guid DataAppealId);
    IEnumerable<DataAppealRouteStageSelect> FuncDataAppealRouteStageSelect(Guid DataAppealId);
    IEnumerable<DataAppealEmailSelect> FuncDataAppealEmailSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected);
    IEnumerable<DataAppealCallSelect> FuncDataAppealCallSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected);
    IEnumerable<ChartInYear> FuncChartInYear();
    IEnumerable<ChartInWeek> FuncChartInWeek();
    IEnumerable<ChartClaimInYear> FuncChartClaimInYear();
    IEnumerable<ChartClaimInWeek> FuncChartClaimInWeek();
    IEnumerable<ChartClaimForMfc> FuncChartClaimForMfc();
    //IEnumerable<StatisticsCount> FuncStatisticsCount(Guid? SprMfcId, Guid? SprCategoryId, Guid? SprTypeDifficultyId, int? year, int? month_start, int? month_stop);
    IEnumerable<ReportCategory> FuncReportCategory(DateTime DateStart, DateTime date_stop);
    IEnumerable<ReportTreatment> FuncReportTreatment(DateTime DateStart, DateTime date_stop);
    bool FuncDeleteDublicationCall();
    DataAppealClaimStatistics FuncDataAppealClaimStatistics(Guid? SprMfcId);
    IEnumerable<DataAppealClaimWeek> FuncDataAppealClaimWeek(Guid? SprMfcId);
    IEnumerable<DataAppealClaimYear> FuncDataAppealClaimYear(Guid? SprMfcId);

    IEnumerable<StatisticsDataAppeal> FuncStatisticsDataAppeal(Guid? SprMfcId, Guid? spr_treatment_id, Guid? SprCategoryId, Guid? SprTypeId, Guid? SprTypeDifficultyId);
    IEnumerable<StatisticsDataAppealCall> FuncStatisticsDataAppealCall();
    IEnumerable<StatisticsDataAppealCategory> FuncStatisticsDataAppealCategory(Guid? SprMfcId, DateTime DateStart, DateTime date_stop);
    IEnumerable<StatisticsDataAppealSubject> FuncStatisticsDataAppealSubject(Guid? SprMfcId, DateTime DateStart, DateTime date_stop);
    IEnumerable<StatisticsDataAppealType> FuncStatisticsDataAppealType(Guid? SprMfcId, DateTime DateStart, DateTime date_stop);
    IEnumerable<StatisticsDataAppealTypeDifficulty> FuncStatisticsDataAppealTypeDifficulty(Guid? SprMfcId, DateTime DateStart, DateTime date_stop);


    void Insert<TEntity>(TEntity entity) where TEntity : class;
    void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void Delete<TEntity>(TEntity entity) where TEntity : class;
}