using Microsoft.EntityFrameworkCore;

namespace ReceptionCentreNew.Data.Context.App;

public partial class ReceptionCentreContext : DbContext
{
    public ReceptionCentreContext(DbContextOptions<ReceptionCentreContext> options)
        : base(options)
    {
    }

    public ReceptionCentreContext()
    {
    }

    public virtual DbSet<DataAppeal> DataAppeal { get; set; }

    public virtual DbSet<DataAppealCall> DataAppealCall { get; set; }

    public virtual DbSet<DataAppealCommentt> DataAppealCommentt { get; set; }

    public virtual DbSet<DataAppealCommenttRecipients> DataAppealCommenttRecipients { get; set; }

    public virtual DbSet<DataAppealEmail> DataAppealEmail { get; set; }

    public virtual DbSet<DataAppealFile> DataAppealFile { get; set; }

    public virtual DbSet<DataAppealMessage> DataAppealMessage { get; set; }

    public virtual DbSet<DataAppealRoutesStage> DataAppealRoutesStage { get; set; }

    public virtual DbSet<DataCalendar> DataCalendar { get; set; }

    public virtual DbSet<DataCallback> DataCallback { get; set; }

    public virtual DbSet<DataCallbackCalls> DataCallbackCalls { get; set; }

    public virtual DbSet<DataChangeLog> DataChangeLog { get; set; }

    public virtual DbSet<DataEmployeesNotification> DataEmployeesNotification { get; set; }

    public virtual DbSet<DataQuestion> DataQuestion { get; set; }

    public virtual DbSet<DataSurvey> DataSurvey { get; set; }

    public virtual DbSet<DataSystemErrors> DataSystemErrors { get; set; }

    public virtual DbSet<SprCategory> SprCategory { get; set; }

    public virtual DbSet<SprEmployees> SprEmployees { get; set; }

    public virtual DbSet<SprEmployeesDepartment> SprEmployeesDepartment { get; set; }

    public virtual DbSet<SprEmployeesJobPos> SprEmployeesJobPos { get; set; }

    public virtual DbSet<SprEmployeesMessageTemplate> SprEmployeesMessageTemplate { get; set; }

    public virtual DbSet<SprEmployeesRole> SprEmployeesRole { get; set; }

