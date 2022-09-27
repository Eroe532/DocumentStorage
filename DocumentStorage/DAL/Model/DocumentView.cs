using System.Drawing;
using System.Linq;
using System.Net.Mime;
using DocumentStorage.Dtos;

namespace DocumentStorage.DAL.Model
{
    /// <summary>
    /// Модель таблицы документы
    /// </summary>
    public class DocumentView
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название (*.*)
        /// </summary>
        public string FileName { get; set; }/*

        /// <summary>
        /// Хэш
        /// </summary>
        public int Hash { get; set; }

        /// <summary>
        /// Размен массива
        /// </summary>
        public int Size { get; set; }*/

        /// <summary>
        /// Файл
        /// </summary>
        public byte[] DigitalBytes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DocumentView()
        {
            FileName = "";
            DigitalBytes = Array.Empty<byte>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DocumentView(FileModel file, DocumentInfoModel documentInfo)
        {
            if (file.Id != documentInfo.DigitalBytesId)
            {
                throw new ArgumentException("Нет такого файла");
            }
            Id = documentInfo.Id;
            FileName = documentInfo.FileName;
            DigitalBytes = file.DigitalBytes;
        }
    }
}
