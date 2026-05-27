using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalapulgaMerge.Data.Migrations
{
    [DbContext(typeof(KalapulkDbContext))]
    [Migration("20260527123000_remove_merge_user_account_id")]
    public partial class remove_merge_user_account_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[MergeScores]', N'U') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_UserAccountId_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
    BEGIN
        DROP INDEX [IX_MergeScores_UserAccountId_Mode] ON [MergeScores]
    END

    IF COL_LENGTH(N'[MergeScores]', N'UserAccountId') IS NOT NULL
    BEGIN
        DECLARE @MergeScoresDefault nvarchar(max) = N''
        SELECT @MergeScoresDefault = @MergeScoresDefault + N'ALTER TABLE [MergeScores] DROP CONSTRAINT [' + dc.name + N'];'
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
        WHERE dc.parent_object_id = OBJECT_ID(N'[MergeScores]') AND c.name = N'UserAccountId'
        IF @MergeScoresDefault <> N'' EXEC sp_executesql @MergeScoresDefault
        ALTER TABLE [MergeScores] DROP COLUMN [UserAccountId]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_PlayerName_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
    AND NOT EXISTS (SELECT 1 FROM [MergeScores] GROUP BY [PlayerName], [Mode] HAVING COUNT(*) > 1)
    BEGIN
        CREATE UNIQUE INDEX [IX_MergeScores_PlayerName_Mode] ON [MergeScores] ([PlayerName], [Mode])
    END
END

IF OBJECT_ID(N'[MergePlayerStates]', N'U') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_UserAccountId' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
    BEGIN
        DROP INDEX [IX_MergePlayerStates_UserAccountId] ON [MergePlayerStates]
    END

    IF COL_LENGTH(N'[MergePlayerStates]', N'UserAccountId') IS NOT NULL
    BEGIN
        DECLARE @MergePlayerStatesDefault nvarchar(max) = N''
        SELECT @MergePlayerStatesDefault = @MergePlayerStatesDefault + N'ALTER TABLE [MergePlayerStates] DROP CONSTRAINT [' + dc.name + N'];'
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
        WHERE dc.parent_object_id = OBJECT_ID(N'[MergePlayerStates]') AND c.name = N'UserAccountId'
        IF @MergePlayerStatesDefault <> N'' EXEC sp_executesql @MergePlayerStatesDefault
        ALTER TABLE [MergePlayerStates] DROP COLUMN [UserAccountId]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_PlayerName' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
    AND NOT EXISTS (SELECT 1 FROM [MergePlayerStates] GROUP BY [PlayerName] HAVING COUNT(*) > 1)
    BEGIN
        CREATE UNIQUE INDEX [IX_MergePlayerStates_PlayerName] ON [MergePlayerStates] ([PlayerName])
    END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
