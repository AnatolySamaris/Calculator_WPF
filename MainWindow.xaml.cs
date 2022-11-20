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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace No_Way_This_Is_A_Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double result = 0;
        string lastOperation = "";
        bool basicMode = true; // Обычный режим калькулятора
        bool start = true; // Для корректного начала цепочки действий

        public MainWindow()
        {
            InitializeComponent();
        }

        private double ToDouble(string pString) // Для удачного преобразования без ошибок
        {
            double res;
            try
            {
                res = double.Parse(pString);
            }
            catch
            {
                string[] parts = pString.Split('.');
                res = double.Parse(parts[0]);
                res += double.Parse(parts[1]) * Math.Pow(10, -parts[1].Length);
            }
            return res;
        }

        private bool CheckComma(string toCheck)
        {
            for (int i = 0; i < toCheck.Length; i++)
            {
                if (toCheck[i] == '.') return true;
            }
            return false;
        }

        private void Clean()
        {
            result = 0;
            lastOperation = "";
            start = true;
        }

        private void Change_Mode_Click(object sender, RoutedEventArgs e)
        {
            if (basicMode)
            {
                basicMode = false;
                // Меняем текст в некоторых кнопках, расширяем калькулятор, где спрятаны еще кнопки
            }

            else
            {
                
            }
        }

        private void DoOperation(string operation)
        {
            switch (operation)
            {
                case "+":
                    result += ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    break;
                case "-":
                    result -= ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    break;
                case "×":
                    result *= ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    break;
                case "÷":
                    if (result != 0 && ExpScreen.Text == "0")
                    {
                        Warning.Content = "Деление на ноль";
                        Clean();
                    }
                    else
                    {
                        result /= ToDouble(ExpScreen.Text);
                        ExpScreen.Text = "0";
                    }
                    break;
                case "1/x":
                    if (ExpScreen.Text == "0")
                    {
                        Warning.Content = "Деление на ноль";
                        Clean();
                    }
                    else ExpScreen.Text = (1.0 / ToDouble(ExpScreen.Text)).ToString();
                    break;
                case "x²":
                    if (ToDouble(ExpScreen.Text) < 0)
                    {
                        Clean();
                        Warning.Content = "Число должно быть неотрицательным";
                    }
                    else
                    {
                        ExpScreen.Text = (Math.Pow(ToDouble(ExpScreen.Text), 2)).ToString();
                        Warning.Content = "";
                    }
                    break;
                case "√x":
                    if (ToDouble(ExpScreen.Text) < 0)
                    {
                        Clean();
                        Warning.Content = "Число должно быть неотрицательным";
                    }
                    else
                    {
                        ExpScreen.Text = (Math.Sqrt(ToDouble(ExpScreen.Text))).ToString();
                        Warning.Content = "";
                    }
                    break;
                case "%":
                    ExpScreen.Text = (ToDouble(ExpScreen.Text) * 0.01).ToString();
                    break;
                default: break;
            }
        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e)
        {
            if (lastOperation == "+") result += ToDouble(ExpScreen.Text);
            else if (lastOperation == "-") result -= ToDouble(ExpScreen.Text);
            else if (lastOperation == "×") result *= ToDouble(ExpScreen.Text);
            else if (lastOperation == "÷")
            {
                if (ExpScreen.Text == "0")
                {
                    Clean();
                    Warning.Content = "Деление на ноль";
                }
                else result /= ToDouble(ExpScreen.Text);
            }

            if (Warning.Content.ToString() == "")
            {
                ExpScreen.Text = result.ToString();
                Warning.Content = "";
                Clean();
            }
        }

        private void Number_Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            if (ExpScreen.Text == "0") ExpScreen.Text = clicked.Content.ToString();
            else ExpScreen.Text += clicked.Content;
            Warning.Content = "";
        }

        private void Operation_Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            string button = clicked.Content.ToString();

            if (button == "+")
            {
                if (start)
                {
                    start = false;
                    result = ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    Warning.Content = "";
                }
                else
                {
                    DoOperation(lastOperation);
                    Warning.Content = result.ToString();
                }
                lastOperation = button;
            }
            else if (button == "-")
            {
                if (start)
                {
                    start = false;
                    result = ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    Warning.Content = "";
                }
                else
                {
                    DoOperation(lastOperation);
                    Warning.Content = result.ToString();
                }
                lastOperation = button;
            }
            else if (button == "×")
            {
                if (start)
                {
                    start = false;
                    result = ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    Warning.Content = "";
                }
                else
                {
                    DoOperation(lastOperation);
                    Warning.Content = result.ToString();
                }
                lastOperation = button;
            }
            else if (button == "÷")
            {
                if (start)
                {
                    start = false;
                    result = ToDouble(ExpScreen.Text);
                    ExpScreen.Text = "0";
                    Warning.Content = "";
                }
                else
                {
                    DoOperation(lastOperation);
                    Warning.Content = result.ToString();
                }
                lastOperation = button;
            }
            else if (button == "1/x" || button == "x²" || button == "√x" || button == "%")
            {
                DoOperation(button);
            }
            else if (button == "+/-")
            {
                ExpScreen.Text = (-ToDouble(ExpScreen.Text)).ToString();
                Warning.Content = "";
            }
            else if (button == ".")
            {
                if (!CheckComma(ExpScreen.Text))
                {
                    ExpScreen.Text += ".";
                    Warning.Content = "";
                }
                else Warning.Content = "Число уже имеет дробную часть";
            }
            else if (button == "CE") 
            {
                ExpScreen.Text = "0";
                Warning.Content = "Введённое выражение очищено";
            }
            else if (button == "C")
            {
                Clean();
                ExpScreen.Text = "0";
                Warning.Content = "Все вычисления очищены";
            }
            else // Кнопка удаления последнего символа
            {
                string newNumber = "";
                for (int i = 0; i < ExpScreen.Text.ToString().Length - 1; i++)
                {
                    newNumber +=  ExpScreen.Text.ToString()[i];
                }
                ExpScreen.Text = newNumber;
            }
        }
    }
}
