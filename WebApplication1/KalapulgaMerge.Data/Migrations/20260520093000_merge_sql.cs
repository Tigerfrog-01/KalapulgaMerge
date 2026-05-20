using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalapulgaMerge.Data.Migrations
{
    public partial class merge_sql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[UserAccounts]', N'U') IS NULL
BEGIN
    CREATE TABLE [UserAccounts] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(200) NOT NULL,
        [Password] nvarchar(200) NOT NULL,
        [ProfilePicPath] nvarchar(300) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_UserAccounts] PRIMARY KEY ([Id])
    )
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UserAccounts_Email' AND object_id = OBJECT_ID(N'[UserAccounts]'))
BEGIN
    CREATE UNIQUE INDEX [IX_UserAccounts_Email] ON [UserAccounts] ([Email])
END

IF OBJECT_ID(N'[MergePlayerStates]', N'U') IS NULL
BEGIN
    CREATE TABLE [MergePlayerStates] (
        [Id] int NOT NULL IDENTITY,
        [PlayerName] nvarchar(100) NOT NULL,
        [Coins] int NOT NULL,
        [UnlockedItemsJson] nvarchar(max) NOT NULL,
        [ActiveEquipmentJson] nvarchar(max) NOT NULL,
        [Theme] nvarchar(40) NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_MergePlayerStates] PRIMARY KEY ([Id])
    )
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergePlayerStates_PlayerName' AND object_id = OBJECT_ID(N'[MergePlayerStates]'))
BEGIN
    CREATE UNIQUE INDEX [IX_MergePlayerStates_PlayerName] ON [MergePlayerStates] ([PlayerName])
END

IF OBJECT_ID(N'[MergeScores]', N'U') IS NULL
BEGIN
    CREATE TABLE [MergeScores] (
        [Id] int NOT NULL IDENTITY,
        [PlayerName] nvarchar(100) NOT NULL,
        [Score] int NOT NULL,
        [Mode] nvarchar(40) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_MergeScores] PRIMARY KEY ([Id])
    )
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MergeScores_PlayerName_Mode' AND object_id = OBJECT_ID(N'[MergeScores]'))
BEGIN
    CREATE UNIQUE INDEX [IX_MergeScores_PlayerName_Mode] ON [MergeScores] ([PlayerName], [Mode])
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[MergeScores]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [MergeScores]
END

IF OBJECT_ID(N'[MergePlayerStates]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [MergePlayerStates]
END

IF OBJECT_ID(N'[UserAccounts]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [UserAccounts]
END
");
        }
    }
}
