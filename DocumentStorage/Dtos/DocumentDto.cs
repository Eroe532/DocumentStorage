namespace DocumentStorage.Dtos
{
    /// <summary>
    /// Документ ДТО
    /// </summary>
    public class DocumentDto
    {
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
        public DocumentDto()
        {
            FileName = "";
            DigitalBytes = Array.Empty<byte>();
        }
    }
}
