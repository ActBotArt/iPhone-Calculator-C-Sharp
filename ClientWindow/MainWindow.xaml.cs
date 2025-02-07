using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ClientWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private decimal firstNumber = 0;
        private string operation = "";
        private bool isNewNumber = true;
        private ObservableCollection<CalculationHistory> history;

        public class CalculationHistory
        {
            public string Operation { get; set; }
            public string Result { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            history = new ObservableCollection<CalculationHistory>();
            HistoryList.ItemsSource = history;

            // Добавляем обработчики для всех кнопок в сетке
            foreach (UIElement child in MainGrid.Children)
            {
                if (child is Grid grid)
                {
                    foreach (UIElement gridChild in grid.Children)
                    {
                        if (gridChild is Button button)
                        {
                            button.Click += Button_Click;
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var content = button.Content.ToString();

            if (double.TryParse(content, out _) || content == ".")
            {
                if (isNewNumber)
                {
                    DisplayText.Text = content;
                    isNewNumber = false;
                }
                else
                {
                    DisplayText.Text += content;
                }
            }
            else
            {
                switch (content)
                {
                    case "C":
                        DisplayText.Text = "0";
                        firstNumber = 0;
                        operation = "";
                        isNewNumber = true;
                        break;

                    case "±":
                        if (decimal.TryParse(DisplayText.Text, out decimal currentNumber))
                        {
                            DisplayText.Text = (-currentNumber).ToString();
                        }
                        break;

                    case "%":
                        if (decimal.TryParse(DisplayText.Text, out decimal percent))
                        {
                            DisplayText.Text = (percent / 100).ToString();
                        }
                        break;

                    case "=":
                        Calculate();
                        break;

                    default: // Операторы +, -, ×, ÷
                        if (decimal.TryParse(DisplayText.Text, out decimal number))
                        {
                            firstNumber = number;
                            operation = content;
                            isNewNumber = true;
                        }
                        break;
                }
            }
        }

        private void Calculate()
        {
            if (decimal.TryParse(DisplayText.Text, out decimal secondNumber))
            {
                decimal result = 0;
                string operationText = $"{firstNumber} {operation} {secondNumber}";

                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "×":
                        result = firstNumber * secondNumber;
                        break;
                    case "÷":
                        if (secondNumber != 0)
                            result = firstNumber / secondNumber;
                        else
                        {
                            MessageBox.Show("Деление на ноль невозможно!");
                            return;
                        }
                        break;
                }

                DisplayText.Text = result.ToString();
                isNewNumber = true;

                // Добавляем операцию в историю
                history.Insert(0, new CalculationHistory 
                { 
                    Operation = operationText,
                    Result = $"= {result}"
                });
            }
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            history.Clear();
        }
    }
}
