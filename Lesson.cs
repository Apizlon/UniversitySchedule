using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Dankov
{
    /// <summary>
    /// Класс, представляющий занятие
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Уникальный идентификатор занятия
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование предмета
        /// </summary>
        public string LessonName { get; set; }


        /// <suДата и время проведения занятияmmary>
        /// 
        /// </summary>
        public DateTime DateAndTime { get; set; }


        /// <summary>
        /// Преподаватель, ведущий занятие
        /// </summary>
        public string Teacher { get; set; }


        /// <summary>
        /// Аудитория, в которой проводится занятие
        /// </summary>
        public string Auditorium { get; set; }


        /// <summary>
        /// Название группы, для которой проводится занятие
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Тип занятия
        /// </summary>
        public LessonType TypeOfLesson { get; set; }
    }
}
