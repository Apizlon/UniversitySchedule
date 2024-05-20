using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_Dankov
{
    /// <summary>
    /// Логика взаимодействия с MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Контекст для взаимодействия с базой данных
        /// </summary>
        ApplicationContext db = new ApplicationContext();

        /// <summary>
        /// Конструктор основного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            db.Database.EnsureCreated();
        }

        /// <summary>
        /// Обработчик загрузки основного окна
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLessonGrid();
            SearchComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку добавления записи
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsDatabaseCreated())
                {
                    throw new Exception("Ошибка!Необходимо создать базу данных!");
                }
                AddOrEditWindow AddWindow = new AddOrEditWindow(new Lesson(), RequestType.Add);
                if (AddWindow.ShowDialog() == true)
                {
                    Lesson lesson = AddWindow.LocalLesson;
                    db.Lessons.Add(lesson);
                    db.SaveChanges();
                    EnsureImmediateWrite();
                    UpdateLessonGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления базы данных
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void DeleteDataBaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow cf = new ConfirmationWindow("Вы уверены, что хотите удалить базу данных?");
                if (cf.ShowDialog() == true)
                {
                    if (IsDatabaseCreated())
                    {
                        db.Lessons.RemoveRange(db.Lessons);
                        db.SaveChanges();
                        db.Database.EnsureDeleted();
                        MessageBox.Show("База данных успешно удалена!");
                        LessonsGrid.ItemsSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка!База данных уже удалена!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Создание новой базы данных
        /// </summary>
        private void CreateNewDataBase()
        {
            db.Database.EnsureCreated();
        }

        /// <summary>
        /// Обновление основной таблицы с данными
        /// </summary>
        private void UpdateLessonGrid()
        {
            db.Lessons.Load();
            DataContext = db.Lessons.Local.ToObservableCollection();
            LessonsGrid.ItemsSource = (System.Collections.IEnumerable)DataContext;
            LessonsGrid.Items.Refresh();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку создания базы данных
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void CreateDataBaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsDatabaseCreated())
                {
                    MessageBox.Show("Ошибка!База данных уже существует!");
                }
                else
                {
                    CreateNewDataBase();
                    MessageBox.Show("База данных успешно создана!");
                    UpdateLessonGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку сохранения базы данных
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void SaveDataBaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsDatabaseCreated())
                {
                    throw new Exception("Ошибка!Необходимо создать базу данных!");
                }
                db.SaveChanges();
                string sourceFilePath = "lessons.db";

                string destinationFolder = "SavedData";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                string timeStamp = DateTime.Now.ToString("dd_MM_yyyy;HH_mm_ss");
                string newFileName = $"lessons_{timeStamp}.db";

                string destinationFilePath = System.IO.Path.Combine(destinationFolder, newFileName);

                File.Copy(sourceFilePath, destinationFilePath, true);
                MessageBox.Show("Файл успешно сохранен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку выхода
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow cf = new ConfirmationWindow("Вы уверены, что хотите выйти?");
                if (cf.ShowDialog() == true)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку открытия базы данных из файла
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void OpenDataBaseFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsDatabaseCreated())
                {
                    throw new Exception("Ошибка!Сначала удалите текущую базу данных!");
                }
                var openFileDialog = new OpenFileDialog()
                {
                    Title = "Открыть файл базы данных",
                    DefaultExt = ".db",
                    Filter = "Файл .db|*.db"
                };


                if (openFileDialog.ShowDialog() == true)
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string destinationFilePath = "lessons.db";
                    db.Dispose();
                    File.Copy(sourceFilePath, destinationFilePath, true);
                    db = new ApplicationContext();
                    db.Database.EnsureCreated();
                    UpdateLessonGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Проверка на существование базы данных
        /// </summary>
        /// <returns>true,если база данных создана, иначе - false</returns>
        private bool IsDatabaseCreated()
        {
            return db.Database.CanConnect();
        }

        /// <summary>
        /// Сохранение базы данных в файл
        /// </summary>
        private void EnsureImmediateWrite()
        {
            db.ExecuteSql("PRAGMA wal_checkpoint(FULL);");
            db.ExecuteSql("PRAGMA synchronous=FULL;");
        }

        /// <summary>
        /// Обработчик нажатия на кнопку редактирования
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LessonsGrid.SelectedItem == null)
                {
                    throw new Exception("Ошибка!Выберете объект");
                }
                Lesson selectedLesson = (Lesson)LessonsGrid.SelectedItem;
                AddOrEditWindow EditWindow = new AddOrEditWindow(selectedLesson, RequestType.Edit);
                if (EditWindow.ShowDialog() == true)
                {
                    Lesson lesson = EditWindow.LocalLesson;
                    db.Lessons.Entry(lesson).State = EntityState.Modified;
                    db.SaveChanges();
                    EnsureImmediateWrite();
                    UpdateLessonGrid();
                    MessageBox.Show("Запись успешно отредактирована!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LessonsGrid.SelectedItem == null)
                {
                    throw new Exception("Ошибка!Выберете объект");
                }
                ConfirmationWindow cf = new ConfirmationWindow("Вы уверены, что хотите удалить запись?");
                if (cf.ShowDialog() == true)
                {
                    Lesson selectedLesson = (Lesson)LessonsGrid.SelectedItem;
                    db.Lessons.Remove(selectedLesson);
                    db.SaveChanges();
                    EnsureImmediateWrite();
                    UpdateLessonGrid();
                    MessageBox.Show("Запись успешно удалена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик изменений в поле поиска
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchTextBox.Text.ToLower();
                string selectedField = SearchComboBox.SelectedIndex.ToString();

                var filteredLessons = db.Lessons.Local.ToObservableCollection().Where(lesson =>
                {
                    switch (selectedField)
                    {
                        case "0":
                            return lesson.LessonName.ToLower().Contains(searchText);
                        case "1":
                            return lesson.DateAndTime.ToString().Contains(searchText);
                        case "2":
                            return lesson.Teacher.ToLower().Contains(searchText);
                        case "3":
                            return lesson.Auditorium.ToLower().Contains(searchText);
                        case "4":
                            return lesson.GroupName.ToLower().Contains(searchText);
                        default:
                            return false;
                    }
                }).ToList();

                LessonsGrid.ItemsSource = filteredLessons;
                LessonsGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку загрузки данных из Excel таблицы
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void LoadFromExcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsDatabaseCreated())
                {
                    throw new Exception("Ошибка!Необходимо создать базу данных!");
                }
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Выберите файл Excel",
                    Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var filePath = openFileDialog.FileName;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            Lesson lesson = new Lesson
                            {
                                LessonName = worksheet.Cells[row, 1].Text,
                                Teacher = worksheet.Cells[row, 3].Text,
                                Auditorium = worksheet.Cells[row, 4].Text,
                                GroupName = worksheet.Cells[row, 5].Text,
                                TypeOfLesson = (LessonType)Enum.Parse(typeof(LessonType), worksheet.Cells[row, 6].Text)
                            };
                            string dateAndTimeText = worksheet.Cells[row, 2].Text;
                            string dateFormat = "M.d.yy H:mm";

                            if (DateTime.TryParseExact(dateAndTimeText, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAndTime))
                            {
                                lesson.DateAndTime = dateAndTime;
                            }
                            else
                            {
                                MessageBox.Show("Ошибка чтения даты/времени\n" + worksheet.Cells[row, 2].Text);
                                break;
                            }
                            db.Lessons.Add(lesson);
                        }

                        db.SaveChanges();
                        UpdateLessonGrid();
                    }

                    MessageBox.Show("Данные успешно импортированы!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}