using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OOP_Dankov
{
    /// <summary>
    /// Контекст базы данных приложения для работы с базой данных SQLite.
    /// </summary>
    class ApplicationContext : DbContext
    {
        /// <summary>
        /// Набор данных, представляющий занятия
        /// </summary>
        public DbSet<Lesson> Lessons { get; set; } = null!;

        /// <summary>
        /// Метод переопределяет настройку подключения, устанавливая SQLite в качестве источника данных.
        /// </summary>
        /// <param name="optionsBuilder"Построитель параметров подключения></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=lessons.db");
        }

        /// <summary>
        /// Выполнение SQL-запроса
        /// </summary>
        /// <param name="sql">Текст SQL-запроса</param>
        public void ExecuteSql(string sql)
        {
            this.Database.ExecuteSqlRaw(sql);
        }
    }
}
