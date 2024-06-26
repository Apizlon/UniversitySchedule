﻿using System;
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
    /// Логика взаимодействия для ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        /// <summary>
        /// Конструктор окна подтверждения
        /// </summary>
        /// <param name="text">Текст,отображемый в окне</param>
        public ConfirmationWindow(string text)
        {
            InitializeComponent();
            ConfirmLabel.Content = text;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку подтверждения
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы</param>
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
