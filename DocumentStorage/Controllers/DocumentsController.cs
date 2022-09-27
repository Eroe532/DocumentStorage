using DocumentStorage.BLL;
using DocumentStorage.DAL.Repository;
using DocumentStorage.Dtos;
using DocumentStorage.Mapper;

using Microsoft.AspNetCore.Mvc;

namespace DocumentsStorage.Controllers
{
    /// <summary>
    /// Контроллер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        /// <summary>
        /// Репозиторий документа
        /// </summary>
        readonly DocumentBL _documentBL;

        /// <summary>
        /// Максимальная величина файла
        /// </summary>
        readonly MaximumFileSize _maximumFileSize;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="documentRepository">Репозиторий документа</param>
        /// <param name="maximumFileSize">Максимальная величина файла</param>
        public DocumentsController(DocumentRepository documentRepository, MaximumFileSize maximumFileSize)
        {
            _documentBL = new DocumentBL(documentRepository);
            _maximumFileSize = maximumFileSize;
        }

        /// <summary>
        /// Вывести документ по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("GetDocumentByteArray")]
        public async Task<IActionResult> GetDocumentByteArray([FromQuery] Guid id)
        {
            return await Task.Run<IActionResult>(() =>
            {
                var res = _documentBL.GetDocumentFullDto(id);
                return res != null
                   ? Ok(res)
                   : BadRequest($"Данныс с таким id нет");
            });
        }

        /// <summary>
        /// Вывести документ по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("GetDocumentFile")]
        public async Task<IActionResult> GetDocumentFile([FromQuery] Guid id)
        {
            return await Task.Run<IActionResult>(() =>
            {
                var res = _documentBL.GetFile(id);
                return res != null
                   ? res
                   : BadRequest($"Данныс с таким id нет");
            });
        }

        /// <summary>
        /// Добавление файла документа во внутреннее хранилище
        /// </summary>
        /// <param name="file">Документ</param>
        /// <returns></returns>
        [HttpPost("SaveDocumentByteArray")]
        public async Task<IActionResult> SaveDocumentByteArray([FromForm] IFormFile file)
        {
            return await Task.Run(() =>
            {
                if (file.Length > _maximumFileSize.Value)
                {
                    return BadRequest($@"Размер файла превышает максимально допустимый. Загрузите файл размером не больше {_maximumFileSize.Value.GetSizeInBytesDescription()}");
                }
                if (!file.FileName.CheckFileExtension())
                {
                    return BadRequest($@"Неизвестное расширение файла");
                }
                var res = _documentBL.Create(file);
                return res != null
                    ? Ok(res.Value)
                    : (IActionResult)NotFound($"Не удалось записать данные");
            });
        }


        /// <summary>
        /// Добавление файла документа во внутреннее хранилище
        /// </summary>
        /// <param name="documentDTO">Документ</param>
        /// <returns></returns>
        [HttpPost("SaveDocumentByteArrayByImport")]
        public async Task<IActionResult> SaveDocumentByteArray([FromBody] DocumentDto documentDTO)
        {
            return await Task.Run(() =>
            {
                if (documentDTO.DigitalBytes.LongLength > _maximumFileSize.Value)
                {
                    return BadRequest($@"Размер файла превышает максимально допустимый. Загрузите файл размером не больше {_maximumFileSize.Value.GetSizeInBytesDescription()}");
                }
                if (!documentDTO.FileName.CheckFileExtension())
                {
                    return BadRequest($@"Неизвестное расширение файла");
                }
                var res = _documentBL.Create(documentDTO);
                return res != null
                    ? Ok(res.Value)
                    : (IActionResult)NotFound($"Не удалось записать данные");
            });
        }
    }
}
