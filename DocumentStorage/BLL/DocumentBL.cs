using DocumentStorage.DAL.Model;
using DocumentStorage.DAL.Repository;
using DocumentStorage.Dtos;
using DocumentStorage.Mapper;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DocumentStorage.BLL
{
    /// <summary>
    /// Бизнес логика для документов
    /// </summary>
    public class DocumentBL
    {
        /// <summary>
        /// Репозиторий документа
        /// </summary>
        readonly DocumentRepository _documentRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="documentRepository">Репозиторий документа</param>
        public DocumentBL(DocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Получение по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentFullDto? GetDocumentFullDto(Guid id)
        {
            var res = _documentRepository.Get(id);
            return res?.ToDocumentFullDto();
        }

        /// <summary>
        /// Получение по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileContentResult? GetFile(Guid id)
        {
            var res = _documentRepository.Get(id);
            return res?.ToFile();
        }

        /// <summary>
        /// Создание
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public Guid? Create(IFormFile file)
        {
            return _documentRepository.Create(file.ToDocumentView());
        }

        /// <summary>
        /// Создание
        /// </summary>
        /// <param name="documentDTO">Документ</param>
        /// <returns></returns>
        public Guid? Create(DocumentDto documentDTO)
        {
            return _documentRepository.Create(documentDTO.ToDocumentView());
        }
    }
}
