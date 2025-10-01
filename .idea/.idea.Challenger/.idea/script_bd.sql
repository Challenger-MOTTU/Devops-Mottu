-- ======================================================
-- Script DDL do banco de dados
-- Baseado na migração EF Core: 20251001195207_Initial
-- Arquivo: script_bd.sql
-- ======================================================

-- ==========================================
-- Tabela: Patios
-- ==========================================
CREATE TABLE Patios (
    Id UNIQUEIDENTIFIER NOT NULL,             -- Identificador único do pátio (UUID)
    Name NVARCHAR(100) NOT NULL,              -- Nome do pátio
    Cidade NVARCHAR(100) NOT NULL,            -- Cidade onde o pátio está localizado
    Capacidade INT NOT NULL,                   -- Capacidade máxima de motos
    CreatedAt DATETIME2 NOT NULL,             -- Data de criação do registro
    CreatedBy NVARCHAR(MAX) NOT NULL,         -- Usuário que criou o registro
    UpdatedAt DATETIME2 NOT NULL,             -- Data da última atualização
    UpdatedBy NVARCHAR(MAX) NOT NULL,         -- Usuário que atualizou o registro
    CONSTRAINT PK_Patios PRIMARY KEY (Id)     -- Chave primária
);
GO

-- Comentários
EXEC sp_addextendedproperty 'MS_Description', 'Tabela que armazena os pátios da empresa', 'SCHEMA', 'dbo', 'TABLE', 'Patios';
EXEC sp_addextendedproperty 'MS_Description', 'Nome do pátio', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'Name';
EXEC sp_addextendedproperty 'MS_Description', 'Cidade do pátio', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'Cidade';
EXEC sp_addextendedproperty 'MS_Description', 'Capacidade máxima de motos', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'Capacidade';
EXEC sp_addextendedproperty 'MS_Description', 'Data de criação', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'CreatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário criador', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'CreatedBy';
EXEC sp_addextendedproperty 'MS_Description', 'Data de atualização', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'UpdatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário que atualizou', 'SCHEMA', 'dbo', 'TABLE', 'Patios', 'COLUMN', 'UpdatedBy';
GO

-- ==========================================
-- Tabela: Users
-- ==========================================
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL,             -- Identificador único do usuário (UUID)
    Username NVARCHAR(100) NOT NULL,          -- Nome de usuário
    Email NVARCHAR(MAX) NOT NULL,             -- E-mail do usuário
    Senha NVARCHAR(MAX) NOT NULL,             -- Senha do usuário
    CreatedAt DATETIME2 NOT NULL,             -- Data de criação do registro
    CreatedBy NVARCHAR(MAX) NOT NULL,         -- Usuário que criou o registro
    UpdatedAt DATETIME2 NOT NULL,             -- Data da última atualização
    UpdatedBy NVARCHAR(MAX) NOT NULL,         -- Usuário que atualizou o registro
    CONSTRAINT PK_Users PRIMARY KEY (Id)      -- Chave primária
);
GO

-- Comentários
EXEC sp_addextendedproperty 'MS_Description', 'Tabela de usuários do sistema', 'SCHEMA', 'dbo', 'TABLE', 'Users';
EXEC sp_addextendedproperty 'MS_Description', 'Nome de usuário', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'Username';
EXEC sp_addextendedproperty 'MS_Description', 'E-mail do usuário', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'Email';
EXEC sp_addextendedproperty 'MS_Description', 'Senha do usuário', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'Senha';
EXEC sp_addextendedproperty 'MS_Description', 'Data de criação', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'CreatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário criador', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'CreatedBy';
EXEC sp_addextendedproperty 'MS_Description', 'Data de atualização', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'UpdatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário que atualizou', 'SCHEMA', 'dbo', 'TABLE', 'Users', 'COLUMN', 'UpdatedBy';
GO

-- ==========================================
-- Tabela: Motos
-- ==========================================
CREATE TABLE Motos (
    Id UNIQUEIDENTIFIER NOT NULL,             -- Identificador único da moto (UUID)
    Placa NVARCHAR(MAX) NOT NULL,             -- Placa da moto
    Modelo INT NOT NULL,                       -- Modelo da moto (enum)
    Status INT NOT NULL,                        -- Status da moto (enum)
    PatioId UNIQUEIDENTIFIER NOT NULL,        -- Pátio onde a moto está estacionada
    CreatedAt DATETIME2 NOT NULL,
    CreatedBy NVARCHAR(MAX) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    UpdatedBy NVARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Motos PRIMARY KEY (Id),
    CONSTRAINT FK_Motos_Patios_PatioId FOREIGN KEY (PatioId) REFERENCES Patios(Id) ON DELETE CASCADE
);
GO

-- Índice para FK
CREATE INDEX IX_Motos_PatioId ON Motos(PatioId);
GO

-- Comentários
EXEC sp_addextendedproperty 'MS_Description', 'Tabela que armazena as motos', 'SCHEMA', 'dbo', 'TABLE', 'Motos';
EXEC sp_addextendedproperty 'MS_Description', 'Placa da moto', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'Placa';
EXEC sp_addextendedproperty 'MS_Description', 'Modelo da moto', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'Modelo';
EXEC sp_addextendedproperty 'MS_Description', 'Status da moto', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'Status';
EXEC sp_addextendedproperty 'MS_Description', 'Chave estrangeira para o pátio', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'PatioId';
EXEC sp_addextendedproperty 'MS_Description', 'Data de criação', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'CreatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário criador', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'CreatedBy';
EXEC sp_addextendedproperty 'MS_Description', 'Data de atualização', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'UpdatedAt';
EXEC sp_addextendedproperty 'MS_Description', 'Usuário que atualizou', 'SCHEMA', 'dbo', 'TABLE', 'Motos', 'COLUMN', 'UpdatedBy';
GO
