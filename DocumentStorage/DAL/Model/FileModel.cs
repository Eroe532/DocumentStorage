using DocumentStorage.BLL;

namespace DocumentStorage.DAL.Model
{
    /// <summary>
    /// Модель таблицы документы
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Хэш
        /// </summary>
        public int Hash { get; set; }

        /// <summary>
        /// Размен массива
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public byte[] DigitalBytes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileModel()
        {
            DigitalBytes = Array.Empty<byte>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="document">Документ</param>
        public FileModel(DocumentView document)
        {
            Id = Guid.NewGuid();
            DigitalBytes = document.DigitalBytes;
            Hash = DigitalBytes.Hash();
            Size = DigitalBytes.Length;
        }
    }
}
