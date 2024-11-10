using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Models;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using CadastroLivros.Service;
using Moq;

namespace CadastroLivros.Test.Services
{
    public class LivroServiceTest
    {
        private readonly Mock<ILivroPersistence> _livroPersistenceMock;
        private readonly Mock<IAutorPersistence> _autorPersistenceMock;
        private readonly Mock<IAssuntoPersistence> _assuntoPersistenceMock;

        public LivroServiceTest()
        {
            _livroPersistenceMock = new Mock<ILivroPersistence>();
            _autorPersistenceMock = new Mock<IAutorPersistence>();
            _assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
        }

        #region Test Objects

        private static Livro CreateValidLivro(int codL = 7)
        {
            return new Livro()
            {
                CodL = codL,
                Titulo = "Abc",
                Edicao = 9,
                Editora = "Xyz",
                AnoPublicacao = "1995"
            };
        }
        private static List<Assunto> CreateValidAssuntos()
        {
            return new List<Assunto>()
            {
                new Assunto() { CodAs = 2, Descricao = "Def" },
                new Assunto() { CodAs = 3, Descricao = "Ghi" },
            };
        }

        private static List<Autor> CreateValidAutores()
        {
            return new List<Autor>()
            {
                new Autor() { CodAu = 4, Nome = "Def" },
                new Autor() { CodAu = 5, Nome = "Ghi" },
                new Autor() { CodAu = 6, Nome = "Jkl" },
            };
        }

        private static LivroDTO CreateValidLivroDTO_Without_CodL()
        {
            return new LivroDTO()
            {
                Titulo = "Abc",
                Edicao = 9,
                Editora = "Xyz",
                AnoPublicacao = "1995",

                Autores = new List<AutorDTO>()
                {
                    new AutorDTO() { CodAu = 4, Nome = "Def" },
                    new AutorDTO() { CodAu = 5, Nome = "Ghi" },
                },

                Assuntos = new List<AssuntoDTO>()
                {
                    new AssuntoDTO() { CodAs = 2, Descricao = "Def" },
                    new AssuntoDTO() { CodAs = 3, Descricao = "Ghi" },
                }
            };
        }

        private static LivroDTO CreateValidLivroDTO(int codL = 7)
        {
            var livroDTO = CreateValidLivroDTO_Without_CodL();
            livroDTO.CodL = codL;
            return livroDTO;
        }

        #endregion

        [Fact]
        public async Task Read_Valid_CodL_Must_Return_Valid_LivroDTO()
        {
            // arrange
            Livro expectLivro = CreateValidLivro();
            List<Autor> expectedAutores = CreateValidAutores();
            List<Assunto> expectedAssuntos = CreateValidAssuntos();

            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Read(It.IsAny<int>())).ReturnsAsync(expectLivro);

            var autorPersistenceMock = new Mock<IAutorPersistence>();
            autorPersistenceMock.Setup(a => a.ReadListFromLivro(It.IsAny<int>())).ReturnsAsync(expectedAutores);

            var assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
            assuntoPersistenceMock.Setup(a => a.ReadListFromLivro(It.IsAny<int>())).ReturnsAsync(expectedAssuntos);

            var livroService = new LivroService(livroPersistenceMock.Object, autorPersistenceMock.Object, assuntoPersistenceMock.Object);

            // act
            var livroResult = await livroService.Read(It.IsAny<int>());

            // assert
            Assert.NotNull(livroResult);
            Assert.Equal(expectLivro.CodL, livroResult.CodL);
            Assert.Equal(expectLivro.Titulo, livroResult.Titulo);
            Assert.Equal(expectLivro.Edicao, livroResult.Edicao);
            Assert.Equal(expectLivro.Editora, livroResult.Editora);
            Assert.Equal(expectLivro.AnoPublicacao, livroResult.AnoPublicacao);

            Assert.NotNull(livroResult.Autores);
            var autoresResult = livroResult.Autores.ToList();
            Assert.Equal(expectedAutores.Count, autoresResult.Count);
            for (int i = 0; i < expectedAutores.Count; i++)
            {
                Assert.Equal(expectedAutores[i].CodAu, autoresResult[i].CodAu);
                Assert.Equal(expectedAutores[i].Nome, autoresResult[i].Nome);
            }

            Assert.NotNull(livroResult.Assuntos);
            var assuntosResult = livroResult.Assuntos.ToList();
            Assert.Equal(expectedAssuntos.Count, assuntosResult.Count);
            for (int i = 0; i < expectedAssuntos.Count; i++)
            {
                Assert.Equal(expectedAssuntos[i].CodAs, assuntosResult[i].CodAs);
                Assert.Equal(expectedAssuntos[i].Descricao, assuntosResult[i].Descricao);
            }
        }

