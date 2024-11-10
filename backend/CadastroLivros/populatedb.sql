
INSERT INTO Livro (CodL, Titulo, Editora, Edicao, AnoPublicacao) VALUES
(1,'Casa Azul','Brasil',2,'1980'),
(2,'O Barco Do Pereira','Europa',3,'1955'),
(3,'Enigma Da Floresta','São Paulo',1,'2010'),
(4,'A Revelacao','Lisboa',1,'1962'),
(5,'Caixa Lacrada','Europa',5,'2017'),
(6,'A Ponte Da Sabedoria','Salvador',22,'1931'),
(7,'Desvendando C#','Matias',1,'2012'),
(8,'Codigo Web Facil','Matias',2,'2018');

INSERT INTO Autor (CodAu, Nome) VALUES
(1,'Augusto Almeida'),
(2,'Morgana Cavalcante'),
(3,'Alice Souza'),
(4,'Roberto Carvalho Filho'),
(5,'Irene Santos'),
(6,'Rodrigo Nascimento'),
(7,'Mariana Pedroso'),
(8,'Alexandro Pescador');

INSERT INTO Assunto (CodAs, Descricao) VALUES
(1, 'Ficcao'),
(2, 'Drama'),
(3, 'Misterio'),
(4, 'Autoajuda'),
(5, 'Codigo'),
(6, 'Programacao'),
(7, 'C#'),
(8, 'Javascript'),
(9, 'HTMl'),
(10,'CSS');

INSERT INTO Livro_Autor (Livro_CodL, Autor_CodAu) VALUES
(1,2), -- Casa Azul            Morgana
(2,3), -- O Barco do Pereira   Alice
(3,2), -- Enigma da Floresta   Morgana (2 autores)
(3,3), -- Enigma da Floresta   Alice   (2 autores)
(4,1), -- A Revelacao          Augusto
(5,5), -- Caixa Lacrada        Irene
(6,4), -- A Ponte Da Sabedoria Roberto
(7,6), -- Desvendando C#       Rodrigo   (3 autores)
(7,7), -- Desvendando C#       Mariana   (3 autores)
(7,8), -- Desvendando C#       Alexandro (3 autores)
(8,7); -- Codigo Web Facil     Mariana

INSERT INTO Livro_Assunto (Livro_CodL, Assunto_CodAs) VALUES
(1,3),  -- Casa Azul            Misterio
(2,2),  -- O Barco do Pereira   Drama
(3,3),  -- Enigma da Floresta   Misterio
(4,1),  -- A Revelacao          Ficcao  (2 assuntos)
(4,2),  -- A Revelacao          Drama   (2 assuntos)
(5,1),  -- Caixa Lacrada        Ficcao
(6,4),  -- A Ponte Da Sabedoria Autoajuda
(7,5),  -- Desvendando C#       Codigo      (3 assuntos)
(7,6),  -- Desvendando C#       Programacao (3 assuntos)
(7,7),  -- Desvendando C#       C#          (3 assuntos)
(8,5),  -- Codigo Web Facil     Codigo      (4 assuntos)
(8,8),  -- Codigo Web Facil     Javascript  (4 assuntos)
(8,9),  -- Codigo Web Facil     HTML        (4 assuntos)
(8,10); -- Codigo Web Facil     CSS         (4 assuntos)

INSERT INTO FormaCompra (CodFC, Descricao) VALUES
(1,'Balcao'),
(2,'Internet'),
(3,'Evento'),
(4,'Self-Service');

INSERT INTO Preco (CodP, CodL, CodFC, Valor) VALUES
( 1,1,1, 29.41),
( 2,2,1, 35.71),
( 3,3,1, 22.51),
( 4,4,1, 35.01),
( 5,5,1, 60.21),
( 6,6,1, 39.91),
( 7,7,1,153.01),
( 8,8,1,125.51),

( 9,1,2, 12.42),
(10,2,2, 20.72),
(11,3,2, 11.52),
(12,4,2, 12.02),
(13,5,2, 40.22),
(14,6,2, 19.92),
(15,7,2, 70.02),
(16,8,2, 75.52),
         
(17,7,3, 59.93),
(18,8,3, 69.93),
         
(19,1,4, 20.14),
(20,2,4, 28.14),
(21,3,4, 10.54),
(22,6,4, 18.94),
(23,7,4,104.94);