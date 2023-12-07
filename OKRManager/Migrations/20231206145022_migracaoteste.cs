using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OKRManager.Migrations
{
    /// <inheritdoc />
    public partial class migracaoteste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyResults_Objective_Id",
                table: "KeyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Objective_User_Id",
                table: "Objective");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTask_KeyResults_Id",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objective",
                table: "Objective");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyResults",
                table: "KeyResults");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SubTask",
                newName: "KeyResultId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Objective",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "KeyResults",
                newName: "ObjectiveId");

            migrationBuilder.AddColumn<int>(
                name: "SubTaskId",
                table: "SubTask",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "SubTask",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ObjectiveId",
                table: "Objective",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "KeyResultId",
                table: "KeyResults",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask",
                column: "SubTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objective",
                table: "Objective",
                column: "ObjectiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyResults",
                table: "KeyResults",
                column: "KeyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_KeyResultId",
                table: "SubTask",
                column: "KeyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Objective_UserId",
                table: "Objective",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyResults_ObjectiveId",
                table: "KeyResults",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyResults_Objective_ObjectiveId",
                table: "KeyResults",
                column: "ObjectiveId",
                principalTable: "Objective",
                principalColumn: "ObjectiveId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objective_User_UserId",
                table: "Objective",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_KeyResults_KeyResultId",
                table: "SubTask",
                column: "KeyResultId",
                principalTable: "KeyResults",
                principalColumn: "KeyResultId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyResults_Objective_ObjectiveId",
                table: "KeyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Objective_User_UserId",
                table: "Objective");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTask_KeyResults_KeyResultId",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask");

            migrationBuilder.DropIndex(
                name: "IX_SubTask_KeyResultId",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objective",
                table: "Objective");

            migrationBuilder.DropIndex(
                name: "IX_Objective_UserId",
                table: "Objective");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyResults",
                table: "KeyResults");

            migrationBuilder.DropIndex(
                name: "IX_KeyResults_ObjectiveId",
                table: "KeyResults");

            migrationBuilder.DropColumn(
                name: "SubTaskId",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "ObjectiveId",
                table: "Objective");

            migrationBuilder.DropColumn(
                name: "KeyResultId",
                table: "KeyResults");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "KeyResultId",
                table: "SubTask",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Objective",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "KeyResults",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objective",
                table: "Objective",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyResults",
                table: "KeyResults",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyResults_Objective_Id",
                table: "KeyResults",
                column: "Id",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objective_User_Id",
                table: "Objective",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_KeyResults_Id",
                table: "SubTask",
                column: "Id",
                principalTable: "KeyResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
