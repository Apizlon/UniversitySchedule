using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Dankov
{
    /// <summary>
    /// Перечисление типов запросов
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Запрос на добавление. Используется для обозначения запроса на добавление нового элемента.
        /// </summary>
        Add,
        /// <summary>
        /// Запрос на редактирование. Используется для обозначения запроса на редактирование существующего элемента.
        /// </summary>
        Edit
    }
}
