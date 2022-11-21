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
        bool input = false; // Проверка, есть ли на экране хоть какое-то число
        bool powering = false;

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
            input = false;
        }

        private double TrigAns(double n)
        {
            bool sign = true;
            if (n < 0) sign = false;
            if (Math.Abs(n) < 1 && Math.Abs(n) > 0.999)
            {
                if (sign) return 1;
                else return -1;
            }
            else if (Math.Abs(n) > 0 && Math.Abs(n) < 0.001) return 0;
            else return n;
        }

        private void Change_Mode_Click(object sender, RoutedEventArgs e)
        {
            Button modeButton = (Button)sender;
            string modeButtonText = modeButton.Content.ToString();
            if (basicMode && modeButtonText == "Инженерный")
            {
                basicMode = false;
                Extended1.Width = new GridLength(1, GridUnitType.Star);
                Extended2.Width = new GridLength(1, GridUnitType.Star);
            }
            else if (!basicMode && modeButtonText == "Обычный")
            {
                basicMode = true;
                Extended1.Width = new GridLength(0, GridUnitType.Star);
                Extended2.Width = new GridLength(0, GridUnitType.Star);
            }
        }

        private void DoOperation(string operation)
        {
            if (powering)
            {
                powering = false;
                result = Math.Pow(result, ToDouble(ExpScreen.Text));
                ExpScreen.Text = result.ToString();
                Warning.Content = "";
            }

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
                    Warning.Content = "";
                    break;
                case "sin":
                    ExpScreen.Text = TrigAns(Math.Sin(ToDouble(ExpScreen.Text) * Math.PI / 180)).ToString();
                    Warning.Content = "";
                    break;
                case "cos":
                    ExpScreen.Text = TrigAns(Math.Cos(ToDouble(ExpScreen.Text) * Math.PI / 180)).ToString();
                    Warning.Content = "";
                    break;
                case "tan":
                    ExpScreen.Text = TrigAns(Math.Tan(ToDouble(ExpScreen.Text) * Math.PI / 180)).ToString();
                    Warning.Content = "";
                    break;
                case "log":
                    ExpScreen.Text = Math.Log10(ToDouble(ExpScreen.Text)).ToString();
                    Warning.Content = "";
                    break;
                case "ln":
                    ExpScreen.Text = Math.Log(ToDouble(ExpScreen.Text)).ToString();
                    Warning.Content = "";
                    break;
                case "exp":
                    ExpScreen.Text = Math.Exp(ToDouble(ExpScreen.Text)).ToString();
                    Warning.Content = "";
                    break;
                case "asin":
                    if (Math.Abs(ToDouble(ExpScreen.Text)) <= 1)
                    {
                        ExpScreen.Text = (Math.Asin(ToDouble(ExpScreen.Text)) * 180 / Math.PI).ToString();
                        Warning.Content = "";
                    }
                    else
                    {
                        Warning.Content = "Аргумент должен быть не больше 1 по модулю";
                        Clean();
                    }
                    break;
                case "acos":
                    if (Math.Abs(ToDouble(ExpScreen.Text)) <= 1)
                    {
                        ExpScreen.Text = (Math.Acos(ToDouble(ExpScreen.Text)) * 180 / Math.PI).ToString();
                        Warning.Content = "";
                    }
                    else
                    {
                        Warning.Content = "Аргумент должен быть не больше 1 по модулю";
                        Clean();
                    }
                    break;
                case "atan":
                    ExpScreen.Text = (Math.Atan(ToDouble(ExpScreen.Text)) * 180 / Math.PI).ToString();
                    Warning.Content = "";
                    break;
                case "mod":
                    if (result != 0 && ExpScreen.Text == "0")
                    {
                        Warning.Content = "Деление на ноль";
                        Clean();
                    }
                    else
                    {
                        result %= ToDouble(ExpScreen.Text);
                        Warning.Content = "";
                        ExpScreen.Text = "0";
                    }
                    break;
                case "x!":
                    double num = ToDouble(ExpScreen.Text);

                    if (Math.Floor(num) == Math.Ceiling(num) && num > 0)
                    {
                        double res = 1;
                        for (int i = 2; i <= ToDouble(ExpScreen.Text); i++)
                        {
                            res *= i;
                        }
                        ExpScreen.Text = res.ToString();
                        Warning.Content = "";
                    }
                    else if (num == 0)
                    {
                        ExpScreen.Text = "1";
                        Warning.Content = "";
                    }
                    else
                    {
                        Warning.Content = "Число отрицательное или не целое";
                        Clean();
                    }
                    break;
                case "xⁿ":
                    ExpScreen.Text = Math.Pow(result, ToDouble(ExpScreen.Text)).ToString();
                    powering = false;
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
                if (result != 0 && ExpScreen.Text == "0")
                {
                    Clean();
                    Warning.Content = "Деление на ноль";
                }
                else result /= ToDouble(ExpScreen.Text);
            }
            else if (lastOperation == "mod")
            {
                if (result != 0 && ExpScreen.Text == "0")
                {
                    Warning.Content = "Деление на ноль";
                    Clean();
                }
                else result %= ToDouble(ExpScreen.Text);
            }
            if (powering)
            {
                result = Math.Pow(result, ToDouble(ExpScreen.Text));
                powering = false;
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
            input = true;
        }

        private void Operation_Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            string button = clicked.Content.ToString();

            if (button == "+")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "+/-")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                ExpScreen.Text = (-ToDouble(ExpScreen.Text)).ToString();
                Warning.Content = "";
            }
            else if (button == ".")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
            else if (button == "sin")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "asin")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "cos")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "acos")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "tan")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "atan")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "xⁿ")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                if (start)
                {
                    start = false;
                    result = ToDouble(ExpScreen.Text);
                    powering = true;
                    ExpScreen.Text = "0";
                    Warning.Content = "";
                }
                else
                {
                    DoOperation(lastOperation);
                    powering = true;
                    lastOperation = "";
                    Warning.Content = result.ToString();
                }
            }
            else if (button == "log")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "exp")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "ln")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "x!")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
                DoOperation(button);
            }
            else if (button == "mod")
            {
                if (!input)
                {
                    Warning.Content = "Требуется ввод";
                    return;
                }
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
            else // Кнопка удаления последнего символа
            {
                string newNumber = "";
                for (int i = 0; i < ExpScreen.Text.ToString().Length - 1; i++)
                {
                    newNumber += ExpScreen.Text.ToString()[i];
                }
                ExpScreen.Text = newNumber;
                if (newNumber == "") ExpScreen.Text = "0";
            }
        }
    }
}
