using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Domain.Concrete;
using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace AisReception.Data.Context.App;
public partial class EFRepository : IRepository
{
    private ReceptionCentreContext _context = new(); 

    public IQueryable<SprCategory> SprCategory => _context.SprCategory.OrderBy(o => o.Sort);
    public IQueryable<SprEmployees> SprEmployees => _context.SprEmployees;
    public IQueryable<SprEmployeesDepartment> SprEmployeesDepartment => _context.SprEmployeesDepartment;
    public IQueryable<SprEmployeesJobPos> SprEmployeesJobPos => _context.SprEmployeesJobPos;
    public IQueryable<SprEmployeesRole> SprEmployeesRole => _context.SprEmployeesRole;
    public IQueryable<SprEmployeesRoleJoin> SprEmployeesRoleJoin => _context.SprEmployeesRoleJoin;
    public IQueryable<SprNotification> SprNotification => _context.SprNotification;
    public IQueryable<SprRoutesStage> SprRoutesStage => _context.SprRoutesStage;
    public IQueryable<SprRoutesStageNext> SprRoutesStageNext => _context.SprRoutesStageNext;
    public IQueryable<SprSetting> SprSetting => _context.SprSetting;
    public IQueryable<SprStatus> SprStatus => _context.SprStatus;
    public IQueryable<SprSubjectTreatment> SprSubjectTreatment => _context.SprSubjectTreatment.OrderBy(o => o.Sort);
    public IQueryable<SprType> SprType => _context.SprType;
    public IQueryable<SprTypeDifficulty> SprTypeDifficulty => _context.SprTypeDifficulty;
    public IQueryable<SprEmployeesMessageTemplate> SprEmployeesMessageTemplate => _context.SprEmployeesMessageTemplate;
    public IQueryable<SprEmployeesTextAppealTemplate> SprEmployeesTextAppealTemplate => _context.SprEmployeesTextAppealTemplate;
    public IQueryable<SprMfc> SprMfc => _context.SprMfc;
    public IQueryable<SprQuestion> SprQuestion => _context.SprQuestion;
    public IQueryable<SprSurveyQuestion> SprSurveyQuestion => _context.SprSurveyQuestion;
    public IQueryable<SprSurveyAnswer> SprSurveyAnswer => _context.SprSurveyAnswer;
     
     
    public IQueryable<DataAppeal> DataAppeal => _context.DataAppeal;
    public IQueryable<DataAppealCall> DataAppealCall => _context.DataAppealCall;
    public IQueryable<DataAppealCommentt> DataAppealCommentt => _context.DataAppealCommentt;
    public IQueryable<DataAppealCommenttRecipients> DataAppealCommenttRecipients => _context.DataAppealCommenttRecipients;
    public IQueryable<DataAppealMessage> DataAppealMessage => _context.DataAppealMessage;
    public IQueryable<DataAppealEmail> DataAppealEmail => _context.DataAppealEmail;
    public IQueryable<DataAppealRoutesStage> DataAppealRoutesStage => _context.DataAppealRoutesStage;
    public IQueryable<DataCalendar> DataCalendar => _context.DataCalendar;
    public IQueryable<DataChangeLog> DataChangeLog => _context.DataChangeLog;
    public IQueryable<DataEmployeesNotification> DataEmployeesNotification => _context.DataEmployeesNotification;
    public IQueryable<DataSystemErrors> DataSystemErrors => _context.DataSystemErrors;
    public IQueryable<DataQuestion> DataQuestion => _context.DataQuestion;
    public IQueryable<DataSurvey> DataSurvey => _context.DataSurvey;
    public IQueryable<DataAppealFile> DataAppealFile => _context.DataAppealFile;
    public IQueryable<DataCallback> DataCallback => _context.DataCallback;
    public IQueryable<DataCallbackCalls> DataCallbackCalls => _context.DataCallbackCalls;
      

