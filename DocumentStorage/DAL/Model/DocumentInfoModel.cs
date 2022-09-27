namespace DocumentStorage.DAL.Model
{
    /// <summary>
    /// Модель таблицы документы
    /// </summary>
    public class DocumentInfoModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название (*.*)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public Guid DigitalBytesId { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DocumentInfoModel()
        {
            FileName = "";
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="digitalBytesId">Идентификатор digitalBytes</param>
        public DocumentInfoModel(DocumentView document, Guid digitalBytesId)
        {
            Id = Guid.NewGuid();
            FileName = document.FileName;
            DigitalBytesId = digitalBytesId;
        }
    }
}
