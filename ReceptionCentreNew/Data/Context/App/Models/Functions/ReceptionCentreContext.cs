using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace ReceptionCentreNew.Data.Context.App;
public partial class ReceptionCentreContext : DbContext
{
    #region Обращения
    /// <summary>
    /// Получение списка обращений
    /// </summary>
    public virtual IEnumerable<DataAppealSelect> FuncDataAppealSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, Guid? in_spr_type_id, Guid? in_SprTypeDifficulty_id, Guid? in_spr_category_id, Guid? in_spr_subject_treatment_id, int? in_spr_status_id)
    {
        NpgsqlParameter param1 = new("@in_spr_employees_id", (object)spr_employee_id ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_date_start", in_date_start);
        NpgsqlParameter param3 = new("@in_date_stop", in_date_stop);
        NpgsqlParameter param4 = new("@in_spr_type_id", (object)in_spr_type_id ?? DBNull.Value);
        NpgsqlParameter param5 = new("@in_SprTypeDifficulty_id", (object)in_SprTypeDifficulty_id ?? DBNull.Value);
        NpgsqlParameter param6 = new("@in_spr_category_id", (object)in_spr_category_id ?? DBNull.Value);
        NpgsqlParameter param7 = new("@in_spr_subject_treatment_id", (object)in_spr_subject_treatment_id ?? DBNull.Value);
        NpgsqlParameter param8 = new("@in_spr_status_id", (object)in_spr_status_id ?? DBNull.Value);
        return Database.SqlQueryRaw<DataAppealSelect>("SELECT * FROM data_appeal_select(@in_spr_employees_id,@in_date_start,@in_date_stop,@in_spr_type_id,@in_SprTypeDifficulty_id,@in_spr_category_id,@in_spr_subject_treatment_id,@in_spr_status_id)", param1, param2, param3, param4, param5, param6, param7, param8).ToArray();
    }
    /// <summary>
    /// Получение информации по обращению
    /// </summary>
    public virtual DataAppealSelect FuncDataAppealInfo(string number)
    {
        NpgsqlParameter param1 = new("@in_number_appeal", number);
        return this.Database.SqlQueryRaw<DataAppealSelect>("SELECT * FROM data_appeal_info(@in_number_appeal)", param1).FirstOrDefault();
    }
    #endregion

    #region Этапы        
    /// <summary>
    /// Получаем список этапов по услуге
    /// </summary>
    public virtual IEnumerable<DataAppealRouteStageSelect> FuncDataAppealRoutesStageSelect(Guid appealId)
    {
        NpgsqlParameter param1 = new("@in_data_appeal_id", appealId);
        return this.Database.SqlQueryRaw<DataAppealRouteStageSelect>("SELECT * FROM data_appeal_routes_stage_select(@in_data_appeal_id)", param1).ToArray();
    }

    /// <summary>
    /// Получаем список следующего этапа по услуге
    /// </summary>
    public virtual IEnumerable<DataAppealRouteStageNext> FuncDataAppealRoutesStageNextSelect(Guid appealId)
    {
        NpgsqlParameter param1 = new("@in_data_appeal_id", appealId);
        return this.Database.SqlQueryRaw<DataAppealRouteStageNext>("SELECT * FROM data_appeal_routes_stage_next_select(@in_data_appeal_id)", param1).ToArray();
    }
    #endregion

    #region Списки
    /// <summary>
    /// Получение списка писем
    /// </summary>
    public virtual IEnumerable<DataAppealEmailSelect> FuncDataAppealEmailSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected)
    {
        NpgsqlParameter param1 = new("@in_date_start", in_date_start);
        NpgsqlParameter param2 = new("@in_date_stop", in_date_stop);
        NpgsqlParameter param3 = new("@in_is_connected", (object)in_is_connected ?? DBNull.Value);
        NpgsqlParameter param4 = new("@in_spr_employees_id", (object)spr_employee_id ?? DBNull.Value);
        NpgsqlParameter param5 = new("@in_email_type_id", (object)in_email_type_id ?? DBNull.Value);
        return Database.SqlQueryRaw<DataAppealEmailSelect>("SELECT * FROM data_appeal_email_select(@in_date_start, @in_date_stop,@in_is_connected, @in_spr_employees_id, @in_email_type_id)", param1, param2, param3, param4, param5).ToArray();
    }
    /// <summary>
    /// Получение списка звонков
    /// </summary>
    public virtual IEnumerable<DataAppealCallSelect> FuncDataAppealCallSelect(Guid? spr_employee_id, DateTime? in_date_start, DateTime? in_date_stop, short? in_email_type_id, short? in_is_connected)
    {
        NpgsqlParameter param1 = new("@in_date_start", in_date_start);
        NpgsqlParameter param2 = new("@in_date_stop", in_date_stop);
        NpgsqlParameter param3 = new("@in_is_connected", (object)in_is_connected ?? DBNull.Value);
        NpgsqlParameter param4 = new("@in_spr_employees_id", (object)spr_employee_id ?? DBNull.Value);
        NpgsqlParameter param5 = new("@in_email_type_id", (object)in_email_type_id ?? DBNull.Value);
        return this.Database.SqlQueryRaw<DataAppealCallSelect>("SELECT * FROM data_appeal_call_select(@in_date_start, @in_date_stop,@in_is_connected, @in_spr_employees_id, @in_email_type_id)", param1, param2, param3, param4, param5).ToArray();
    }
    #endregion

    #region Графики
    public virtual IEnumerable<ChartInWeek> FuncChartInWeek() =>
         Database.SqlQueryRaw<ChartInWeek>("SELECT * FROM chart_in_week()").ToArray();

    public virtual IEnumerable<ChartInYear> FuncChartInYear() =>
         Database.SqlQueryRaw<ChartInYear>("SELECT * FROM chart_in_year()").ToArray();
    public virtual IEnumerable<ChartClaimInWeek> FuncChartClaimInWeek() =>
         Database.SqlQueryRaw<ChartClaimInWeek>("SELECT * FROM chart_claim_in_week()").ToArray();
    public virtual IEnumerable<ChartClaimInYear> FuncChartClaimInYear() =>
         Database.SqlQueryRaw<ChartClaimInYear>("SELECT * FROM chart_claim_in_year()").ToArray();
    public virtual IEnumerable<ChartClaimForMfc> FuncChartClaimForMfc() =>
        Database.SqlQueryRaw<ChartClaimForMfc>("SELECT * FROM chart_claim_for_mfc()").ToArray();

    /// <summary>
    /// Статистика по количеству обращений
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppeal> FuncStatisticsDataAppeal(Guid? SprMfcId, Guid? spr_treatment_id, Guid? SprCategoryId, Guid? SprTypeId, Guid? SprTypeDifficultyId)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_spr_subject_treatment_id", (object)spr_treatment_id ?? DBNull.Value);
        NpgsqlParameter param3 = new("@in_spr_category_id", (object)SprCategoryId ?? DBNull.Value);
        NpgsqlParameter param4 = new("@in_spr_type_id", (object)SprTypeId ?? DBNull.Value);
        NpgsqlParameter param5 = new("@in_SprTypeDifficulty_id", (object)SprTypeDifficultyId ?? DBNull.Value);
        return Database.SqlQueryRaw<StatisticsDataAppeal>("SELECT * FROM statistics_data_appeal(@in_spr_mfc_id, @in_spr_subject_treatment_id,@in_spr_category_id, @in_spr_type_id, @in_SprTypeDifficulty_id)", param1, param2, param3, param4, param5).ToArray();
    }

    /// <summary>
    /// Статистика звонков
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppealCall> FuncStatisticsDataAppealCall() =>
        Database.SqlQueryRaw<StatisticsDataAppealCall>("SELECT * FROM statistics_DataAppealCall()").ToArray(); 

    /// <summary>
    /// Статистика по категориям
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppealCategory> FuncStatisticsDataAppealCategory(Guid? SprMfcId, DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_date_start", DateStart);
        NpgsqlParameter param3 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<StatisticsDataAppealCategory>("SELECT * FROM statistics_data_appeal_category(@in_spr_mfc_id, @in_date_start, @in_date_stop)", param1, param2, param3).ToArray();
    }

    /// <summary>
    /// Статистика по предмету
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppealSubject> FuncStatisticsDataAppealSubject(Guid? SprMfcId, DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_date_start", DateStart);
        NpgsqlParameter param3 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<StatisticsDataAppealSubject>("SELECT * FROM statistics_data_appeal_subject(@in_spr_mfc_id, @in_date_start,@in_date_stop)", param1, param2, param3).ToArray();
    }

    /// <summary>
    /// Статистика по типу
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppealType> FuncStatisticsDataAppealType(Guid? SprMfcId, DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_date_start", DateStart);
        NpgsqlParameter param3 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<StatisticsDataAppealType>("SELECT * FROM statistics_data_appeal_type(@in_spr_mfc_id, @in_date_start,@in_date_stop)", param1, param2, param3).ToArray();
    }

    /// <summary>
    /// Статистика по типу сложности
    /// </summary>
    public virtual IEnumerable<StatisticsDataAppealTypeDifficulty> FuncStatisticsDataAppealTypeDifficulty(Guid? SprMfcId, DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        NpgsqlParameter param2 = new("@in_date_start", DateStart);
        NpgsqlParameter param3 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<StatisticsDataAppealTypeDifficulty>("SELECT * FROM statistics_data_appeal_type_difficulty(@in_spr_mfc_id, @in_date_start,@in_date_stop)", param1, param2, param3).ToArray();
    }

    /// <summary>
    /// Отчет по категориям
    /// </summary>
    public virtual IEnumerable<ReportCategory> FuncReportCategory(DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_date_start", DateStart);
        NpgsqlParameter param2 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<ReportCategory>("SELECT * FROM report_category(@in_date_start, @in_date_stop)", param1, param2).ToArray();
    }

    /// <summary>
    /// Отчет по предметам обращения
    /// </summary>
    public virtual IEnumerable<ReportTreatment> FuncReportTreatment(DateTime DateStart, DateTime date_stop)
    {
        NpgsqlParameter param1 = new("@in_date_start", DateStart);
        NpgsqlParameter param2 = new("@in_date_stop", date_stop);
        return this.Database.SqlQueryRaw<ReportTreatment>("SELECT * FROM report_treatment(@in_date_start, @in_date_stop)", param1, param2).ToArray();
    }
    #endregion

    #region System
    public bool FuncDeleteDublicationCall()
    {
        try
        {
            this.Database.ExecuteSqlRaw("SELECT * from delete_dublication_call_run()");
            return true;
        }
        catch (Exception ex)
        {
            string m = ex.Message;
            return false;
        }

    }
    #endregion

    #region Api
    public virtual DataAppealClaimStatistics FuncDataAppealClaimStatistics(Guid? SprMfcId)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        return Database.SqlQueryRaw<DataAppealClaimStatistics>("SELECT * FROM data_appeal_claim_statistics(@in_spr_mfc_id)", param1).FirstOrDefault();
    }
    public virtual IEnumerable<DataAppealClaimWeek> FuncDataAppealClaimWeek(Guid? SprMfcId)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        return Database.SqlQueryRaw<DataAppealClaimWeek>("SELECT * FROM data_appeal_claim_week(@in_spr_mfc_id)", param1).ToArray();
    }
    public virtual IEnumerable<DataAppealClaimYear> FuncDataAppealClaimYear(Guid? SprMfcId)
    {
        NpgsqlParameter param1 = new("@in_spr_mfc_id", (object)SprMfcId ?? DBNull.Value);
        return Database.SqlQueryRaw<DataAppealClaimYear>("SELECT * FROM data_appeal_claim_year(@in_spr_mfc_id)", param1).ToArray();
    }
    #endregion
}