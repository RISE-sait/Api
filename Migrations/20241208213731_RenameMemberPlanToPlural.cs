using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameMemberPlanToPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipPlan_Memberships_MembershipId",
                table: "MembershipPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipPlan",
                table: "MembershipPlan");

            migrationBuilder.RenameTable(
                name: "MembershipPlan",
                newName: "MembershipPlans");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipPlan_MembershipId",
                table: "MembershipPlans",
                newName: "IX_MembershipPlans_MembershipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipPlans",
                table: "MembershipPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipPlans_Memberships_MembershipId",
                table: "MembershipPlans",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipPlans_Memberships_MembershipId",
                table: "MembershipPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipPlans",
                table: "MembershipPlans");

            migrationBuilder.RenameTable(
                name: "MembershipPlans",
                newName: "MembershipPlan");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipPlans_MembershipId",
                table: "MembershipPlan",
                newName: "IX_MembershipPlan_MembershipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipPlan",
                table: "MembershipPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipPlan_Memberships_MembershipId",
                table: "MembershipPlan",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
