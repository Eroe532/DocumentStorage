using System.Net.Mime;
using System.Reflection.Metadata;

using DocumentStorage.BLL;
using DocumentStorage.DAL.Model;
using DocumentStorage.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace DocumentStorage.Mapper
{
    /// <summary>
    /// Методы расширения
    /// </summary>
    public static class ConverterExtension
    {
        /// <summary>
        /// Преобразование
        /// </summary>
        /// <param name="file">Документ ДТО</param>
        public static DocumentView ToDocumentView(this IFormFile file)
        {
            var documentView = new DocumentView()
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName
            };
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                documentView.DigitalBytes = ms.ToArray();
                return documentView;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Преобразование
        /// </summary>
        /// <param name="documentDTO">Документ ДТО</param>
        public static DocumentView ToDocumentView(this DocumentDto documentDTO)
        {
            return new DocumentView()
            {
                Id = Guid.NewGuid(),
                FileName = documentDTO.FileName,
                DigitalBytes = documentDTO.DigitalBytes
            };
        }



        /// <summary>
        /// Преобразование
        /// </summary>
        /// <returns></returns>
        public static DocumentFullDto ToDocumentFullDto(this DocumentView document)
        {
            return new DocumentFullDto()
            {
                Id = document.Id,
                FileName = document.FileName,
                DigitalBytes = document.DigitalBytes
            };
        }

        /// <summary>
        /// Преобразование
        /// </summary>
        /// <returns></returns>
        public static IFormFile ToIFormFile(this DocumentView document)
        {
            using var ms = new MemoryStream(document.DigitalBytes);
            var file = new FormFile(ms, 0, ms.Length, document.FileName, document.FileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = document.FileName.GetContextType()??"",
                ContentDisposition = $"attachment; filename=\"{document.FileName}\"",
            };
            return file;
        }


        /// <summary>
        /// Преобразование
        /// </summary>
        /// <returns></returns>
        public static FileContentResult ToFile(this DocumentView document)
        {
            return new FileContentResult(document.DigitalBytes, document.FileName.GetContextType() ?? "")
            {
                FileDownloadName = document.FileName
            };
        }
    }
}
