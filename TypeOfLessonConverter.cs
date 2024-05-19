using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OOP_Dankov
{
    /// <summary>
    /// Класс-конверетер для перечеслиения LessonType
    /// </summary>
    class TypeOfLessonConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует значение LessonType в строковое представление
        /// </summary>
        /// <param name="value">Исходное значение типа LessonType</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура</param>
        /// <returns>Строковое представление типа занятия</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Lecture")
            {
                return "Лекция";
            }
            else
            {
                return "Практика";
            }
        }

        /// <summary>
        /// Преобразование строки обратно в LessonType.
        /// </summary>
        /// <param name="value">Значение для преобразования</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
