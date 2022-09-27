namespace DocumentStorage.Dtos
{
    /// <summary>
    /// Документ ДТО
    /// </summary>
    public class DocumentFullDto
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
        public byte[] DigitalBytes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DocumentFullDto()
        {
            FileName = "";
            DigitalBytes = Array.Empty<byte>();
        }
    }
}
