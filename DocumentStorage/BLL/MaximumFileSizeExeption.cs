namespace DocumentStorage.BLL
{
    /// <summary>
    /// Методы расширения для отображения данных о превышении максимума файла
    /// </summary>
    public static class MaximumFileSizeExeption
    {
        /// <summary>
        /// Вывод максимального размера файла в понятном виде
        /// </summary>
        /// <param name="value">Величина</param>
        /// <returns></returns>
        public static string GetSizeInBytesDescription(this long value)
        {
            return value < 1024
                ? GetDeclined(value, @"Байт")
                : value < 1048576
                    ? GetDeclined(value, 1024, @"Килобайт")
                    : value < 1073741824
                        ? GetDeclined(value, 1048576, @"Мегабайт")
                        : GetDeclined(value, 1073741824, @"Гигабайт");
        }

        private static string GetDeclined(long value, long divider, string singularUnit)
        {
            var resultValue = Math.Round(value / (double)divider, 2);
            var integerPart = Math.Truncate(resultValue);
            var fractionalPart = resultValue - integerPart;
            return fractionalPart != 0d
                ? $@"{resultValue} {singularUnit}а"
                : GetDeclined((long)integerPart, singularUnit);
        }

        private static string GetDeclined(long value, string singularUnit)
        {
            var remainder = value % 10;
            return $@"{value} {(remainder >= 2 && remainder <= 4 ? singularUnit + @"а" : singularUnit)}";
        }
    }
}