        [Fact]
        public async Task Insert_Null_LivroDTO_Must_Throw_CadastroLivrosBadRequestException()
        {
            // arrange
            var livroService = new LivroService(_livroPersistenceMock.Object, _autorPersistenceMock.Object, _assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Insert(null);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosBadRequestException>(act);
        }

        private async Task Insert_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(
            bool autorPersistenceInsertOrUpdateFromLivroResult,
            bool assuntoPersistenceInsertOrUpdateFromLivroResult)
        {
            // arrange
            var expectedCodL = 15;
            LivroDTO livroDTO = CreateValidLivroDTO_Without_CodL();

            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Insert(It.IsAny<Livro>())).ReturnsAsync(CreateValidLivro(expectedCodL));

            var autorPersistenceMock = new Mock<IAutorPersistence>();
            autorPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Autor>>())).ReturnsAsync(autorPersistenceInsertOrUpdateFromLivroResult);

            var assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
            assuntoPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Assunto>>())).ReturnsAsync(assuntoPersistenceInsertOrUpdateFromLivroResult);

            var livroService = new LivroService(livroPersistenceMock.Object, autorPersistenceMock.Object, assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Insert(livroDTO);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosDataBaseException>(act);
        }

        [Fact]
        public async Task Insert_Valid_LivroDTO_But_False_Result_In_AutorPersistence_InsertOrUpdateFromLivro_Must_Throw_CadastroLivrosDataBaseException()
        {
            await Insert_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(false, true);
        }

        [Fact]
        public async Task Insert_Valid_LivroDTO_But_False_Result_In_AssuntoPersistence_InsertOrUpdateFromLivro_Must_Throw_CadastroLivrosDataBaseException()
        {
            await Insert_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(true, false);
        }

        [Fact]
        public async Task Insert_Valid_LivroDTO_Must_Return_Same_Livro_With_CodL()
        {
            // arrange
            var expectedCodL = 15;
            LivroDTO livroDTO = CreateValidLivroDTO_Without_CodL();
            var livro = CreateValidLivro(expectedCodL);

            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Insert(It.IsAny<Livro>())).ReturnsAsync(livro);

            var autorPersistenceMock = new Mock<IAutorPersistence>();
            autorPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Autor>>())).ReturnsAsync(true);

            var assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
            assuntoPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Assunto>>())).ReturnsAsync(true);

            var livroService = new LivroService(livroPersistenceMock.Object, autorPersistenceMock.Object, assuntoPersistenceMock.Object);

            // act
            var livroDTOResult = await livroService.Insert(livroDTO);

            // assert
            Assert.NotNull(livroDTOResult);
            Assert.Equal(expectedCodL, livroDTOResult.CodL);
            Assert.Equal(livroDTO.Titulo, livroDTOResult.Titulo);
            Assert.Equal(livroDTO.Edicao, livroDTOResult.Edicao);
            Assert.Equal(livroDTO.Editora, livroDTOResult.Editora);
            Assert.Equal(livroDTO.AnoPublicacao, livroDTOResult.AnoPublicacao);

            Assert.NotNull(livroDTOResult.Autores);
            var expectedAutores = livroDTO.Autores.ToList();
            var autoresResult = livroDTOResult.Autores.ToList();

            Assert.Equal(expectedAutores.Count, autoresResult.Count);
            for (int i = 0; i < expectedAutores.Count; i++)
            {
                Assert.Equal(expectedAutores[i].CodAu, autoresResult[i].CodAu);
                Assert.Equal(expectedAutores[i].Nome, autoresResult[i].Nome);
            }

            Assert.NotNull(livroDTOResult.Assuntos);
            var expectedAssuntos = livroDTO.Assuntos.ToList();
            var assuntosResult = livroDTOResult.Assuntos.ToList();

            Assert.Equal(expectedAssuntos.Count, assuntosResult.Count);
            for (int i = 0; i < expectedAssuntos.Count; i++)
            {
                Assert.Equal(expectedAssuntos[i].CodAs, assuntosResult[i].CodAs);
                Assert.Equal(expectedAssuntos[i].Descricao, assuntosResult[i].Descricao);
            }
        }

        private async Task Update_Invalid_LivroDTO_Must_Throw_CadastroLivrosBadRequestException(LivroDTO livroDTO)
        {
            // arrange
            var livroService = new LivroService(_livroPersistenceMock.Object, _autorPersistenceMock.Object, _assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Update(livroDTO);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosBadRequestException>(act);
        }

        [Fact]
        public async Task Update_Null_LivroDTO_Must_Throw_CadastroLivrosBadRequestException()
        {
            await Update_Invalid_LivroDTO_Must_Throw_CadastroLivrosBadRequestException(null);
        }

        [Fact]
        public async Task Update_LivroDTO_With_Invalid_CodL_Must_Throw_CadastroLivrosBadRequestException()
        {
            await Update_Invalid_LivroDTO_Must_Throw_CadastroLivrosBadRequestException(new LivroDTO() { CodL = -1 });
        }

        private async Task Update_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(
            bool livroPersistenceUpdateResult,
            bool autorPersistenceInsertOrUpdateFromLivroResult,
            bool assuntoPersistenceInsertOrUpdateFromLivroResult)
        {
            // arrange
            LivroDTO livroDTO = CreateValidLivroDTO();

            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Update(It.IsAny<Livro>())).ReturnsAsync(livroPersistenceUpdateResult);

            var autorPersistenceMock = new Mock<IAutorPersistence>();
            autorPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Autor>>())).ReturnsAsync(autorPersistenceInsertOrUpdateFromLivroResult);

            var assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
            assuntoPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Assunto>>())).ReturnsAsync(assuntoPersistenceInsertOrUpdateFromLivroResult);

            var livroService = new LivroService(livroPersistenceMock.Object, autorPersistenceMock.Object, assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Update(livroDTO);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosDataBaseException>(act);
        }

        [Fact]
        public async Task Update_Valid_LivroDTO_But_False_Result_In_AssuntoPersistence_InsertOrUpdateFromLivro_Must_Throw_CadastroLivrosDataBaseException()
        {
            await Update_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(true, true, false);
        }

        [Fact]
        public async Task Update_Valid_LivroDTO_But_False_Result_In_AutorPersistence_InsertOrUpdateFromLivro_Must_Throw_CadastroLivrosDataBaseException()
        {
            await Update_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(true, false, true);
        }

        [Fact]
        public async Task Update_Valid_LivroDTO_But_False_Result_In_LivroPersistence_Update_Must_Throw_CadastroLivrosDataBaseException()
        {
            await Update_Valid_LivroDTO_But_False_Result_Must_Throw_CadastroLivrosDataBaseException(false, true, true);
        }

        [Fact]
        public async Task Update_Valid_LivroDTO_Must_Not_Throw_Exceptions()
        {
            // arrange
            LivroDTO livroDTO = CreateValidLivroDTO();

            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Update(It.IsAny<Livro>())).ReturnsAsync(true);

            var autorPersistenceMock = new Mock<IAutorPersistence>();
            autorPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Autor>>())).ReturnsAsync(true);

            var assuntoPersistenceMock = new Mock<IAssuntoPersistence>();
            assuntoPersistenceMock.Setup(a => a.InsertOrUpdateFromLivro(It.IsAny<int>(), It.IsAny<IEnumerable<Assunto>>())).ReturnsAsync(true);

            var livroService = new LivroService(livroPersistenceMock.Object, autorPersistenceMock.Object, assuntoPersistenceMock.Object);

            // act
            var exception = await Record.ExceptionAsync(() => livroService.Update(livroDTO));

            // assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task Delete_Invalid_CodL_Must_Throw_CadastroLivrosBadRequestException()
        {
            // arrange
            var livroService = new LivroService(_livroPersistenceMock.Object, _autorPersistenceMock.Object, _assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Delete(-1);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosBadRequestException>(act);
        }

        [Fact]
        public async Task Delete_Valid_CodL_But_False_Result_In_LivroPersistence_Delete_Must_Throw_CadastroLivrosDataBaseException()
        {
            // arrange
            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var livroService = new LivroService(livroPersistenceMock.Object, _autorPersistenceMock.Object, _assuntoPersistenceMock.Object);

            // act
            var act = () => livroService.Delete(1);

            // assert
            await Assert.ThrowsAsync<CadastroLivrosDataBaseException>(act);
        }

        [Fact]
        public async Task Delete_Valid_CodL_Must_Not_Throw_Exceptions()
        {
            // arrange
            var livroPersistenceMock = new Mock<ILivroPersistence>();
            livroPersistenceMock.Setup(a => a.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var livroService = new LivroService(livroPersistenceMock.Object, _autorPersistenceMock.Object, _assuntoPersistenceMock.Object);

            // act
            var exception = await Record.ExceptionAsync(() => livroService.Delete(1));

            // assert
            Assert.Null(exception);
        }

    }
}