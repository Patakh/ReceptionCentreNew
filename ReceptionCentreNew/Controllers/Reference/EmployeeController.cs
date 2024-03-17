using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App; 
using SmartBreadcrumbs.Attributes; 
using ReceptionCentreNew.CustomCripto;

namespace ReceptionCentreNew.Controllers;
public partial class ReferenceController
{
    [Breadcrumb("Справочники \"Сотрудники\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
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

    public async Task SubmitEmployeeSave(SprEmployees employee)
    {
        if (employee.Id == Guid.Empty)
        {
            employee.EmployeesPass = Crypto.HashPassword(employee.EmployeesPass);
            employee.EmployeesNameAdd = UserName;
            employee.DateAdd = DateTime.Now;
            await _repository.Insert(employee);
        }
        else
        {
            employee.EmployeesNameModify = UserName;
            employee.DateModify = DateTime.Now;
            await _repository.Update(employee);
        }

    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="employeeId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeRecovery(Guid id)
    {
        SprEmployees recoveryEmployee = _repository.SprEmployees.SingleOrDefault(se => se.Id == id);
        recoveryEmployee.IsRemove = false;
        await _repository.Update(recoveryEmployee);
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="employeeId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeDelete(Guid id)
    {
        SprEmployees deleteEmployee = _repository.SprEmployees.SingleOrDefault(se => se.Id == id);
        if (deleteEmployee != null)
        {
            deleteEmployee.IsRemove = true;
            deleteEmployee.EmployeesNameModify = UserName;
            deleteEmployee.DateModify = DateTime.Now;
            await _repository.Delete(deleteEmployee);
        }
    }

    public IActionResult PartialTableEmployees(string search, bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        ViewBag.Search = search;
        var employees = _repository.SprEmployees.Include(se => se.SprEmployeesJobPos).Include(i => i.SprEmployeesRoleJoin).Where(o => o.IsRemove == isRemove);

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
    public IActionResult PartialModalAddEmployeeRole(Guid id)
    {
        ViewBag.EmployeeId = id;
        ViewBag.EmployeeRoles = new SelectList(_repository.SprEmployeesRole, "Id", "Commentt");
        return PartialView("Employees/EmployeeRoles/PartialModalAddEmployeeRole", new SprEmployeesRoleJoin { SprEmployeesId = id });
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет роль сотрудника
    /// </summary>
    /// <param name="employeeRole">объект роль Сотрудника</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeRoleSave(SprEmployeesRoleJoin employeeRole)
    {
        if (employeeRole.Id == Guid.Empty)
        {
            await _repository.Insert(employeeRole);
        }
        else
        {
            await _repository.Update(employeeRole);
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="employeeRoleId">Id роли Сотрудника</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeRoleDelete(Guid employeeRoleId)
    {
        var employeeRole = _repository.SprEmployeesRoleJoin.SingleOrDefault(ser => ser.Id == employeeRoleId);
        if (employeeRole != null) await _repository.Delete(employeeRole);
    }

    public IActionResult PartialTableEmployeeRoles(Guid id)
    {
        ViewBag.EmployeeId = id;
        var employeeRoleJoins = _repository.SprEmployeesRoleJoin.Where(ser => ser.SprEmployeesId == id).Include(se => se.SprEmployeesRole);
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
    public IActionResult PartialModalEditDepartment(Guid id)
    {
        return PartialView("Employees/Departments/PartialModalEditDepartment", _repository.SprEmployeesDepartment.SingleOrDefault(ed => ed.Id == id));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="department">объект</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitDepartmentSave(SprEmployeesDepartment department)
    {
        if (department.Id == Guid.Empty)
        {
            department.EmployeesNameAdd = UserName;
            department.DateAdd = DateTime.Now;
            await _repository.Insert(department);
        }
        else
        {
            department.EmployeesNameModify = UserName;
            department.DateModify = DateTime.Now;
            await _repository.Update(department);
        }
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="departmentId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitDepartmentRecovery(Guid id)
    {
        SprEmployeesDepartment recoveryDepartment = _repository.SprEmployeesDepartment.SingleOrDefault(so => so.Id == id);
        if (recoveryDepartment != null)
        {
            recoveryDepartment.IsRemove = false;
            await _repository.Update(recoveryDepartment);
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="departmentId">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitDepartmentDelete(Guid id)
    {
        SprEmployeesDepartment deleteDepartment = _repository.SprEmployeesDepartment.SingleOrDefault(sed => sed.Id == id);
        if (deleteDepartment != null)
        {
            deleteDepartment.IsRemove = true;
            deleteDepartment.EmployeesNameModify = UserName;
            deleteDepartment.DateModify = DateTime.Now;
            await _repository.Update(deleteDepartment);
        }
    }

    public IActionResult PartialTableDepartments(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var departments = _repository.SprEmployeesDepartment.Where(o => o.IsRemove == isRemove); 

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
    public IActionResult PartialModalEditEmployeeJobPos(Guid id)
    {
        return PartialView("Employees/EmployeeJobPos/PartialModalEditEmployeeJobPos", _repository.SprEmployeesJobPos.SingleOrDefault(ed => ed.Id == id));
    }

    /// <summary>
    /// Сохраняет изменнения или добавляет
    /// </summary>
    /// <param name="employeeJobPos">объект</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeJobPosSave(SprEmployeesJobPos employeeJobPos)
    {
        if (ModelState.IsValid)
        {
            if (employeeJobPos.Id == Guid.Empty)
            { 
                employeeJobPos.EmployeesNameAdd = UserName;
                employeeJobPos.DateAdd = DateTime.Now; 
                await _repository.Insert(employeeJobPos);
            }
            else
            {
                employeeJobPos.EmployeesNameModify = UserName;
                employeeJobPos.DateModify = DateTime.Now;
                await _repository.Update(employeeJobPos);
            }
        }
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeJobPosRecovery(Guid id)
    {
        SprEmployeesJobPos recoveryEmployeeJobPos = _repository.SprEmployeesJobPos.SingleOrDefault(sejp => sejp.Id == id);
        if (recoveryEmployeeJobPos != null)
        {
            recoveryEmployeeJobPos.IsRemove = false;
            await _repository.Update(recoveryEmployeeJobPos);
        }
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>частичное представление таблицы</returns> 
    public async Task SubmitEmployeeJobPosDelete(Guid id)
    {
        SprEmployeesJobPos deleteEmployeeJobPos = _repository.SprEmployeesJobPos.SingleOrDefault(sejp => sejp.Id == id);
        if (deleteEmployeeJobPos != null)
        {
            deleteEmployeeJobPos.IsRemove = true;
            await _repository.Delete(deleteEmployeeJobPos);
        }
    }

    public IActionResult PartialTableEmployeeJobPos(bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        var employeeJobPos = _repository.SprEmployeesJobPos.Where(o => o.IsRemove == isRemove);

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