using System.Reflection.Metadata;

namespace DocumentStorage.BLL
{
    /// <summary>
    /// ContextType
    /// </summary>
    public static class ContextTypes
    {
        /// <summary>
        /// Словарь со всеми конвертациями
        /// </summary>
        static readonly Dictionary<string, string> _contextTypes = SetContextTypes();

        /// <summary>
        /// Получение словаря
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> SetContextTypes()
        {
            var result = new Dictionary<string, string>
            {
                { "json", "application/json" },
                { "js", "application/javascript" },
                { "cjs", "application/javascript" },
                { "pdf", "application/pdf" },
                { "zip", "application/zip" },
                { "xml", "application/xml" },
                { "doc", "application/msword" },
                { "docx", "application/msword" },
                { "gif", "image/gif" },
                { "jpeg", "image/jpeg" },
                { "png", "image/png" },
                { "webp", "image/webp" },
                { "css", "text/css" },
                { "xml", "text/xml" }
            };
            return result;
        }

        /// <summary>
        /// Получить ContextType
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        public static string? GetContextType(this string fileName)
        {
            var extansion = fileName.Split('.').LastOrDefault()?.ToLower()??"";
            return _contextTypes.TryGetValue(extansion,out var contextType) ? contextType : null;
        }

        /// <summary>
        /// Возможно ли занести файл с таким расширением в систему
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        public static bool CheckFileExtension(this string fileName)
        {
            return GetContextType(fileName) != null;
        }
    }
}