    public virtual DbSet<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; set; }

    public virtual DbSet<SprEmployeesTextAppealTemplate> SprEmployeesTextAppealTemplate { get; set; }

    public virtual DbSet<SprMfc> SprMfc { get; set; }

    public virtual DbSet<SprNotification> SprNotification { get; set; }

    public virtual DbSet<SprQuestion> SprQuestion { get; set; }

    public virtual DbSet<SprRoutesStage> SprRoutesStage { get; set; }

    public virtual DbSet<SprRoutesStageNext> SprRoutesStageNext { get; set; }

    public virtual DbSet<SprSetting> SprSetting { get; set; }

    public virtual DbSet<SprStatus> SprStatus { get; set; }

    public virtual DbSet<SprSubjectTreatment> SprSubjectTreatment { get; set; }

    public virtual DbSet<SprSurveyAnswer> SprSurveyAnswer { get; set; }

    public virtual DbSet<SprSurveyQuestion> SprSurveyQuestion { get; set; }

    public virtual DbSet<SprType> SprType { get; set; }

    public virtual DbSet<SprTypeDifficulty> SprTypeDifficulty { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=192.168.34.5;User Id=postgres;Password=!ShamiL7;Port=5432;Database=reception_centre;CommandTimeout=1000;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
         
        modelBuilder.Entity<DataAppeal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_pkey");

            entity.ToTable("data_appeal", tb => tb.HasComment("Обращение"));

            entity.HasIndex(e => e.Id, "data_appeal_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesIdExecution, "data_appeal_idx10");

            entity.HasIndex(e => e.NumberAppeal, "data_appeal_idx11").IsUnique();

            entity.HasIndex(e => e.SprMfcId, "data_appeal_idx12");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_idx2");

            entity.HasIndex(e => e.SprTypeId, "data_appeal_idx3");

            entity.HasIndex(e => e.SprSubjectTreatmentId, "data_appeal_idx4");

            entity.HasIndex(e => e.SprTypeDifficultyId, "data_appeal_idx5");

            entity.HasIndex(e => e.SprCategoryId, "data_appeal_idx6");

            entity.HasIndex(e => e.SprStatusId, "data_appeal_idx7");

            entity.HasIndex(e => e.SprRoutesStageIdCurrent, "data_appeal_idx8");

            entity.HasIndex(e => e.SprEmployeesIdCurrent, "data_appeal_idx9");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.ApplicantName)
                .HasMaxLength(70)
                .HasComment("ФИО заявителя")
                .HasColumnName("applicant_name");
            entity.Property(e => e.CaseNumber)
                .HasMaxLength(70)
                .HasComment("номер обращения в МФЦ")
                .HasColumnName("case_number");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.CountDay)
                .HasComment("Время обработки, дни")
                .HasColumnName("count_day");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateExecution)
                .HasComment("дата и время исполнения")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_execution");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.DateRegulation)
                .HasComment("Регламентный срок")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_regulation");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .HasComment("Электронная почта")
                .HasColumnName("email_");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.NumberAppeal)
                .HasMaxLength(30)
                .HasComment("номер обращения")
                .HasColumnName("number_appeal");
            entity.Property(e => e.PhoneNumber)
                .HasComment("Номер телефона")
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.SprCategoryId)
                .HasComment("Категория")
                .HasColumnName("spr_category_id");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprEmployeesIdCurrent)
                .HasComment("текущий сотрудник")
                .HasColumnName("spr_employees_id_current");
            entity.Property(e => e.SprEmployeesIdExecution)
                .HasComment("исполнитель")
                .HasColumnName("spr_employees_id_execution");
            entity.Property(e => e.SprMfcId)
                .HasComment("Наименование мфц")
                .HasColumnName("spr_mfc_id");
            entity.Property(e => e.SprRoutesStageIdCurrent)
                .HasDefaultValue(1)
                .HasComment("текущий этап")
                .HasColumnName("spr_routes_stage_id_current");
            entity.Property(e => e.SprStatusId)
                .HasDefaultValue(0)
                .HasComment("Статус")
                .HasColumnName("spr_status_id");
            entity.Property(e => e.SprSubjectTreatmentId)
                .HasComment("Предмет обращения")
                .HasColumnName("spr_subject_treatment_id");
            entity.Property(e => e.SprTypeDifficultyId)
                .HasComment("Сложность")
                .HasColumnName("spr_type_difficulty_id");
            entity.Property(e => e.SprTypeId)
                .HasComment("Тип")
                .HasColumnName("spr_type_id");
            entity.Property(e => e.TextAppeal)
                .HasComment("Текст обращения")
                .HasColumnName("text_appeal");

            entity.HasOne(d => d.SprCategory).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_category_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealSprEmployees)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_employees_id_fkey");

            entity.HasOne(d => d.SprEmployeesIdCurrentNavigation).WithMany(p => p.DataAppealSprEmployeesIdCurrentNavigation)
                .HasForeignKey(d => d.SprEmployeesIdCurrent)
                .HasConstraintName("data_appeal_spr_employees_id_current_fkey");

            entity.HasOne(d => d.SprEmployeesIdExecutionNavigation).WithMany(p => p.DataAppealSprEmployeesIdExecutionNavigation)
                .HasForeignKey(d => d.SprEmployeesIdExecution)
                .HasConstraintName("data_appeal_spr_employees_id_execution_fkey");

            entity.HasOne(d => d.SprMfc).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprMfcId)
                .HasConstraintName("data_appeal_spr_mfc_id_fkey");

            entity.HasOne(d => d.SprRoutesStageIdCurrentNavigation).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprRoutesStageIdCurrent)
                .HasConstraintName("data_appeal_spr_routes_stage_id_current_fkey");

            entity.HasOne(d => d.SprStatus).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_status_id_fkey");

            entity.HasOne(d => d.SprSubjectTreatment).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprSubjectTreatmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_subject_treatment_id_fkey");

            entity.HasOne(d => d.SprTypeDifficulty).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprTypeDifficultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_type_difficulty_id_fkey");

            entity.HasOne(d => d.SprType).WithMany(p => p.DataAppeal)
                .HasForeignKey(d => d.SprTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_spr_type_id_fkey");
        });

        modelBuilder.Entity<DataAppealCall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_call_pkey");

            entity.ToTable("data_appeal_call", tb => tb.HasComment("Звонки"));

            entity.HasIndex(e => e.Id, "data_appeal_call_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_call_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_call_idx3");

            entity.HasIndex(e => e.CallType, "data_appeal_call_idx4");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CallType)
                .HasComment("тип звонка (1-исходящий 2-входящий)")
                .HasColumnName("call_type");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateCall)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время звонка")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_call");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("ФИО сотрудника")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.IsChecked)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_checked");
            entity.Property(e => e.PeerId)
                .HasComment("id звонка jitsi")
                .HasColumnType("character varying")
                .HasColumnName("peer_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasComment("Номер телефона")
                .HasColumnName("phone_number");
            entity.Property(e => e.SaveFtp)
                .HasDefaultValue(false)
                .HasComment("Признак сохранения на ftp")
                .HasColumnName("save_ftp");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.TimeTalk)
                .HasMaxLength(10)
                .HasComment("Время разговора")
                .HasColumnName("time_talk");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealCall)
                .HasForeignKey(d => d.DataAppealId)
                .HasConstraintName("data_appeal_call_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealCall)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_appeal_call_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealCommentt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_commentt_pkey");

            entity.ToTable("data_appeal_commentt", tb => tb.HasComment("Примечания"));

            entity.HasIndex(e => e.Id, "data_appeal_commentt_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_commentt_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_commentt_idx3");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasComment("Текст примечания")
                .HasColumnName("commentt");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealCommentt)
                .HasForeignKey(d => d.DataAppealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_commentt_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealCommentt)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_commentt_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealCommenttRecipients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_commentt_recipients_pkey");

            entity.ToTable("data_appeal_commentt_recipients", tb => tb.HasComment("Получатели примечаний"));

            entity.HasIndex(e => e.Id, "data_appeal_commentt_recipients_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealCommenttId, "data_appeal_commentt_recipients_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_commentt_recipients_idx3");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.DataAppealCommenttId)
                .HasComment("Примечание")
                .HasColumnName("data_appeal_commentt_id");
            entity.Property(e => e.ReadDate)
                .HasComment("Дата и время прочтения")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("read_date");
            entity.Property(e => e.ReadMark)
                .HasComment("Отметка о прочтении")
                .HasColumnName("read_mark");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");

            entity.HasOne(d => d.DataAppealCommentt).WithMany(p => p.DataAppealCommenttRecipients)
                .HasForeignKey(d => d.DataAppealCommenttId)
                .HasConstraintName("data_appeal_commentt_recipients_data_appeal_commentt_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealCommenttRecipients)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_commentt_recipients_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_email_pkey");

            entity.ToTable("data_appeal_email", tb => tb.HasComment("Электронная почта"));

            entity.HasIndex(e => e.Id, "data_appeal_email_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_email_idx2");

            entity.HasIndex(e => e.Email, "data_appeal_email_idx3");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_email_idx4");

            entity.HasIndex(e => e.EmailType, "data_appeal_email_idx5");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Caption)
                .HasComment("Заголовок")
                .HasColumnName("caption");
            entity.Property(e => e.DataAppealId)
                .HasComment("id обращения")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateEmail)
                .HasDefaultValueSql("now()")
                .HasComment("дата и время письма")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_email");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasComment("электронная почта")
                .HasColumnName("email_");
            entity.Property(e => e.EmailType)
                .HasComment("1 исходящий 2 входящий")
                .HasColumnName("email_type");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(50)
                .HasComment("фио сотрудника")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasComment("false - не прочитано, true - прочитано")
                .HasColumnName("is_read");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("id сотрудника")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.TextEmail)
                .HasComment("Текст письма")
                .HasColumnName("text_email");
            entity.Property(e => e.Uids)
                .HasMaxLength(25)
                .HasComment("идентификатор сообщения на почте")
                .HasColumnName("uids_");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealEmail)
                .HasForeignKey(d => d.DataAppealId)
                .HasConstraintName("data_appeal_email_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealEmail)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_appeal_email_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_file_pkey");

            entity.ToTable("data_appeal_file", tb => tb.HasComment("Файлы к обращению"));

            entity.HasIndex(e => e.Id, "data_appeal_file_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_file_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_file_idx3");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.FileExt)
                .HasMaxLength(10)
                .HasComment("расширение файла")
                .HasColumnName("file_ext");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasComment("имя файла")
                .HasColumnName("file_name");
            entity.Property(e => e.FileSize)
                .HasComment("размер файла")
                .HasColumnName("file_size");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealFile)
                .HasForeignKey(d => d.DataAppealId)
                .HasConstraintName("data_appeal_file_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealFile)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_appeal_file_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_message_pkey");

            entity.ToTable("data_appeal_message", tb => tb.HasComment("Сообщения заявителям"));

            entity.HasIndex(e => e.Id, "data_appeal_message_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_message_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_message_idx3");

            entity.HasIndex(e => e.DateMessage, "data_appeal_message_idx4");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateMessage)
                .HasDefaultValueSql("now()")
                .HasComment("дата и время добавления записи, отправки сообщения")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_message");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("фио сотрудника")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.MessageType)
                .HasComment("тип сообщения (1-исходящий 2-входящий)")
                .HasColumnName("message_type");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(70)
                .HasComment("номер телефона")
                .HasColumnName("phone_number");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.TextMessage)
                .HasComment("текст сообщения")
                .HasColumnName("text_message");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealMessage)
                .HasForeignKey(d => d.DataAppealId)
                .HasConstraintName("data_appeal_message_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealMessage)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_appeal_message_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataAppealRoutesStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_appeal_routes_stage_pkey");

            entity.ToTable("data_appeal_routes_stage", tb => tb.HasComment("Этапы обращения"));

            entity.HasIndex(e => e.Id, "data_appeal_routes_stage_idx1").IsUnique();

            entity.HasIndex(e => e.DataAppealId, "data_appeal_routes_stage_idx2");

            entity.HasIndex(e => e.SprRoutesStageId, "data_appeal_routes_stage_idx3");

            entity.HasIndex(e => e.DateStart, "data_appeal_routes_stage_idx4");

            entity.HasIndex(e => e.DateStop, "data_appeal_routes_stage_idx6");

            entity.HasIndex(e => e.SprEmployeesId, "data_appeal_routes_stage_idx8");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(255)
                .HasComment("Комментарий")
                .HasColumnName("commentt");
            entity.Property(e => e.CountDayFact)
                .HasComment("фактическое количество дней")
                .HasColumnType("character varying")
                .HasColumnName("count_day_fact");
            entity.Property(e => e.CountDayRegulation)
                .HasComment("Регламентное количество дней")
                .HasColumnName("count_day_regulation");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateRegulation)
                .HasComment("Регламентная дата")
                .HasColumnName("date_regulation");
            entity.Property(e => e.DateStart)
                .HasDefaultValueSql("now()")
                .HasComment("Дата начала")
                .HasColumnName("date_start");
            entity.Property(e => e.DateStop)
                .HasComment("Дата окончания")
                .HasColumnName("date_stop");
            entity.Property(e => e.EmployeesName)
                .HasMaxLength(70)
                .HasComment("фио сотрудника")
                .HasColumnName("employees_name");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprRoutesStageId)
                .HasComment("Этап")
                .HasColumnName("spr_routes_stage_id");
            entity.Property(e => e.TimeStart)
                .HasPrecision(0)
                .HasDefaultValueSql("now()")
                .HasComment("Время начала")
                .HasColumnName("time_start");
            entity.Property(e => e.TimeStop)
                .HasPrecision(0)
                .HasComment("Время окончания")
                .HasColumnName("time_stop");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataAppealRoutesStage)
                .HasForeignKey(d => d.DataAppealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_routes_stage_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataAppealRoutesStage)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_routes_stage_spr_employees_id_fkey");

            entity.HasOne(d => d.SprRoutesStage).WithMany(p => p.DataAppealRoutesStage)
                .HasForeignKey(d => d.SprRoutesStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_appeal_routes_stage_spr_routes_stage_id_fkey");
        });

        modelBuilder.Entity<DataCalendar>(entity =>
        {
            entity.HasKey(e => e.Date).HasName("data_calendar_pkey");

            entity.ToTable("data_calendar", tb => tb.HasComment("Календарь"));

            entity.HasIndex(e => e.Date, "data_calendar_idx1").IsUnique();

            entity.HasIndex(e => e.DateType, "data_calendar_idx2");

            entity.Property(e => e.Date)
                .HasComment("Первичный ключ (дата)")
                .HasColumnName("date_");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.DateType)
                .HasComment("Тип дня, 1 - рабочие день, 0 - (Суббота Воскресенье), 2 (Праздничный день)")
                .HasColumnName("date_type");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
        });

        modelBuilder.Entity<DataCallback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_callback_pkey");

            entity.ToTable("data_callback", tb => tb.HasComment("Таблица хранения заказных звонков"));

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CallbackId)
                .HasComment("ID звонка")
                .HasColumnName("callback_id");
            entity.Property(e => e.CountTry)
                .HasComment("Количество попыток")
                .HasColumnName("count_try");
            entity.Property(e => e.CustomerFio)
                .HasMaxLength(255)
                .HasComment("Владелец номера телефона")
                .HasColumnName("customer_fio");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время занесения записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateClose)
                .HasComment("Дата закрытия")
                .HasColumnName("date_close");
            entity.Property(e => e.DateOrder)
                .HasComment("Дата на которую заказан звонок")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("date_order");
            entity.Property(e => e.IsHand)
                .HasDefaultValue(false)
                .HasComment("В ручную?")
                .HasColumnName("is_hand");
            entity.Property(e => e.IsSync)
                .HasDefaultValue(false)
                .HasComment("Полное занесение звонка  в базу")
                .HasColumnName("is_sync");
            entity.Property(e => e.PhoneNumber)
                .HasComment("Номер телефона заявителя")
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprEmployeesIdClose)
                .HasComment("Сотрудник закрывший")
                .HasColumnName("spr_employees_id_close");
            entity.Property(e => e.Status)
                .HasComment("Статус звонка (1 - Новый звонок, 2 - Обработан, 3 - Не отвеченный)")
                .HasColumnName("status");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataCallbackSprEmployees)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_callback_spr_employees_id_fkey");

            entity.HasOne(d => d.SprEmployeesIdCloseNavigation).WithMany(p => p.DataCallbackSprEmployeesIdCloseNavigation)
                .HasForeignKey(d => d.SprEmployeesIdClose)
                .HasConstraintName("data_callback_spr_employees_id_close_fkey");
        });

        modelBuilder.Entity<DataCallbackCalls>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_callback_calls_pkey");

            entity.ToTable("data_callback_calls", tb => tb.HasComment("Звонки"));

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CallDuration)
                .HasMaxLength(20)
                .HasComment("Продолжительность звонка")
                .HasColumnName("call_duration");
            entity.Property(e => e.DataCallbackId)
                .HasComment("Связь с заказом звонка")
                .HasColumnName("data_callback_id");
            entity.Property(e => e.DateCall)
                .HasComment("Дата звонка")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("date_call");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(255)
                .HasComment("МФЦ в котором принят звонок")
                .HasColumnName("department_name");
            entity.Property(e => e.EmployeeFio)
                .HasMaxLength(255)
                .HasComment("Сотрудник принявший звонок")
                .HasColumnName("employee_fio");
            entity.Property(e => e.SaveFtp)
                .HasDefaultValue(false)
                .HasComment("Признак сохранения звонка на FTP")
                .HasColumnName("save_ftp");
            entity.Property(e => e.SprEmployeesDepartmentId)
                .HasComment("Связь с МФЦ в котором принят звонок")
                .HasColumnName("spr_employees_department_id");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Связь с сотрудником принявшем звонок")
                .HasColumnName("spr_employees_id");

            entity.HasOne(d => d.DataCallback).WithMany(p => p.DataCallbackCalls)
                .HasForeignKey(d => d.DataCallbackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_callback_calls_data_callback_id_fkey");

            entity.HasOne(d => d.SprEmployeesDepartment).WithMany(p => p.DataCallbackCalls)
                .HasForeignKey(d => d.SprEmployeesDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_callback_calls_spr_employees_department_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataCallbackCalls)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_callback_calls_spr_employees_id_fkey");
        });

        modelBuilder.Entity<DataChangeLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_change_log_pkey");

            entity.ToTable("data_change_log", tb => tb.HasComment("История изменений"));

            entity.HasIndex(e => e.Id, "data_change_log_idx1").IsUnique();

            entity.HasIndex(e => e.RowId, "data_change_log_idx2");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(1500)
                .HasComment("комментарий")
                .HasColumnName("commentt");
            entity.Property(e => e.DateChange)
                .HasDefaultValueSql("now()")
                .HasComment("дата и время изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_change");
            entity.Property(e => e.EmployeesName)
                .HasMaxLength(70)
                .HasComment("сторудник")
                .HasColumnName("employees_name");
            entity.Property(e => e.FieldName)
                .HasMaxLength(70)
                .HasComment("наименование поля")
                .HasColumnName("field_name_");
            entity.Property(e => e.FieldNameBase)
                .HasComment("наименование поля в базе")
                .HasColumnType("character varying")
                .HasColumnName("field_name_base_");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(20)
                .HasComment("ip адрес")
                .HasColumnName("ip_address");
            entity.Property(e => e.NewValue)
                .HasMaxLength(5000)
                .HasComment("новое значение")
                .HasColumnName("new_value");
            entity.Property(e => e.OldValue)
                .HasMaxLength(5000)
                .HasComment("старое значение")
                .HasColumnName("old_value");
            entity.Property(e => e.RowId).HasColumnName("row_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(70)
                .HasComment("наименование таблицы")
                .HasColumnName("table_name_");
            entity.Property(e => e.TableNameBase)
                .HasMaxLength(70)
                .HasComment("наименование таблицы в базе")
                .HasColumnName("table_name_base_");
        });

        modelBuilder.Entity<DataEmployeesNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_employees_notification_pkey");

            entity.ToTable("data_employees_notification", tb => tb.HasComment("Уведомления сотрудником"));

            entity.HasIndex(e => e.Id, "data_employees_notification_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesId, "data_employees_notification_idx2");

            entity.HasIndex(e => e.SprNotificationId, "data_employees_notification_idx3");

            entity.HasIndex(e => e.DataAppealId, "data_employees_notification_idx4");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.DataAppealId)
                .HasComment("Обращение")
                .HasColumnName("data_appeal_id");
            entity.Property(e => e.DateReceipt)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время поступления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_receipt");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("Активно/неактивно")
                .HasColumnName("is_active");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprNotificationId)
                .HasComment("Тип уведомления")
                .HasColumnName("spr_notification_id");

            entity.HasOne(d => d.DataAppeal).WithMany(p => p.DataEmployeesNotification)
                .HasForeignKey(d => d.DataAppealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_employees_notification_data_appeal_id_fkey");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataEmployeesNotification)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_employees_notification_spr_employees_id_fkey");

            entity.HasOne(d => d.SprNotification).WithMany(p => p.DataEmployeesNotification)
                .HasForeignKey(d => d.SprNotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_employees_notification_spr_notification_id_fkey");
        });

        modelBuilder.Entity<DataQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_question_pkey");

            entity.ToTable("data_question", tb => tb.HasComment("Ответы клиентов"));

            entity.HasIndex(e => e.Id, "data_question_idx1").IsUnique();

            entity.HasIndex(e => e.SprQuestionId, "data_question_idx2");

            entity.HasIndex(e => e.SprEmployeesId, "data_question_idx3");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.DateQuestion)
                .HasComment("Дата ответа")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_question");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(255)
                .HasComment("Кто добавил")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasComment("Номер ответа")
                .HasColumnName("phone_number");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprQuestionId)
                .HasComment("Справочник консультаций")
                .HasColumnName("spr_question_id");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataQuestion)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_question_spr_employees_id_fkey");

            entity.HasOne(d => d.SprQuestion).WithMany(p => p.DataQuestion)
                .HasForeignKey(d => d.SprQuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_question_spr_question_id_fkey");
        });

        modelBuilder.Entity<DataSurvey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_servey_pkey");

            entity.ToTable("data_survey", tb => tb.HasComment("Вопросы клиентов"));

            entity.HasIndex(e => e.Id, "data_survey_idx1").IsUnique();

            entity.HasIndex(e => e.SprSurveyQuestionId, "data_survey_idx2");

            entity.HasIndex(e => e.SprSurveyAnswerId, "data_survey_idx3");

            entity.HasIndex(e => e.SprEmployeesId, "data_survey_idx4");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasMaxLength(255)
                .HasComment("Текст ответа")
                .HasColumnName("answer");
            entity.Property(e => e.DateSurvey)
                .HasComment("Дата опроса")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_survey");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasComment("Номер телефона")
                .HasColumnName("phone_number");
            entity.Property(e => e.Question)
                .HasMaxLength(255)
                .HasComment("Текст вопроса")
                .HasColumnName("question");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("Сотрудник")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprSurveyAnswerId)
                .HasComment("Ответ")
                .HasColumnName("spr_survey_answer_id");
            entity.Property(e => e.SprSurveyQuestionId)
                .HasComment("Вопрос")
                .HasColumnName("spr_survey_question_id");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.DataSurvey)
                .HasForeignKey(d => d.SprEmployeesId)
                .HasConstraintName("data_survey_spr_employees_id_fkey");

            entity.HasOne(d => d.SprSurveyAnswer).WithMany(p => p.DataSurvey)
                .HasForeignKey(d => d.SprSurveyAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_survey_spr_survey_answer_id_fkey");

            entity.HasOne(d => d.SprSurveyQuestion).WithMany(p => p.DataSurvey)
                .HasForeignKey(d => d.SprSurveyQuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_survey_spr_survey_question_id_fkey");
        });

        modelBuilder.Entity<DataSystemErrors>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("data_system_errors_pkey");

            entity.ToTable("data_system_errors", tb => tb.HasComment("Системные ошибки"));

            entity.HasIndex(e => e.Id, "data_system_errors_idx1");

            entity.HasIndex(e => e.SprEmployeesId, "data_system_errors_idx2");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.EmployeesName)
                .HasMaxLength(255)
                .HasComment("ФИО сотрудника")
                .HasColumnName("employees_name");
            entity.Property(e => e.ErrorDate)
                .HasDefaultValueSql("now()")
                .HasComment("дата ошибки")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("error_date");
            entity.Property(e => e.ErrorInnerException)
                .HasComment("внутреняя ошибка")
                .HasColumnName("error_inner_exception");
            entity.Property(e => e.ErrorMessage)
                .HasComment("текст ошибки")
                .HasColumnName("error_message");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("сотрудник, связь с spr_employees id")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.StackTrace)
                .HasComment("трассировка стека")
                .HasColumnName("stack_trace");
        });

        modelBuilder.Entity<SprCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_category_pkey");

            entity.ToTable("spr_category", tb => tb.HasComment("Категории"));

            entity.HasIndex(e => e.Id, "spr_category_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .HasComment("Наименование категории")
                .HasColumnName("category_name");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .HasComment("Краткое сокращение для номера обращения")
                .HasColumnName("short_name");
            entity.Property(e => e.Sort)
                .HasComment("Поле для сортировки")
                .HasColumnName("sort");
        });

        modelBuilder.Entity<SprEmployees>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_pkey");

            entity.ToTable("spr_employees", tb => tb.HasComment("Сотрудники"));

            entity.HasIndex(e => e.Id, "spr_employees_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesDepartmentId, "spr_employees_idx2");

            entity.HasIndex(e => e.SprEmployeesJobPosId, "spr_employees_idx3");

            entity.HasIndex(e => e.EmployeesLogin, "spr_employees_idx4").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CanTakeAppeal)
                .HasDefaultValue(false)
                .HasComment("может принимать дела")
                .HasColumnName("can_take_appeal");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesActive)
                .HasComment("Активность сотрудника")
                .HasColumnName("employees_active");
            entity.Property(e => e.EmployeesLogin)
                .HasMaxLength(20)
                .HasComment("Логин сотрудника")
                .HasColumnName("employees_login");
            entity.Property(e => e.EmployeesName)
                .HasMaxLength(70)
                .HasComment("ФИО сотрудника")
                .HasColumnName("employees_name");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.EmployeesPass)
                .HasMaxLength(255)
                .HasComment("Пароль")
                .HasColumnName("employees_pass");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.SprEmployeesDepartmentId)
                .HasComment("Отдел")
                .HasColumnName("spr_employees_department_id");
            entity.Property(e => e.SprEmployeesJobPosId)
                .HasComment("Должность")
                .HasColumnName("spr_employees_job_pos_id");

            entity.HasOne(d => d.SprEmployeesDepartment).WithMany(p => p.SprEmployees)
                .HasForeignKey(d => d.SprEmployeesDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_spr_employees_department_id_fkey");

            entity.HasOne(d => d.SprEmployeesJobPos).WithMany(p => p.SprEmployees)
                .HasForeignKey(d => d.SprEmployeesJobPosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_spr_employees_job_pos_id_fkey");
        });

        modelBuilder.Entity<SprEmployeesDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_department_pkey");

            entity.ToTable("spr_employees_department", tb => tb.HasComment("Отдел"));

            entity.HasIndex(e => e.Id, "spr_employees_department_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(70)
                .HasComment("Наименование отдела")
                .HasColumnName("department_name");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
        });

        modelBuilder.Entity<SprEmployeesJobPos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_job_pos_pkey");

            entity.ToTable("spr_employees_job_pos", tb => tb.HasComment("Должность"));

            entity.HasIndex(e => e.Id, "spr_employees_job_pos_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.JobPosName)
                .HasMaxLength(70)
                .HasComment("Наименование должности")
                .HasColumnName("job_pos_name");
        });

        modelBuilder.Entity<SprEmployeesMessageTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_massage_temp_pkey");

            entity.ToTable("spr_employees_message_template", tb => tb.HasComment("Шаблоны сообщений по сотрудникам"));

            entity.HasIndex(e => e.Id, "spr_employees_message_template_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesId, "spr_employees_message_template_idx2");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasComment("дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("дата и время изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(255)
                .HasComment("кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего запись")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего запись")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("признак удаления записи f - не удалена,t - удалена")
                .HasColumnName("is_remove");
            entity.Property(e => e.MessageText)
                .HasMaxLength(1000)
                .HasComment("текст сообщения")
                .HasColumnName("message_text");
            entity.Property(e => e.Sort)
                .HasDefaultValue(1)
                .HasComment("Приоритет")
                .HasColumnName("sort");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("связь с сотрудником?spr_employees  id")
                .HasColumnName("spr_employees_id");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.SprEmployeesMessageTemplate)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_message_template_spr_employees_id_fkey");
        });

        modelBuilder.Entity<SprEmployeesRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_role_pkey");

            entity.ToTable("spr_employees_role", tb => tb.HasComment("Роли"));

            entity.HasIndex(e => e.Id, "spr_employees_role_idx1").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(255)
                .HasColumnName("commentt");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasComment("наименование роли")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<SprEmployeesRoleJoin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_role_join_pkey");

            entity.ToTable("spr_employees_role_join", tb => tb.HasComment("Связь ролей и сотрудников"));

            entity.HasIndex(e => e.Id, "spr_employees_role_join_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesId, "spr_employees_role_join_idx2");

            entity.HasIndex(e => e.SprEmployeesRoleId, "spr_employees_role_join_idx3");

            entity.HasIndex(e => e.DateAdd, "spr_employees_role_join_idx4");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(255)
                .HasComment("комментарий")
                .HasColumnName("commentt");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("дата добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(100)
                .HasComment("кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("связь с пользователями, spr_employees id")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.SprEmployeesRoleId)
                .HasComment("связь с ролью, spr_employees_role id")
                .HasColumnName("spr_employees_role_id");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.SprEmployeesRoleJoin)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_role_join_spr_employees_id_fkey");

            entity.HasOne(d => d.SprEmployeesRole).WithMany(p => p.SprEmployeesRoleJoin)
                .HasForeignKey(d => d.SprEmployeesRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_role_join_spr_employees_role_id_fkey");
        });

        modelBuilder.Entity<SprEmployeesTextAppealTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_employees_text_appeal_template_pkey");

            entity.ToTable("spr_employees_text_appeal_template", tb => tb.HasComment("Обращения по сотрудникам"));

            entity.HasIndex(e => e.Id, "spr_employees_text_appeal_template_idx1").IsUnique();

            entity.HasIndex(e => e.SprEmployeesId, "spr_employees_text_appeal_template_idx2");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasComment("дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("дата и время изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(255)
                .HasComment("кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего запись")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего запись")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("признак удаления записи f - не удалена,t - удалена")
                .HasColumnName("is_remove");
            entity.Property(e => e.Sort)
                .HasDefaultValue(1)
                .HasComment("Приоритет")
                .HasColumnName("sort");
            entity.Property(e => e.SprEmployeesId)
                .HasComment("связь с сотрудником?spr_employees  id")
                .HasColumnName("spr_employees_id");
            entity.Property(e => e.TextAppeal)
                .HasMaxLength(1000)
                .HasComment("текст обращения")
                .HasColumnName("text_appeal");

            entity.HasOne(d => d.SprEmployees).WithMany(p => p.SprEmployeesTextAppealTemplate)
                .HasForeignKey(d => d.SprEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_employees_text_appeal_template_spr_employees_id_fkey");
        });

        modelBuilder.Entity<SprMfc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_mfc_pkey");

            entity.ToTable("spr_mfc", tb => tb.HasComment("Справочник мфц"));

            entity.HasIndex(e => e.Id, "spr_mfc_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasComment("дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("дата и время изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(255)
                .HasComment("кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего запись")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего запись")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.MfcName)
                .HasMaxLength(255)
                .HasComment("нименование мфц")
                .HasColumnName("mfc_name");
            entity.Property(e => e.MfcNameSmall)
                .HasMaxLength(100)
                .HasComment("краткое наименование мфц")
                .HasColumnName("mfc_name_small");
        });

        modelBuilder.Entity<SprNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_notification_pkey");

            entity.ToTable("spr_notification", tb => tb.HasComment("Типы уводемлений"));

            entity.HasIndex(e => e.Id, "spr_notification_idx1").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Notification)
                .HasMaxLength(30)
                .HasComment("Наименование типа уведомлений")
                .HasColumnName("notification");
        });

        modelBuilder.Entity<SprQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_consultation_pkey");

            entity.ToTable("spr_question", tb => tb.HasComment("Справочник консультаций"));

            entity.HasIndex(e => e.Id, "spr_consultation_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasComment("Ответ")
                .HasColumnName("answer");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.Question)
                .HasMaxLength(500)
                .HasComment("Название консультации")
                .HasColumnName("question");
        });

        modelBuilder.Entity<SprRoutesStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_routes_stage_pkey");

            entity.ToTable("spr_routes_stage", tb => tb.HasComment("Этапы"));

            entity.HasIndex(e => e.Id, "spr_routes_stage_idx1").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.StageName)
                .HasMaxLength(30)
                .HasComment("Наименование этапа")
                .HasColumnName("stage_name");
        });

        modelBuilder.Entity<SprRoutesStageNext>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_routes_stage_next_pkey");

            entity.ToTable("spr_routes_stage_next", tb => tb.HasComment("Порядок этапов"));

            entity.HasIndex(e => e.Id, "spr_routes_stage_next_idx1").IsUnique();

            entity.HasIndex(e => e.SprRoutesStageId, "spr_routes_stage_next_idx2");

            entity.HasIndex(e => e.SprRoutesStageIdNext, "spr_routes_stage_next_idx3");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.SprRoutesStageId)
                .HasComment("Этап")
                .HasColumnName("spr_routes_stage_id");
            entity.Property(e => e.SprRoutesStageIdNext)
                .HasComment("Следующий этап")
                .HasColumnName("spr_routes_stage_id_next");

            entity.HasOne(d => d.SprRoutesStage).WithMany(p => p.SprRoutesStageNextSprRoutesStage)
                .HasForeignKey(d => d.SprRoutesStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_routes_stage_next_spr_routes_stage_id_fkey");

            entity.HasOne(d => d.SprRoutesStageIdNextNavigation).WithMany(p => p.SprRoutesStageNextSprRoutesStageIdNextNavigation)
                .HasForeignKey(d => d.SprRoutesStageIdNext)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_routes_stage_next_spr_routes_stage_id_next_fkey");
        });

        modelBuilder.Entity<SprSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_setting_pkey");

            entity.ToTable("spr_setting", tb => tb.HasComment("Настройки"));

            entity.HasIndex(e => e.Id, "spr_setting_idx1").IsUnique();

            entity.HasIndex(e => e.ParamName, "spr_setting_idx2");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(255)
                .HasComment("Комментарий")
                .HasColumnName("commentt");
            entity.Property(e => e.ParamName)
                .HasMaxLength(70)
                .HasComment("Переменная")
                .HasColumnName("param_name");
            entity.Property(e => e.ParamValue)
                .HasMaxLength(255)
                .HasComment("Значение переменной")
                .HasColumnName("param_value");
        });

        modelBuilder.Entity<SprStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_status_pkey");

            entity.ToTable("spr_status", tb => tb.HasComment("Статусы"));

            entity.HasIndex(e => e.Id, "spr_status_idx1").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(30)
                .HasComment("Наименование статуса")
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<SprSubjectTreatment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_subject_treatment_pkey");

            entity.ToTable("spr_subject_treatment", tb => tb.HasComment("Предмет обращения"));

            entity.HasIndex(e => e.Id, "spr_subject_treatment_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.Sort)
                .HasComment("Поле для сортировки")
                .HasColumnName("sort");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(30)
                .HasComment("Наименование предмета обращения")
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<SprSurveyAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_survey_answer_pkey");

            entity.ToTable("spr_survey_answer", tb => tb.HasComment("Ответы"));

            entity.HasIndex(e => e.Id, "spr_survey_answer_idx1").IsUnique();

            entity.HasIndex(e => e.SprSurveyQuestionId, "spr_survey_answer_idx2");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasMaxLength(255)
                .HasComment("Текст ответа")
                .HasColumnName("answer");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.RecordNumber)
                .HasComment("Номер")
                .HasColumnName("record_number");
            entity.Property(e => e.SprSurveyQuestionId)
                .HasComment("Вопрос")
                .HasColumnName("spr_survey_question_id");

            entity.HasOne(d => d.SprSurveyQuestion).WithMany(p => p.SprSurveyAnswer)
                .HasForeignKey(d => d.SprSurveyQuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spr_survey_answer_spr_survey_question_id_fkey");
        });

        modelBuilder.Entity<SprSurveyQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_survey_question_pkey");

            entity.ToTable("spr_survey_question", tb => tb.HasComment("Вопросы"));

            entity.HasIndex(e => e.Id, "spr_survey_question_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.Question)
                .HasMaxLength(255)
                .HasComment("Текст вопроса")
                .HasColumnName("question");
        });

        modelBuilder.Entity<SprType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_type_pkey");

            entity.ToTable("spr_type", tb => tb.HasComment("Тип"));

            entity.HasIndex(e => e.Id, "spr_type_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.TypeName)
                .HasMaxLength(30)
                .HasComment("Наименование типа")
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<SprTypeDifficulty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spr_type_difficulty_pkey");

            entity.ToTable("spr_type_difficulty", tb => tb.HasComment("Тип сложности"));

            entity.HasIndex(e => e.Id, "spr_type_difficulty_idx1").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("Первичный ключ")
                .HasColumnName("id");
            entity.Property(e => e.CommenttModify)
                .HasMaxLength(255)
                .HasComment("Комментарий при изменении")
                .HasColumnName("commentt_modify");
            entity.Property(e => e.CountDay)
                .HasComment("Время обработки, дни")
                .HasColumnName("count_day");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateModify)
                .HasComment("Дата и время последних изменений")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("date_modify");
            entity.Property(e => e.EmployeesNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил запись")
                .HasColumnName("employees_name_add");
            entity.Property(e => e.EmployeesNameModify)
                .HasMaxLength(70)
                .HasComment("Кто изменил запись")
                .HasColumnName("employees_name_modify");
            entity.Property(e => e.IpAddressAdd)
                .HasMaxLength(20)
                .HasComment("ip адрес добавившего")
                .HasColumnName("ip_address_add");
            entity.Property(e => e.IpAddressModify)
                .HasMaxLength(20)
                .HasComment("ip адрес изменившего")
                .HasColumnName("ip_address_modify");
            entity.Property(e => e.IsRemove)
                .HasDefaultValue(false)
                .HasComment("Признак удаления")
                .HasColumnName("is_remove");
            entity.Property(e => e.TypeName)
                .HasMaxLength(30)
                .HasComment("Наименование типа сложности")
                .HasColumnName("type_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}