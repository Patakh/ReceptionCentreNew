using ReceptionCentreNew.Models;
using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Areas.Identity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize(Roles = "superadmin, admin")]
    public partial class ReferenceController : Controller
    {
        public int PageSize = 10;

        #region Инициализация Repository
        private IRepository _repository;
        private string? UserName;
        public ReferenceController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == userManager.GetUserAsync(signInManager.Context.User).Result.Email).EmployeesName;
        }
        #endregion
        // GET: Reference
        public IActionResult Main()
        {
            return View();
        }
        
        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Тип обращения  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

        public IActionResult CaseType()
        {
            return View("CaseType/Main");
        }
        public IActionResult PartialTableCaseType(bool isRemove = false, int page = 1)
        {
            ViewBag.IsRemove = isRemove;
            var CaseType = _repository.SprType;
            CaseType = !isRemove ? CaseType.Where(o => o.IsRemove != true) : CaseType;

            ReferenceViewModel model = new ReferenceViewModel
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
        [HttpPost]
        public IActionResult PartialModalAddCaseType()
        {
            return PartialView("CaseType/PartialModalAddCaseType", new SprType { EmployeesNameAdd = UserName });            
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalEditCaseType(Guid caseTypeID)
        {
            return PartialView("CaseType/PartialModalEditCaseType", _repository.SprType.SingleOrDefault(st => st.Id == caseTypeID));
        }

        /// <summary>
        /// Сохраняет изменнения или добавляет
        /// </summary>
        /// <param name="caseType">объект</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeSave(SprType caseType)
        {
            if (ModelState.IsValid)
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
            throw new Exception("Ошибка валидации!");
        }

        /// <summary>
        /// Восстанавливает запись по указанному Id
        /// </summary>
        /// <param name="subjectID">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeRecovery(Guid caseTypeID)
        {
            SprType recoveryCaseType = _repository.SprType.SingleOrDefault(so => so.Id == caseTypeID);

            recoveryCaseType.IsRemove = false;
            _repository.Update(recoveryCaseType);
            return RedirectToAction("PartialTableCaseType");
        }

        /// <summary>
        /// Удаляет запись по указанному Id
        /// </summary>
        /// <param name="subjectID">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeDelete(Guid caseTypeID)
        {
            SprType deleteCaseType = _repository.SprType.SingleOrDefault(so => so.Id == caseTypeID);

            deleteCaseType.IsRemove = true;
            _repository.Update(deleteCaseType);
            return RedirectToAction("PartialTableCaseType");
        }
        #endregion

        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Тип сложности обращения  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

        public IActionResult CaseTypeDifficulty()
        {
            return View("CaseTypeDifficulty/Main");
        }
        public IActionResult PartialTableCaseTypeDifficulty(bool isRemove = false, int page = 1)
        {
            ViewBag.IsRemove = isRemove;
            var CaseTypeDifficulty = _repository.SprTypeDifficulty;
            CaseTypeDifficulty = !isRemove ? CaseTypeDifficulty.Where(o => o.IsRemove != true) : CaseTypeDifficulty;

            ReferenceViewModel model = new ReferenceViewModel
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
        [HttpPost]
        public IActionResult PartialModalAddCaseTypeDifficulty()
        {
            return PartialView("CaseTypeDifficulty/PartialModalAddCaseTypeDifficulty", new SprTypeDifficulty { EmployeesNameAdd = UserName});
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalEditCaseTypeDifficulty(Guid caseTypeDifficultyID)
        {
            return PartialView("CaseTypeDifficulty/PartialModalEditCaseTypeDifficulty", _repository.SprTypeDifficulty.SingleOrDefault(st => st.Id == caseTypeDifficultyID));
        }

        /// <summary>
        /// Сохраняет изменнения или добавляет
        /// </summary>
        /// <param name="caseTypeDifficulty">объект</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeDifficultySave(SprTypeDifficulty caseTypeDifficulty)
        {
            if (ModelState.IsValid)
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
            throw new Exception("Ошибка валидации!");
        }

        /// <summary>
        /// Восстанавливает запись по указанному Id
        /// </summary>
        /// <param name="subjectID">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeDifficultyRecovery(Guid caseTypeDifficultyID)
        {
            SprTypeDifficulty recoveryCaseTypeDifficulty = _repository.SprTypeDifficulty.SingleOrDefault(so => so.Id == caseTypeDifficultyID);

            recoveryCaseTypeDifficulty.IsRemove = false;
            _repository.Update(recoveryCaseTypeDifficulty);
            return RedirectToAction("PartialTableCaseTypeDifficulty");
        }

        /// <summary>
        /// Удаляет запись по указанному Id
        /// </summary>
        /// <param name="subjectID">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitCaseTypeDifficultyDelete(Guid caseTypeDifficultyID)
        {
            SprTypeDifficulty deleteCaseTypeDifficulty = _repository.SprTypeDifficulty.SingleOrDefault(so => so.Id == caseTypeDifficultyID);

            deleteCaseTypeDifficulty.IsRemove = true;
            _repository.Update(deleteCaseTypeDifficulty);
            return RedirectToAction("PartialTableCaseTypeDifficulty");
        }
        #endregion

        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Вопрос-ответ  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

        public IActionResult Questions()
        {
            return View("Questions/Main");
        }
        public IActionResult PartialTableQuestions(string search, bool isRemove = false, int page = 1)
        {
            ViewBag.IsRemove = isRemove;
            ViewBag.Search = search;
            var Question = _repository.SprQuestion;
            Question = !isRemove ? Question.Where(o => o.IsRemove != true) : Question;

            Question = String.IsNullOrEmpty(search) ? Question :
                search.ToLower().Split().Aggregate(Question, (current, item) => current.Where(h => h.Question.ToLower().Contains(item)
                || (h.Answer != null ? h.Answer.ToString().Contains(item) : false)
                || (h.DateAdd != null ? h.DateAdd.Value.ToString().Contains(item) : false)));

            ReferenceViewModel model = new ReferenceViewModel
            {
                SprQuestionList = Question.OrderBy(a => a.Question),
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = Question.Count()
                },
            };
            return PartialView("Questions/PartialTableQuestions", model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalAddQuestion()
        {
            return PartialView("Questions/PartialModalAddQuestion", new SprQuestion { EmployeesNameAdd = UserName });
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalEditQuestion(Guid questionId)
        {
            return PartialView("Questions/PartialModalEditQuestion", _repository.SprQuestion.SingleOrDefault(st => st.Id == questionId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateInput(false)]
        public IActionResult SubmitQuestionSave(SprQuestion request)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("PartialTableQuestions");
            }
            throw new Exception("Ошибка валидации!");
        }

        /// <summary>
        /// Восстанавливает запись по указанному Id
        /// </summary>
        /// <param name="questionId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitQuestionRecovery(Guid questionId)
        {
            SprQuestion recoveryQuestion = _repository.SprQuestion.SingleOrDefault(so => so.Id == questionId);

            recoveryQuestion.IsRemove = false;
            _repository.Update(recoveryQuestion); recoveryQuestion.EmployeesNameModify = UserName;
            recoveryQuestion.DateModify = DateTime.Now;
            return RedirectToAction("PartialTableQuestions");
        }

        /// <summary>
        /// Удаляет запись по указанному Id
        /// </summary>
        /// <param name="questionId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitQuestionDelete(Guid questionId)
        {
            SprQuestion deleteQuestion = _repository.SprQuestion.SingleOrDefault(so => so.Id == questionId);

            deleteQuestion.IsRemove = true;
            deleteQuestion.DateModify = DateTime.Now;
            deleteQuestion.EmployeesNameModify = UserName;
            _repository.Update(deleteQuestion);
            return RedirectToAction("PartialTableQuestions");
        }

        #endregion

        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Опрос  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

        #region Вопросы
        public IActionResult SurveyQuestions()
        {
            return View("SurveyQuestions/Main");
        }
        public IActionResult PartialTableSurveyQuestions(bool isRemove = false, int page = 1)
        {
            ViewBag.IsRemove = isRemove;
            var SurveyQuestion = _repository.SprSurveyQuestion.Include(i=>i.SprSurveyAnswer);
            SurveyQuestion = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<SprSurveyQuestion, ICollection<SprSurveyAnswer>>)(!isRemove ? SurveyQuestion.Where(o => o.IsRemove != true) : SurveyQuestion);

            ReferenceViewModel model = new ReferenceViewModel
            {
                SprSurveyQuestionList = SurveyQuestion.OrderBy(a => a.Question),
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = SurveyQuestion.Count()
                },
            };
            return PartialView("SurveyQuestions/PartialTableSurveyQuestions", model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalAddSurveyQuestion()
        {
            return PartialView("SurveyQuestions/PartialModalAddSurveyQuestion", new SprSurveyQuestion { EmployeesNameAdd = UserName });
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalEditSurveyQuestion(Guid questionId)
        {
            return PartialView("SurveyQuestions/PartialModalEditSurveyQuestion", _repository.SprSurveyQuestion.SingleOrDefault(st => st.Id == questionId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateInput(false)]
        public IActionResult SubmitSurveyQuestionSave(SprSurveyQuestion request)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("PartialTableSurveyQuestions");
            }
            throw new Exception("Ошибка валидации!");
        }

        /// <summary>
        /// Восстанавливает запись по указанному Id
        /// </summary>
        /// <param name="questionId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitSurveyQuestionRecovery(Guid questionId)
        {
            SprSurveyQuestion recoverySurveyQuestion = _repository.SprSurveyQuestion.SingleOrDefault(so => so.Id == questionId);

            recoverySurveyQuestion.IsRemove = false;
            _repository.Update(recoverySurveyQuestion);
            return RedirectToAction("PartialTableSurveyQuestions");
        }

        /// <summary>
        /// Удаляет запись по указанному Id
        /// </summary>
        /// <param name="questionId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitSurveyQuestionDelete(Guid questionId)
        {
            SprSurveyQuestion deleteSurveyQuestion = _repository.SprSurveyQuestion.SingleOrDefault(so => so.Id == questionId);

            deleteSurveyQuestion.IsRemove = true;
            _repository.Update(deleteSurveyQuestion);
            return RedirectToAction("PartialTableSurveyQuestions");
        }
        #endregion


        #region Ответы

        public IActionResult PartialTableSurveyAnswers(Guid surveyQuestionId, bool isRemove = false, int page = 1)
        {
            ViewBag.IsRemove = isRemove;
            var SurveyAnswer = _repository.SprSurveyAnswer.Where(w=>w.SprSurveyQuestionId== surveyQuestionId);
            SurveyAnswer = !isRemove ? SurveyAnswer.Where(o => o.IsRemove != true) : SurveyAnswer;
            ViewBag.SurveyQuestionId = surveyQuestionId;
            ReferenceViewModel model = new ReferenceViewModel
            {
                SprSurveyAnswerList = SurveyAnswer.OrderBy(a => a.Answer),
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = SurveyAnswer.Count()
                },
            };
            return PartialView("SurveyAnswers/PartialTableSurveyAnswers", model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalAddSurveyAnswer(Guid surveyQuestionId)
        {
            return PartialView("SurveyAnswers/PartialModalAddSurveyAnswer", new SprSurveyAnswer { SprSurveyQuestionId=surveyQuestionId, EmployeesNameAdd = UserName });
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns>частичное представление модального окна</returns>
        [HttpPost]
        public IActionResult PartialModalEditSurveyAnswer(Guid surveyAnswerId)
        {
            return PartialView("SurveyAnswers/PartialModalEditSurveyAnswer", _repository.SprSurveyAnswer.SingleOrDefault(st => st.Id == surveyAnswerId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ansver"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SubmitSurveyAnswerSave(SprSurveyAnswer request)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("PartialTableSurveyAnswers", new { surveyQuestionId = request.SprSurveyQuestionId});
            }
            throw new Exception("Ошибка валидации!");
        }

        /// <summary>
        /// Восстанавливает запись по указанному Id
        /// </summary>
        /// <param name="ansverId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitSurveyAnswerRecovery(Guid surveyAnswerId)
        {
            SprSurveyAnswer recoverySurveyAnswer = _repository.SprSurveyAnswer.SingleOrDefault(so => so.Id == surveyAnswerId);
            recoverySurveyAnswer.IsRemove = false;
            _repository.Update(recoverySurveyAnswer);
            return RedirectToAction("PartialTableSurveyAnswers", new { surveyQuestionId = recoverySurveyAnswer.SprSurveyQuestionId });
        }

        /// <summary>
        /// Удаляет запись по указанному Id
        /// </summary>
        /// <param name="avnserId">Id</param>
        /// <returns>частичное представление таблицы</returns>
        [HttpPost]
        public IActionResult SubmitSurveyAnswerDelete(Guid surveyAnswerId)
        {
            SprSurveyAnswer deleteSurveyAnswer = _repository.SprSurveyAnswer.SingleOrDefault(so => so.Id == surveyAnswerId);
            deleteSurveyAnswer.IsRemove = true;
            _repository.Update(deleteSurveyAnswer);
            return RedirectToAction("PartialTableSurveyAnswers", new { surveyQuestionId = deleteSurveyAnswer.SprSurveyQuestionId });
        }
        #endregion

        #endregion
    }
}