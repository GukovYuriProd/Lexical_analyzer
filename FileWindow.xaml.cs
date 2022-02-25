using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lexical_Analizer
{
    public partial class FileWindow : Window
    {
        public string _FilePath;
        public FileWindow(string pathLine)
        {
            _FilePath = pathLine;
            InitializeComponent();
            FilePathLabel.Content = _FilePath;
            TextVisualizing();
        }
        #region dictionaries
        string _identifiers = "abcdefghijklmnopqrstuvwxyz0123456789"; //словарь для идентификаторов StatusBar -> Ident 1
        List<string> _constatnts = new List<string> { "1", "0" }; //словарь для констант StatusBar -> Const 2
        string _equlifiers = ":="; //словарь для знака присваивания StatusBar -> Equal 3
        List<string> _operations = new List<string> { "or", "xor", "and", "not", "&", "|", "¬", "^" }; //словарь для логических операций StatusBar -> Logic 4
        string _bracket = "()"; //словарь для скобок StatusBar -> Brack 5
        string _endline = ";"; //словарь для знаков завершения строки StatusBar -> Ended 6
        int _state = 1; //стандартное положение свитчера StatusBar -> Check 0
        int _prevstate; //значение предыдущего состояния автомата
        bool _DefFlag = true; //флаг для первого ввода
        string _word; //текущее обрабатываемое слово
        string _wordClass; //текущая классификация слова
        int _runningDelay = 600; //задержка выполнения итераций
        #endregion
        #region Работа с окном
        private void WindowMoving(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ExitWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion
        List<string> _allLines = new List<string>();
        public async void TextVisualizing() //вывод в левую часть окна всех строк из файла
        {
            using (StreamReader reader = new StreamReader(_FilePath))
            {
                string line;
                int _LineCounter = 1;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    _allLines.Add(line);
                    StackPanel newline = new StackPanel();
                    newline.Orientation = Orientation.Horizontal;
                    TextBlock linenumber = new TextBlock();
                    linenumber.FontSize = 15;
                    linenumber.Width = 30;
                    linenumber.VerticalAlignment = VerticalAlignment.Center;
                    linenumber.Text = _LineCounter.ToString();
                    linenumber.Foreground = new SolidColorBrush(Color.FromRgb(43, 145, 175));
                    newline.HorizontalAlignment = HorizontalAlignment.Left;
                    linenumber.Margin = new Thickness(0, 0, 5, 0);
                    newline.Children.Add(linenumber);
                    newline.Children.Add(DrawOneLine(line));
                    fileReader.Children.Add(newline);
                    _LineCounter++;
                }
            }
        }

        private Button DrawOneLine(string _LineFromFile)
        {
            Button OneLine = new Button();
            OneLine.FontSize = 15;
            OneLine.Foreground = new SolidColorBrush(Colors.White);
            OneLine.Height = 21;
            OneLine.Content = _LineFromFile;
            OneLine.Background = new SolidColorBrush(Colors.Transparent);
            OneLine.BorderThickness = new Thickness(0);
            OneLine.Click += ClickOnLine;
            return OneLine;
        }

        private void ClickOnLine(object sender, RoutedEventArgs e)
        {
            fileVisualizator.Children.Clear();
            string _AnanlizedLine = (sender as Button).Content.ToString();
            StartAnalizeFileLine(_AnanlizedLine);
        }

        private void StartAnalizeFileLine(string _InputLine) //нажатие на кнопку поиска
        {
            _InputLine = _InputLine.ToLower() + ' ';
            if (_InputLine.Length > 1)
            {
                //WorkArea
                _state = 1;
                _prevstate = 0;
                _DefFlag = true;
                _word = null;
                _wordClass = null;
                if (_InputLine != "")
                {
                    foreach (char _simbol in _InputLine) //перебор всей строки по символам
                    {
                        _state = StateSetter(_simbol);
                        if ((_state != _prevstate) && (_DefFlag == false) && !(_state == 4 && _prevstate == 1))
                        {
                            //fileVisualizator
                            StackPanel newlineindatagrid = new StackPanel();
                            newlineindatagrid.Orientation = Orientation.Horizontal;
                            Border leftpad = new Border();
                            leftpad.Height = 23;
                            leftpad.Width = 120;
                            leftpad.BorderThickness = new Thickness(1);
                            leftpad.BorderBrush = new SolidColorBrush(Colors.Gray);
                            TextBlock textinleftpad = new TextBlock();
                            textinleftpad.FontSize = 15;
                            textinleftpad.Foreground = new SolidColorBrush(Colors.White);
                            textinleftpad.HorizontalAlignment = HorizontalAlignment.Center;
                            textinleftpad.VerticalAlignment = VerticalAlignment.Center;
                            textinleftpad.Text= _word;
                            leftpad.Child = textinleftpad;
                            Border rightpad = new Border();
                            rightpad.Height = 23;
                            rightpad.Width = 240;
                            rightpad.BorderThickness = new Thickness(1);
                            rightpad.BorderBrush = new SolidColorBrush(Colors.Gray);
                            TextBlock textinrightpad = new TextBlock();
                            textinrightpad.FontSize = 15;
                            textinrightpad.Foreground = new SolidColorBrush(Colors.White);
                            textinrightpad.HorizontalAlignment = HorizontalAlignment.Center;
                            textinrightpad.VerticalAlignment = VerticalAlignment.Center;
                            textinrightpad.Text = _wordClass;
                            rightpad.Child = textinrightpad;
                            newlineindatagrid.Children.Add(leftpad);
                            newlineindatagrid.Children.Add(rightpad);
                            fileVisualizator.Children.Add(newlineindatagrid);
                            //label.Content = '"' + _word + '"' + " IS " + _wordClass;
                        }
                        switch (_state) //автомат состояний
                        {
                            case 0: //Basic
                                _word = "";
                                _prevstate = 0;
                                _DefFlag = true;
                                if (_simbol != ' ') MessageBox.Show("Вы ввели неверный символ '" + _simbol.ToString() + "'");
                                break;
                            case 1: //Ident
                                StateRouter(_simbol, 1, "an Identificator");
                                break;
                            case 2: //Const
                                StateRouter(_simbol, 2, "a Constant");
                                break;
                            case 3: //Equal
                                StateRouter(_simbol, 3, "an Equality sign");
                                break;
                            case 4: //Logic
                                StateRouter(_simbol, 4, "a Logical operation");
                                break;
                            case 5: //Brack
                                StateRouter(_simbol, 5, "a Bracket");
                                break;
                            case 6: //Ended
                                StateRouter(_simbol, 6, "an End sign");
                                break;
                        }
                    }
                }
            }

        }

        private void StateRouter(char _localSimbol, int _localState, string _localClassification) //логический маршрутизатор и форматор "слова" для вывода в строку
        {
            if (_DefFlag)
            {
                _word += _localSimbol;
                _DefFlag = !_DefFlag;
            }
            else
            {
                if (_state == _prevstate)
                {
                    _word += _localSimbol;
                }
                else
                {
                    if (_localState != 4)
                    {
                        _word = "";
                        _word += _localSimbol;
                    }
                    else
                    {
                        _word += _localSimbol;
                    }
                }
            }
            _wordClass = _localClassification;
            _prevstate = _localState;
        }

        private int StateSetter(char _InputSimbol) //логический переключатель состояний
        {
            int _returnState = 0;
            if (_identifiers.Contains(_InputSimbol) && (_word != "" && _prevstate == 1 || !_constatnts.Contains(_InputSimbol.ToString())) && !_operations.Contains(_word + _InputSimbol)) _returnState = 1;
            else if (_constatnts.Contains(_InputSimbol.ToString())) _returnState = 2;
            else if (_equlifiers.Contains(_InputSimbol)) _returnState = 3;
            else if (_operations.Contains(_word + _InputSimbol)) _returnState = 4;
            else if (_bracket.Contains(_InputSimbol)) _returnState = 5;
            else if (_endline.Contains(_InputSimbol)) _returnState = 6;
            return _returnState;
        }

    }
}
