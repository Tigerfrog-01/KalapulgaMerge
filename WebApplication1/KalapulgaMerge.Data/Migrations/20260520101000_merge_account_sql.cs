using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalapulgaMerge.Data.Migrations
{
    [DbContext(typeof(KalapulkDbContext))]
    [Migration("20260520101000_merge_account_sql")]
    public partial class merge_account_sql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
