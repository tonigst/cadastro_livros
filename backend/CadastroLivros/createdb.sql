-- script de criacao do banco em formato SQLITE

CREATE TABLE Livro (
    CodL          INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo        TEXT(40),
    Editora       TEXT(40),
    Edicao        INTEGER,
    AnoPublicacao TEXT(4) 
);

CREATE TABLE Preco (
    CodP      INTEGER PRIMARY KEY AUTOINCREMENT,
    CodL      INTEGER REFERENCES Livro (CodL),
    CodFC     INTEGER REFERENCES FormaCompra (CodFC),
    Valor     REAL(8, 2) 
);

CREATE TABLE FormaCompra (
    CodFC     INTEGER PRIMARY KEY AUTOINCREMENT,
    Descricao TEXT(50)
);

CREATE TABLE Autor (
    CodAu INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome  TEXT(40) 
);

CREATE TABLE Assunto (
    CodAs     INTEGER PRIMARY KEY AUTOINCREMENT,
    Descricao TEXT(20) 
);

CREATE TABLE Livro_Autor (
    Livro_CodL   INTEGER REFERENCES Livro (CodL),
    Autor_CodAu  INTEGER REFERENCES Autor (CodAu) 
);

CREATE TABLE Livro_Assunto (
    Livro_CodL    INTEGER REFERENCES Livro (CodL),
    Assunto_CodAs INTEGER REFERENCES Assunto (CodAs) 
);

CREATE INDEX LivroIndex ON Livro (    
    CodL
);

CREATE INDEX PrecoIndex ON Preco (
    CodP,
    CodL,
    CodFC
);

CREATE INDEX FormaCompraIndex ON FormaCompra (    
    CodFC
);

CREATE INDEX AutorIndex ON Autor (
    CodAu
);

CREATE INDEX AssuntoIndex ON Assunto (
    CodAs
);

CREATE INDEX Livro_AssuntoIndex ON Livro_Assunto (
    Livro_CodL,
    Assunto_CodAs
);

CREATE INDEX Livro_AutorIndex ON Livro_Autor (
    Livro_CodL,
    Autor_CodAu
);

--CREATE VIEW RelatorioView AS 
--SELECT 