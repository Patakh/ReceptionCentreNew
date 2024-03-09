using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using System.Web.Helpers;

namespace ReceptionCentreNew.Controllers;
public partial class ReferenceController
{
    public IActionResult Employees()
    {
        return View("Employees/Main");
    }

    /// <summary>
    /// Добавление Сотрудника
    /// </summary>
    /// <returns>частичное представление модального окна</returns>

    public IActionResult PartialModalAddEmployee()
    {
        ViewBag.EmployeeJobPos = new SelectList(_repository.SprEmployeesJobPos, "Id", "JobPosName");
        ViewBag.EmployeeDepartments = new SelectList(_repository.SprEmployeesDepartment, "Id", "DepartmentName");
        return PartialView("Employees/Employees/PartialModalAddEmployee", new SprEmployees { EmployeesNameAdd = UserName, SprEmployeesDepartment = new(), SprEmployeesJobPos = new() });
    }

    /// <summary>
    /// Изменение данных Сотрудника
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalEditEmployee(Guid employeeId)
    {
        ViewBag.EmployeeJobPos = new SelectList(_repository.SprEmployeesJobPos, "Id", "JobPosName");
        ViewBag.EmployeeDepartments = new SelectList(_repository.SprEmployeesDepartment, "Id", "DepartmentName");
        return PartialView("Employees/Employees/PartialModalEditEmployee", _repository.SprEmployees.SingleOrDefault(se => se.Id == employeeId));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет сотрудника
    /// </summary>
    /// <param name="employee">объект Сотрудника</param>
    /// <returns>частичное представление таблицы</returns>

    public async Task<IActionResult> SubmitEmployeeSave(SprEmployees employee)
    {
        if (employee.Id == Guid.Empty)
        {
            employee.EmployeesPass = Crypto.HashPassword(employee.EmployeesPass);
            employee.EmployeesNameAdd = UserName;
            employee.DateAdd = DateTime.Now;
            _repository.Insert(employee);
        }
        else
        {
            employee.EmployeesNameModify = UserName;
            employee.DateModify = DateTime.Now;
            _repository.Update(employee);
        }
        return RedirectToAction("PartialTableEmployees");
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="employeeId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeRecovery(Guid employeeId)
    {
        SprEmployees recoveryEmployee = _repository.SprEmployees.SingleOrDefault(se => se.Id == employeeId);
        recoveryEmployee.IsRemove = false;
        _repository.Update(recoveryEmployee);
        return RedirectToAction("PartialTableEmployees");
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="employeeId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeDelete(Guid employeeId)
    {
        SprEmployees deleteEmployee = _repository.SprEmployees.SingleOrDefault(se => se.Id == employeeId);
        deleteEmployee.IsRemove = true;
        deleteEmployee.EmployeesNameModify = UserName;
        deleteEmployee.DateModify = DateTime.Now;
        _repository.Delete(deleteEmployee);
        return RedirectToAction("PartialTableEmployees");
    }

    public IActionResult PartialTableEmployees(string search, bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        ViewBag.Search = search;
        var employees = _repository.SprEmployees.Include(se => se.SprEmployeesJobPos).Include(i => i.SprEmployeesRoleJoin).Where(o => o.IsRemove != true);

        EmployeeViewModel model = new()
        {
            SprEmployees = employees.OrderBy(e => e.EmployeesName),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = employees.Count()
            },
        };
        return PartialView("Employees/Employees/PartialTableEmployees", model);
    }

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Роли сотрудника  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
    /// <summary>
    /// Добавление роль Сотрудника
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalAddEmployeeRole(Guid employeeId)
    {
        ViewBag.EmployeeId = employeeId;
        ViewBag.EmployeeRoles = new SelectList(_repository.SprEmployeesRole, "Id", "Commentt");
        return PartialView("Employees/EmployeeRoles/PartialModalAddEmployeeRole", new SprEmployeesRoleJoin { SprEmployeesId = employeeId });
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет роль сотрудника
    /// </summary>
    /// <param name="employeeRole">объект роль Сотрудника</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeRoleSave(SprEmployeesRoleJoin employeeRole)
    {
        if (ModelState.IsValid)
        {
            if (employeeRole.Id == Guid.Empty)
            {
                _repository.Insert(employeeRole);
            }
            else
            {
                _repository.Update(employeeRole);
            }
            return RedirectToAction("PartialTableEmployeeRoles", new { employeeId = employeeRole.SprEmployeesId });
        }
        throw new Exception("Ошибка валидации!");
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="employeeRoleId">Id роли Сотрудника</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeRoleDelete(Guid employeeRoleId)
    {
        var employeeRole = _repository.SprEmployeesRoleJoin.SingleOrDefault(ser => ser.Id == employeeRoleId);
        _repository.Delete(employeeRole);
        return RedirectToAction("PartialTableEmployeeRoles", new { employeeId = employeeRole.SprEmployeesId });
    }

    public IActionResult PartialTableEmployeeRoles(Guid employeeId)
    {
        ViewBag.EmployeeId = employeeId;
        var employeeRoleJoins = _repository.SprEmployeesRoleJoin.Where(ser => ser.SprEmployeesId == employeeId).Include(se => se.SprEmployeesRole);
        EmployeeViewModel model = new()
        {
            SprEmployeesRoleJoin = employeeRoleJoins.OrderBy(e => e.SprEmployeesRole.RoleName),
            PageInfo = new PageInfo(),
        };
        return PartialView("Employees/EmployeeRoles/PartialTableEmployeeRoles", model);
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Отделы  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalAddDepartment()
    {
        return PartialView("Employees/Departments/PartialModalAddDepartment", new SprEmployeesDepartment { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalEditDepartment(Guid departmentId)
    {
        return PartialView("Employees/Departments/PartialModalEditDepartment", _repository.SprEmployeesDepartment.SingleOrDefault(ed => ed.Id == departmentId));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="department">объект</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitDepartmentSave(SprEmployeesDepartment department)
    {
        if (ModelState.IsValid)
        {
            if (department.Id == Guid.Empty)
            {
                department.EmployeesNameAdd = UserName;
                department.DateAdd = DateTime.Now;
                _repository.Insert(department);
            }
            else
            {
                department.EmployeesNameModify = UserName;
                department.DateModify = DateTime.Now;
                _repository.Update(department);
            }
            return RedirectToAction("PartialTableDepartments");
        }
        throw new Exception("Ошибка валидации!");
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="departmentId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitDepartmentRecovery(Guid departmentId)
    {
        SprEmployeesDepartment recoveryDepartment = _repository.SprEmployeesDepartment.SingleOrDefault(so => so.Id == departmentId);

        recoveryDepartment.IsRemove = false;
        _repository.Update(recoveryDepartment);
        return RedirectToAction("PartialTableDepartments");
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="departmentId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitDepartmentDelete(Guid departmentId)
    {
        SprEmployeesDepartment deleteDepartment = _repository.SprEmployeesDepartment.SingleOrDefault(sed => sed.Id == departmentId);

        deleteDepartment.IsRemove = true;
        deleteDepartment.EmployeesNameModify = UserName;
        deleteDepartment.DateModify = DateTime.Now;
        _repository.Delete(deleteDepartment);
        return RedirectToAction("PartialTableDepartments");
    }

    public IActionResult PartialTableDepartments(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var departments = _repository.SprEmployeesDepartment;
        departments = !isRemove ? departments.Where(o => o.IsRemove != true) : departments;

        EmployeeViewModel model = new()
        {
            SprEmployeesDepartmentList = departments.OrderBy(a => a.DepartmentName),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = departments.Count()
            },
        };
        return PartialView("Employees/Departments/PartialTableDepartments", model);
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Должности  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalAddEmployeeJobPos()
    {
        return PartialView("Employees/EmployeeJobPos/PartialModalAddEmployeeJobPos", new SprEmployeesJobPos { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalEditEmployeeJobPos(Guid employeeJobPosId)
    {
        return PartialView("Employees/EmployeeJobPos/PartialModalEditEmployeeJobPos", _repository.SprEmployeesJobPos.SingleOrDefault(ed => ed.Id == employeeJobPosId));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="employeeJobPos">объект</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeJobPosSave(SprEmployeesJobPos employeeJobPos)
    {
        if (ModelState.IsValid)
        {
            if (employeeJobPos.Id == Guid.Empty)
            {
                employeeJobPos.EmployeesNameAdd = UserName;
                employeeJobPos.DateAdd = DateTime.Now;
                _repository.Insert(employeeJobPos);
            }
            else
            {
                employeeJobPos.EmployeesNameModify = UserName;
                employeeJobPos.DateModify = DateTime.Now;
                _repository.Update(employeeJobPos);
            }
            return RedirectToAction("PartialTableEmployeeJobPos");
        }
        return View(employeeJobPos);
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="employeeJobPosId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeJobPosRecovery(Guid employeeJobPosId)
    {
        SprEmployeesJobPos recoveryEmployeeJobPos = _repository.SprEmployeesJobPos.SingleOrDefault(sejp => sejp.Id == employeeJobPosId);

        recoveryEmployeeJobPos.IsRemove = false;
        _repository.Update(recoveryEmployeeJobPos);
        return RedirectToAction("PartialTableEmployeeJobPos");
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="employeeJobPosId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public IActionResult SubmitEmployeeJobPosDelete(Guid employeeJobPosId)
    {
        SprEmployeesJobPos deleteEmployeeJobPos = _repository.SprEmployeesJobPos.SingleOrDefault(sejp => sejp.Id == employeeJobPosId);

        deleteEmployeeJobPos.IsRemove = true;
        _repository.Delete(deleteEmployeeJobPos);
        return RedirectToAction("PartialTableEmployeeJobPos");
    }

    public IActionResult PartialTableEmployeeJobPos(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var employeeJobPos = _repository.SprEmployeesJobPos;
        employeeJobPos = !isRemove ? employeeJobPos.Where(o => o.IsRemove != true) : employeeJobPos;

        EmployeeViewModel model = new()
        {
            SprEmployeesJobPosList = employeeJobPos.OrderBy(a => a.JobPosName),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = employeeJobPos.Count()
            },
        };
        return PartialView("Employees/EmployeeJobPos/PartialTableEmployeeJobPos", model);
    }
    #endregion
}