using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP_Dankov
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditWindow.xaml
    /// </summary>
    public partial class AddOrEditWindow : Window
    {
        /// <summary>
        /// Локальный объект занятия
        /// </summary>
        public Lesson LocalLesson { get; private set; }

        /// <summary>
        /// Конструктор окна добавления изменения
        /// </summary>
        /// <param name="lesson">Объект занятия</param>
        /// <param name="type">Тип запроса(добавление/изменение)</param>
        public AddOrEditWindow(Lesson lesson, RequestType type)
        {
            InitializeComponent();
            LocalLesson = lesson;
            DataContext = LocalLesson;
            if (type == RequestType.Edit)
            {
                SendButton.Content = "Изменить";
                LessonNameInput.Text = LocalLesson.LessonName;
                TeacherNameInput.Text = LocalLesson.Teacher;
                AuditoriumInput.Text = LocalLesson.Auditorium;
                GroupNameInput.Text = LocalLesson.GroupName;
                LessonTypeComboBox.SelectedIndex = (int)LocalLesson.TypeOfLesson;
                DateSetter.SelectedDate = LocalLesson.DateAndTime;
                TimeComboBox.SelectedIndex = GetTimeIndex(LocalLesson.DateAndTime.Hour, LocalLesson.DateAndTime.Minute);
            }
        }

        /// <summary>
        /// Обработчик кнопки отправления
        /// </summary>
        /// <param name="sender">Объект-тправитель</param>
        /// <param name="e">Аргументы</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateData();
                LocalLesson.LessonName = LessonNameInput.Text;
                LocalLesson.Teacher = TeacherNameInput.Text;
                LocalLesson.Auditorium = AuditoriumInput.Text;
                LocalLesson.GroupName = GroupNameInput.Text;
                LocalLesson.TypeOfLesson = LessonTypeComboBox.SelectedIndex == 0 ? LessonType.Lecture : LessonType.Practice;
                DateTime? selectedDate = DateSetter.SelectedDate;
                (int h, int m) = GetTime(TimeComboBox.SelectedIndex);
                LocalLesson.DateAndTime = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, h, m, 0);
                DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Конвертер выбранного индекса во время
        /// </summary>
        /// <param name="selectedIndex">Выбранный индекс</param>
        /// <returns>Кортеж со временем(часы,минуты)</returns>
        private (int, int) GetTime(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: return (8, 00);
                case 1: return (9, 50);
                case 2: return (11, 40);
                case 3: return (13, 45);
                case 4: return (15, 35);
                case 5: return (17, 25);
                default: return (8, 00);
            }
        }

        /// <summary>
        /// Конвертер времени в индекс
        /// </summary>
        /// <param name="h">часы</param>
        /// <param name="m">минуты</param>
        /// <returns>Выбранный индекс</returns>
        private int GetTimeIndex(int h, int m)
        {
            switch (h, m)
            {
                case (8, 00): return 0;
                case (9, 50): return 1;
                case (11, 40): return 2;
                case (13, 45): return 3;
                case (15, 35): return 4;
                case (17, 25): return 5;
                default: return 0;
            }
        }

        /// <summary>
        /// Валидация полей
        /// </summary>
        /// <exception cref="Exception">ошибка валидации</exception>
        private void ValidateData()
        {
            // Регулярные выражения
            string lettersOnlyPattern = @"^[a-zA-Zа-яА-Я\s.-]+$";
            string alphanumericPattern = @"^[a-zA-Zа-яА-Я0-9\s-]+$";

            // Проверка LessonNameInput
            if (string.IsNullOrWhiteSpace(LessonNameInput.Text) || !System.Text.RegularExpressions.Regex.IsMatch(LessonNameInput.Text, lettersOnlyPattern))
            {
                LessonNameInput.Focus();
                throw new Exception("Пожалуйста, введите корректное наименование предмета (только буквы).");
            }

            // Проверка TeacherNameInput
            if (string.IsNullOrWhiteSpace(TeacherNameInput.Text) || !System.Text.RegularExpressions.Regex.IsMatch(TeacherNameInput.Text, lettersOnlyPattern))
            {
                TeacherNameInput.Focus();
                throw new Exception("Пожалуйста, введите корректное имя преподавателя (только буквы).");
            }

            // Проверка AuditoriumInput
            if (string.IsNullOrWhiteSpace(AuditoriumInput.Text) || !System.Text.RegularExpressions.Regex.IsMatch(AuditoriumInput.Text, alphanumericPattern))
            {
                AuditoriumInput.Focus();
                throw new Exception("Пожалуйста, введите корректный номер аудитории (буквы и цифры).");
            }

            // Проверка GroupNameInput
            if (string.IsNullOrWhiteSpace(GroupNameInput.Text) || !System.Text.RegularExpressions.Regex.IsMatch(GroupNameInput.Text, alphanumericPattern))
            {
                GroupNameInput.Focus();
                throw new Exception("Пожалуйста, введите корректное название группы (буквы и цифры).");
            }

            // Проверка LessonTypeComboBox
            if (LessonTypeComboBox.SelectedIndex == -1)
            {
                LessonTypeComboBox.Focus();
                throw new Exception("Пожалуйста, выберите тип урока.");
            }

            // Проверка DateSetter
            if (DateSetter.SelectedDate == null)
            {
                DateSetter.Focus();
                throw new Exception("Пожалуйста, выберите дату.");
            }
            else
            {
                DateTime selectedDate = DateSetter.SelectedDate.Value;
                DateTime currentDate = DateTime.Now;
                if (selectedDate < currentDate.AddYears(-5) || selectedDate > currentDate.AddYears(5))
                {
                    DateSetter.Focus();
                    throw new Exception("Пожалуйста, выберите дату в пределах ±5 лет от текущей даты.");
                }
            }

            // Проверка TimeComboBox
            if (TimeComboBox.SelectedIndex == -1)
            {
                TimeComboBox.Focus();
                throw new Exception("Пожалуйста, выберите время.");
            }
        }
    }
}
