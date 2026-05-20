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
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[MergePlayerStates]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'[MergePlayerStates]', N'UserAccountId') IS NULL
    BEGIN
        ALTER TABLE [MergePlayerStates] ADD [UserAccountId] int NOT NULL CONSTRAINT [DF_MergePlayerStates_UserAccountId] DEFAULT 0
        UPDATE [MergePlayerStates] SET [UserAccountId] = -[Id] WHERE [UserAccountId] = 0
        ALTER TABLE [MergePlayerStates] DROP CONSTRAINT [DF_MergePlayerStates_UserAccountId]
    END

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_PlayerName' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
    BEGIN
        DROP INDEX [IX_MergePlayerStates_PlayerName] ON [MergePlayerStates]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_UserAccountId' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
    BEGIN
        CREATE UNIQUE INDEX [IX_MergePlayerStates_UserAccountId] ON [MergePlayerStates] ([UserAccountId])
    END
END

IF OBJECT_ID(N'[MergeScores]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'[MergeScores]', N'UserAccountId') IS NULL
    BEGIN
        ALTER TABLE [MergeScores] ADD [UserAccountId] int NOT NULL CONSTRAINT [DF_MergeScores_UserAccountId] DEFAULT 0
        UPDATE [MergeScores] SET [UserAccountId] = -[Id] WHERE [UserAccountId] = 0
        ALTER TABLE [MergeScores] DROP CONSTRAINT [DF_MergeScores_UserAccountId]
    END

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_PlayerName_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
    BEGIN
        DROP INDEX [IX_MergeScores_PlayerName_Mode] ON [MergeScores]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_UserAccountId_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
    BEGIN
        CREATE UNIQUE INDEX [IX_MergeScores_UserAccountId_Mode] ON [MergeScores] ([UserAccountId], [Mode])
    END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        ALTER TABLE [MergeScores] DROP COLUMN [UserAccountId]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_PlayerName_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
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
        ALTER TABLE [MergePlayerStates] DROP COLUMN [UserAccountId]
    END

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_PlayerName' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
    BEGIN
        CREATE UNIQUE INDEX [IX_MergePlayerStates_PlayerName] ON [MergePlayerStates] ([PlayerName])
    END
END
");
        }
    }
}
