using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private decimal firstNumber = 0;
    private string operation = "";
    private bool isNewNumber = true;

    public Form1()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Калькулятор";
        this.Width = 300;
        this.Height = 400;

        // Создаем текстовое поле для отображения результата
        TextBox display = new TextBox
        {
            Width = 260,
            Height = 40,
            Top = 20,
            Left = 20,
            Font = new System.Drawing.Font("Arial", 20),
            TextAlign = HorizontalAlignment.Right,
            ReadOnly = true,
            Text = "0"
        };

        // Добавляем кнопки
        string[] buttonTexts = {
            "7", "8", "9", "÷",
            "4", "5", "6", "×",
            "1", "2", "3", "-",
            "0", ".", "=", "+"
        };

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            Button button = new Button
            {
                Text = buttonTexts[i],
                Width = 60,
                Height = 60,
                Left = 20 + (i % 4) * 65,
                Top = 80 + (i / 4) * 65,
                Font = new System.Drawing.Font("Arial", 16)
            };

            if (char.IsDigit(buttonTexts[i][0]) || buttonTexts[i] == ".")
            {
                button.Click += (s, e) =>
                {
                    if (isNewNumber)
                    {
                        display.Text = (s as Button).Text;
                        isNewNumber = false;
                    }
                    else
                    {
                        display.Text += (s as Button).Text;
                    }
                };
            }
            else
            {
                button.Click += (s, e) =>
                {
                    if ((s as Button).Text == "=")
                    {
                        decimal secondNumber = decimal.Parse(display.Text);
                        decimal result = 0;

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
                                    MessageBox.Show("Деление на ноль невозможно!");
                                break;
                        }

                        display.Text = result.ToString();
                        isNewNumber = true;
                    }
                    else
                    {
                        firstNumber = decimal.Parse(display.Text);
                        operation = (s as Button).Text;
                        isNewNumber = true;
                    }
                };
            }

            this.Controls.Add(button);
        }

        // Добавляем кнопку очистки (C)
        Button clearButton = new Button
        {
            Text = "C",
            Width = 60,
            Height = 60,
            Left = 20,
            Top = 340,
            Font = new System.Drawing.Font("Arial", 16)
        };

        clearButton.Click += (s, e) =>
        {
            display.Text = "0";
            firstNumber = 0;
            operation = "";
            isNewNumber = true;
        };

        this.Controls.Add(display);
        this.Controls.Add(clearButton);
    }
} 