    public IEnumerable<DataAppealSelect> FuncDataAppealSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, Guid? in_spr_type_id, Guid? in_spr_type_difficulty_id, Guid? in_spr_category_id, Guid? in_spr_subject_treatment_id, int? in_spr_status_id) => _context.FuncDataAppealSelect(spr_employee_id, in_date_start, in_date_stop, in_spr_type_id, in_spr_type_difficulty_id, in_spr_category_id, in_spr_subject_treatment_id, in_spr_status_id);
    public DataAppealSelect FuncDataAppealInfo(string number) => _context.FuncDataAppealInfo(number);
    public IEnumerable<DataAppealRouteStageNext> FuncDataAppealRouteStageNext(Guid data_appeal_id) => _context.FuncDataAppealRoutesStageNextSelect(data_appeal_id);
    public IEnumerable<DataAppealRouteStageSelect> FuncDataAppealRouteStageSelect(Guid data_appeal_id) => _context.FuncDataAppealRoutesStageSelect(data_appeal_id);
    public IEnumerable<DataAppealEmailSelect> FuncDataAppealEmailSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected) => _context.FuncDataAppealEmailSelect(spr_employee_id, in_date_start, in_date_stop, in_email_type_id, in_is_connected);
    public IEnumerable<DataAppealCallSelect> FuncDataAppealCallSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected) => _context.FuncDataAppealCallSelect(spr_employee_id, in_date_start, in_date_stop, in_email_type_id, in_is_connected);
    public IEnumerable<ChartInYear> FuncChartInYear() => _context.FuncChartInYear();
    public IEnumerable<ChartInWeek> FuncChartInWeek() => _context.FuncChartInWeek();
    public IEnumerable<ChartClaimInYear> FuncChartClaimInYear() => _context.FuncChartClaimInYear();
    public IEnumerable<ChartClaimInWeek> FuncChartClaimInWeek() => _context.FuncChartClaimInWeek();
    public IEnumerable<ChartClaimForMfc> FuncChartClaimForMfc() => _context.FuncChartClaimForMfc();
    public IEnumerable<ReportCategory> FuncReportCategory(DateTime date_start, DateTime date_stop) => _context.FuncReportCategory(date_start, date_stop);
    public IEnumerable<ReportTreatment> FuncReportTreatment(DateTime date_start, DateTime date_stop) => _context.FuncReportTreatment(date_start, date_stop);
    public bool FuncDeleteDublicationCall() => _context.FuncDeleteDublicationCall();

    public DataAppealClaimStatistics FuncDataAppealClaimStatistics(Guid? spr_mfc_id) => _context.FuncDataAppealClaimStatistics(spr_mfc_id);
    public IEnumerable<DataAppealClaimWeek> FuncDataAppealClaimWeek(Guid? spr_mfc_id) => _context.FuncDataAppealClaimWeek(spr_mfc_id);
    public IEnumerable<DataAppealClaimYear> FuncDataAppealClaimYear(Guid? spr_mfc_id) => _context.FuncDataAppealClaimYear(spr_mfc_id);

    public IEnumerable<StatisticsDataAppeal> FuncStatisticsDataAppeal(Guid? spr_mfc_id, Guid? spr_treatment_id, Guid? spr_category_id, Guid? spr_type_id, Guid? spr_type_difficulty_id) => _context.FuncStatisticsDataAppeal(spr_mfc_id, spr_treatment_id, spr_category_id, spr_type_id, spr_type_difficulty_id);
    public IEnumerable<StatisticsDataAppealCall> FuncStatisticsDataAppealCall() => _context.FuncStatisticsDataAppealCall();
    public IEnumerable<StatisticsDataAppealCategory> FuncStatisticsDataAppealCategory(Guid? spr_mfc_id, DateTime date_start, DateTime date_stop) => _context.FuncStatisticsDataAppealCategory(spr_mfc_id, date_start, date_stop);
    public IEnumerable<StatisticsDataAppealSubject> FuncStatisticsDataAppealSubject(Guid? spr_mfc_id, DateTime date_start, DateTime date_stop) => _context.FuncStatisticsDataAppealSubject(spr_mfc_id, date_start, date_stop);
    public IEnumerable<StatisticsDataAppealType> FuncStatisticsDataAppealType(Guid? spr_mfc_id, DateTime date_start, DateTime date_stop) => _context.FuncStatisticsDataAppealType(spr_mfc_id, date_start, date_stop);
    public IEnumerable<StatisticsDataAppealTypeDifficulty> FuncStatisticsDataAppealTypeDifficulty(Guid? spr_mfc_id, DateTime date_start, DateTime date_stop) => _context.FuncStatisticsDataAppealTypeDifficulty(spr_mfc_id, date_start, date_stop);
     

    /// <summary>
    /// Создание универсального метода вставки
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    public void Insert<TEntity>(TEntity entity) where TEntity : class
    { 
        _context.Entry(entity).State = EntityState.Added;
        _context.SaveChanges();
    }

    /// <summary>
    /// Запись нескольких полей в БД
    /// </summary>
    public void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    { 
        foreach (TEntity entity in entities)
            _context.Entry(entity).State = EntityState.Added;
        _context.SaveChanges(); 
    }

    /// <summary>
    /// Универсальный метод обновления
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    public void Update<TEntity>(TEntity entity)
        where TEntity : class
    { 
        _context.Entry(entity).State = EntityState.Modified;

        _context.SaveChanges();
    }

    /// <summary>
    /// Универсальный метод удаления данных
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    public void Delete<TEntity>(TEntity entity)
        where TEntity : class
    {  
        _context.Entry<TEntity>(entity).State = EntityState.Deleted;
        _context.SaveChanges();
    } 
}