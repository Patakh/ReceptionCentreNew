using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceptionCentreNew.Migrations
{
    /// <inheritdoc />
    public partial class AspIdentityv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeesLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeesPass",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SprEmployeesJobPosId",
                table: "AspNetUsers",
                newName: "spr_employees_job_pos_id");

            migrationBuilder.RenameColumn(
                name: "SprEmployeesDepartmentId",
                table: "AspNetUsers",
                newName: "spr_employees_department_id");

            migrationBuilder.RenameColumn(
                name: "IsRemove",
                table: "AspNetUsers",
                newName: "is_remove");

            migrationBuilder.RenameColumn(
                name: "IpAddressModify",
                table: "AspNetUsers",
                newName: "ip_address_modify");

            migrationBuilder.RenameColumn(
                name: "IpAddressAdd",
                table: "AspNetUsers",
                newName: "ip_address_add");

            migrationBuilder.RenameColumn(
                name: "EmployeesNameModify",
                table: "AspNetUsers",
                newName: "employees_name_modify");

            migrationBuilder.RenameColumn(
                name: "EmployeesNameAdd",
                table: "AspNetUsers",
                newName: "employees_name_add");

            migrationBuilder.RenameColumn(
                name: "EmployeesName",
                table: "AspNetUsers",
                newName: "employees_name");

            migrationBuilder.RenameColumn(
                name: "EmployeesActive",
                table: "AspNetUsers",
                newName: "employees_active");

            migrationBuilder.RenameColumn(
                name: "DateModify",
                table: "AspNetUsers",
                newName: "date_modify");

            migrationBuilder.RenameColumn(
                name: "DateAdd",
                table: "AspNetUsers",
                newName: "date_add");

            migrationBuilder.RenameColumn(
                name: "CommenttModify",
                table: "AspNetUsers",
                newName: "commentt_modify");

            migrationBuilder.RenameColumn(
                name: "CanTakeAppeal",
                table: "AspNetUsers",
                newName: "can_take_appeal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "spr_employees_job_pos_id",
                table: "AspNetUsers",
                newName: "SprEmployeesJobPosId");

            migrationBuilder.RenameColumn(
                name: "spr_employees_department_id",
                table: "AspNetUsers",
                newName: "SprEmployeesDepartmentId");

            migrationBuilder.RenameColumn(
                name: "is_remove",
                table: "AspNetUsers",
                newName: "IsRemove");

            migrationBuilder.RenameColumn(
                name: "ip_address_modify",
                table: "AspNetUsers",
                newName: "IpAddressModify");

            migrationBuilder.RenameColumn(
                name: "ip_address_add",
                table: "AspNetUsers",
                newName: "IpAddressAdd");

            migrationBuilder.RenameColumn(
                name: "employees_name_modify",
                table: "AspNetUsers",
                newName: "EmployeesNameModify");

            migrationBuilder.RenameColumn(
                name: "employees_name_add",
                table: "AspNetUsers",
                newName: "EmployeesNameAdd");

            migrationBuilder.RenameColumn(
                name: "employees_name",
                table: "AspNetUsers",
                newName: "EmployeesName");

            migrationBuilder.RenameColumn(
                name: "employees_active",
                table: "AspNetUsers",
                newName: "EmployeesActive");

            migrationBuilder.RenameColumn(
                name: "date_modify",
                table: "AspNetUsers",
                newName: "DateModify");

            migrationBuilder.RenameColumn(
                name: "date_add",
                table: "AspNetUsers",
                newName: "DateAdd");

            migrationBuilder.RenameColumn(
                name: "commentt_modify",
                table: "AspNetUsers",
                newName: "CommenttModify");

            migrationBuilder.RenameColumn(
                name: "can_take_appeal",
                table: "AspNetUsers",
                newName: "CanTakeAppeal");

            migrationBuilder.AddColumn<string>(
                name: "EmployeesLogin",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeesPass",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
