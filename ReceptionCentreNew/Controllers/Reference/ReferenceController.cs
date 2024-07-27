using ReceptionCentreNew.Models;
using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;
using SmartBreadcrumbs.Attributes;

namespace ReceptionCentreNew.Controllers;

[ClientErrorHandler]
[Authorize(Roles = "superadmin, admin")]
public partial class ReferenceController : Controller
{
    public int PageSize = 10;

    private IRepository _repository;
    private SprEmployees _employee;
    private string? UserName;

    public ReferenceController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        _repository = repo;
        _employee = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name);
        UserName = _employee.EmployeesName;
    }

    // GET: Reference
    public IActionResult Main()
    {
        return View();
    }

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Тип обращения  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
    [Breadcrumb("Справочники \"Тип обращения\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult CaseType()
    {
        return View("CaseType/Main");
    }

    public IActionResult PartialTableCaseType(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var CaseType = _repository.SprType.Where(o => o.IsRemove == isRemove);

        ReferenceViewModel model = new()
        {
            SprCaseTypeList = CaseType.OrderBy(a => a.TypeName),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = CaseType.Count()
            },
        };
        return PartialView("CaseType/PartialTableCaseType", model);
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalAddCaseType()
    {
        return PartialView("CaseType/PartialModalAddCaseType", new SprType { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalEditCaseType(Guid id)
    {
        return PartialView("CaseType/PartialModalEditCaseType", _repository.SprType.SingleOrDefault(st => st.Id == id));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="caseType">объект</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmitCaseTypeSave(SprType caseType)
    {
        if (caseType.Id == Guid.Empty)
        {
            caseType.DateAdd = DateTime.Now;
            _repository.Insert(caseType);
        }
        else
        {
            caseType.EmployeesNameModify = UserName;
            caseType.DateModify = DateTime.Now;
            _repository.Update(caseType);
        }
        return RedirectToAction("PartialTableCaseType");
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns>

    public async Task SubmitCaseTypeRecovery(Guid id)
    {
        SprType recoveryCaseType = _repository.SprType.SingleOrDefault(so => so.Id == id);
        if (recoveryCaseType != null)
        {
            recoveryCaseType.IsRemove = false;
            await _repository.Update(recoveryCaseType);
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="subjectID">Id</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public async Task SubmitCaseTypeDelete(Guid id)
    {
        SprType deleteCaseType = _repository.SprType.SingleOrDefault(so => so.Id == id);
        if (deleteCaseType != null)
        {
            deleteCaseType.IsRemove = true;
            await _repository.Delete(deleteCaseType);
        }
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Тип сложности обращения  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
    [Breadcrumb("Справочники \"Тип сложности обращения\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult CaseTypeDifficulty()
    {
        return View("CaseTypeDifficulty/Main");
    }
    public IActionResult PartialTableCaseTypeDifficulty(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var CaseTypeDifficulty = _repository.SprTypeDifficulty.Where(o => o.IsRemove == isRemove);

        ReferenceViewModel model = new()
        {
            SprCaseTypeDifficultyList = CaseTypeDifficulty.OrderBy(a => a.TypeName),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = CaseTypeDifficulty.Count()
            },
        };
        return PartialView("CaseTypeDifficulty/PartialTableCaseTypeDifficulty", model);
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    public IActionResult PartialModalAddCaseTypeDifficulty()
    {
        return PartialView("CaseTypeDifficulty/PartialModalAddCaseTypeDifficulty", new SprTypeDifficulty { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalEditCaseTypeDifficulty(Guid id)
    {
        return PartialView("CaseTypeDifficulty/PartialModalEditCaseTypeDifficulty", _repository.SprTypeDifficulty.SingleOrDefault(st => st.Id == id));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="caseTypeDifficulty">объект</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitCaseTypeDifficultySave(SprTypeDifficulty caseTypeDifficulty)
    {
        if (caseTypeDifficulty.Id == Guid.Empty)
        {
            caseTypeDifficulty.DateAdd = DateTime.Now;
            _repository.Insert(caseTypeDifficulty);
        }
        else
        {
            caseTypeDifficulty.EmployeesNameModify = UserName;
            caseTypeDifficulty.DateModify = DateTime.Now;
            _repository.Update(caseTypeDifficulty);
        }
        return RedirectToAction("PartialTableCaseTypeDifficulty");
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitCaseTypeDifficultyRecovery(Guid id)
    {
        SprTypeDifficulty recoveryCaseTypeDifficulty = _repository.SprTypeDifficulty.SingleOrDefault(so => so.Id == id);
        if (recoveryCaseTypeDifficulty != null)
        {
            recoveryCaseTypeDifficulty.IsRemove = false;
            await _repository.Update(recoveryCaseTypeDifficulty);
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public async Task SubmitCaseTypeDifficultyDelete(Guid id)
    {
        SprTypeDifficulty deleteCaseTypeDifficulty = _repository.SprTypeDifficulty.SingleOrDefault(so => so.Id == id);
        if (deleteCaseTypeDifficulty != null)
        {
            deleteCaseTypeDifficulty.IsRemove = true;
            await _repository.Delete(deleteCaseTypeDifficulty);
        }
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Вопрос-ответ  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
    [Breadcrumb("Справочники \"Вопрос-ответ\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult Questions()
    {
        return View("Questions/Main");
    }
    public IActionResult PartialTableQuestions(string search, bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        ViewBag.Search = search;
        var Question = _repository.SprQuestion.Where(o => o.IsRemove == isRemove);

        ReferenceViewModel model = new()
        {
            SprQuestionList = Question.OrderBy(a => a.Question),
        };
        return PartialView("Questions/PartialTableQuestions", model);
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    public IActionResult PartialModalAddQuestion()
    {
        return PartialView("Questions/PartialModalAddQuestion", new SprQuestion { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary> 
    public IActionResult PartialModalEditQuestion(Guid id)
    {
        return PartialView("Questions/PartialModalEditQuestion", _repository.SprQuestion.SingleOrDefault(st => st.Id == id));
    }

    /// <summary>
    /// 
    /// </summary> 
    public void SubmitQuestionSave(SprQuestion request)
    {
        if (request.Id == Guid.Empty)
        {
            request.DateAdd = DateTime.Now;
            _repository.Insert(request);
        }
        else
        {
            request.EmployeesNameModify = UserName;
            request.DateModify = DateTime.Now;
            _repository.Update(request);
        }
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary> 
    public void SubmitQuestionRecovery(Guid id)
    {
        SprQuestion recoveryQuestion = _repository.SprQuestion.SingleOrDefault(so => so.Id == id);
        if (recoveryQuestion != null)
        {
            recoveryQuestion.IsRemove = false;
            recoveryQuestion.DateModify = DateTime.Now;
            _repository.Update(recoveryQuestion); recoveryQuestion.EmployeesNameModify = UserName;
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary> 
    public void SubmitQuestionDelete(Guid id)
    {
        SprQuestion deleteQuestion = _repository.SprQuestion?.SingleOrDefault(so => so.Id == id);
        if (deleteQuestion != null)
        {
            deleteQuestion.IsRemove = true;
            deleteQuestion.DateModify = DateTime.Now;
            deleteQuestion.EmployeesNameModify = UserName;
            _repository.Update(deleteQuestion);
        }
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Опрос  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    #region Вопросы
    [Breadcrumb("Справочники \"Опрос\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult SurveyQuestions()
    {
        return View("SurveyQuestions/Main");
    }
    public IActionResult PartialTableSurveyQuestions(bool isRemove = false)
    {
        ViewBag.IsRemove = isRemove;
        var SurveyQuestion = _repository.SprSurveyQuestion.Include(i => i.SprSurveyAnswer).Where(o => o.IsRemove == isRemove);
        ReferenceViewModel model = new()
        {
            SprSurveyQuestionList = SurveyQuestion.OrderBy(a => a.Question).ToList()
        };
        return PartialView("SurveyQuestions/PartialTableSurveyQuestions", model);
    }


    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns>

    public IActionResult PartialModalAddSurveyQuestion()
    {
        return PartialView("SurveyQuestions/PartialModalAddSurveyQuestion", new SprSurveyQuestion { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns>

    public IActionResult PartialModalEditSurveyQuestion(Guid id)
    {
        return PartialView("SurveyQuestions/PartialModalEditSurveyQuestion", _repository.SprSurveyQuestion.SingleOrDefault(st => st.Id == id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Question"></param>
    /// <returns></returns> 
    public void SubmitSurveyQuestionSave(SprSurveyQuestion request)
    {
        if (request.Id == Guid.Empty)
        {
            request.DateAdd = DateTime.Now;
            _repository.Insert(request);
        }
        else
        {
            request.EmployeesNameModify = UserName;
            request.DateModify = DateTime.Now;
            _repository.Update(request);
        }
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns>
    public void SubmitSurveyQuestionRecovery(Guid id)
    {
        SprSurveyQuestion recoverySurveyQuestion = _repository.SprSurveyQuestion.SingleOrDefault(so => so.Id == id);
        recoverySurveyQuestion.IsRemove = false;
        _repository.Update(recoverySurveyQuestion);
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns>
    public void SubmitSurveyQuestionDelete(Guid id)
    {
        SprSurveyQuestion deleteSurveyQuestion = _repository.SprSurveyQuestion.SingleOrDefault(so => so.Id == id);
        deleteSurveyQuestion.IsRemove = true;
        _repository.Update(deleteSurveyQuestion);
    }
    #endregion


    #region Ответы

    public IActionResult PartialTableSurveyAnswers(Guid id, bool isRemoveSurveyAnswer, bool isRemove = false)
    {
        ViewBag.SurveyQuestionId = id;
        var SurveyAnswer = _repository.SprSurveyAnswer.Where(w => w.SprSurveyQuestionId == id && w.IsRemove == isRemoveSurveyAnswer);

        ViewBag.IsRemoveSurveyAnswer = isRemoveSurveyAnswer;
        ViewBag.IsRemove = isRemove;

        ReferenceViewModel model = new()
        {
            SprSurveyAnswerList = SurveyAnswer.OrderBy(a => a.Answer).ToList(),
        };
        return PartialView("SurveyAnswers/PartialTableSurveyAnswers", model);
    }

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    public IActionResult PartialModalAddSurveyAnswer(Guid id)
    {
        return PartialView("SurveyAnswers/PartialModalAddSurveyAnswer", new SprSurveyAnswer { SprSurveyQuestionId = id, EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    public IActionResult PartialModalEditSurveyAnswer(Guid id)
    {
        return PartialView("SurveyAnswers/PartialModalEditSurveyAnswer", _repository.SprSurveyAnswer.SingleOrDefault(st => st.Id == id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ansver"></param>
    /// <returns></returns>
    public void SubmitSurveyAnswerSave(SprSurveyAnswer request)
    {
        if (request.Id == Guid.Empty)
        {
            request.DateAdd = DateTime.Now;
            request.EmployeesNameAdd = UserName;
            _repository.Insert(request);
        }
        else
        {
            request.DateModify = DateTime.Now;
            request.EmployeesNameModify = UserName;
            _repository.Update(request);
        }
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="ansverId">Id</param>
    /// <returns>частичное представление таблицы</returns>
    public void SubmitSurveyAnswerRecovery(Guid surveyAnswerId)
    {
        SprSurveyAnswer recoverySurveyAnswer = _repository.SprSurveyAnswer.SingleOrDefault(so => so.Id == surveyAnswerId);
        recoverySurveyAnswer.IsRemove = false;
        _repository.Update(recoverySurveyAnswer);
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="avnserId">Id</param>
    /// <returns>частичное представление таблицы</returns>
    public void SubmitSurveyAnswerDelete(Guid id)
    {
        SprSurveyAnswer deleteSurveyAnswer = _repository.SprSurveyAnswer.SingleOrDefault(so => so.Id == id);
        deleteSurveyAnswer.IsRemove = true;
        _repository.Update(deleteSurveyAnswer);
    }
    #endregion

    #endregion
}