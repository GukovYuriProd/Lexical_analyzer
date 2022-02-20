using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Media.Animation;

namespace Lexical_Analizer
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HelpBorder.Width = 0;
        }

        #region DOCS
        //Входной язык содержит логические выражения, разделенные символом ; (точка с запятой).
        //Логические выражения состоят из
        //идентификаторов,
        //констант 0 и 1,
        //знака присваивания (:=),
        //знаков операций or, xor, and, not и
        //круглых скобок.
        #endregion

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

        #region Animation Block
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation headerminimyse = new DoubleAnimation();
            headerminimyse.From = CorrectionLayout.ActualHeight;
            headerminimyse.To = 50;
            headerminimyse.Duration = TimeSpan.FromSeconds(0.3);
            CorrectionLayout.BeginAnimation(Border.HeightProperty, headerminimyse);
            DoubleAnimation ButtonsToDifSides = new DoubleAnimation();
            ButtonsToDifSides.From = ButtonsUseless.ActualHeight;
            ButtonsToDifSides.To = 0;
            ButtonsToDifSides.Duration = TimeSpan.FromSeconds(0.3);
            ButtonsUseless.BeginAnimation(Border.HeightProperty, ButtonsToDifSides);
            DoubleAnimation ButtonOpacity = new DoubleAnimation();
            ButtonOpacity.From = ButtonsUseless.Opacity;
            ButtonOpacity.To = 0;
            ButtonOpacity.Duration = TimeSpan.FromSeconds(0.3);
            ButtonsUseless.BeginAnimation(Border.OpacityProperty, ButtonOpacity);
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e) //потеря фокуса на строке
        {
            if (SearchLine.Text.Length == 0)
            {
                WorkArea.Children.Clear();
                WorkAreaZone.Opacity = 0;
                WorkAreaZone.Height = 0;
                DoubleAnimation headerminimyse = new DoubleAnimation();
                headerminimyse.From = CorrectionLayout.ActualHeight;
                headerminimyse.To = 230;
                headerminimyse.Duration = TimeSpan.FromSeconds(0.3);
                CorrectionLayout.BeginAnimation(Border.HeightProperty, headerminimyse);
                DoubleAnimation ButtonsToDifSides = new DoubleAnimation();
                ButtonsToDifSides.From = ButtonsUseless.ActualHeight;
                ButtonsToDifSides.To = 33;
                ButtonsToDifSides.Duration = TimeSpan.FromSeconds(0.3);
                ButtonsUseless.BeginAnimation(Border.HeightProperty, ButtonsToDifSides);
                DoubleAnimation ButtonOpacity = new DoubleAnimation();
                ButtonOpacity.From = ButtonsUseless.Opacity;
                ButtonOpacity.To = 1;
                ButtonOpacity.Duration = TimeSpan.FromSeconds(0.3);
                ButtonsUseless.BeginAnimation(Border.OpacityProperty, ButtonOpacity);
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            KASTIL.Focus();
        } //снятие фокуса с основного окна ввода текста
        #endregion
        private async void StartAnalize(object sender, MouseButtonEventArgs e) //нажатие на кнопку поиска
        {
            string _InputLine = SearchLine.Text.ToString().ToLower() + ' ';
            if (_InputLine.Length > 1)
            {
                WorkAreaZone.Height = 15;
                WorkAreaZone.Opacity = 0.17;
                WorkArea.Children.Clear();
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
                            //отрисовка всей визуальной появляющейся информации
                            WorkAreaZone.Height = WorkAreaZone.Height + 37;
                            Border newLine = new Border();
                            newLine.Width = 490;
                            newLine.Height = 30;
                            newLine.HorizontalAlignment = HorizontalAlignment.Left;
                            newLine.Margin = new Thickness(0, 7, 0, 0);
                            newLine.CornerRadius = new CornerRadius(15);
                            newLine.Background = new SolidColorBrush(Colors.Transparent);
                            newLine.BorderThickness = new Thickness(0.6);
                            newLine.Opacity = 0.7;
                            newLine.BorderBrush = new SolidColorBrush(Colors.Gray);
                            Label label = new Label();
                            label.FontSize = 14;
                            label.Content = '"' + _word + '"' + " IS " + _wordClass; ;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.HorizontalAlignment = HorizontalAlignment.Left;
                            label.Margin = new Thickness(10, 0, 0, 0);
                            newLine.Child = label;
                            WorkArea.Children.Add(newLine);
                            await Task.Delay(_runningDelay);
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

        private void ClearSearch(object sender, MouseButtonEventArgs e)
        {
            WorkArea.Children.Clear();
            SearchLine.Clear();
            WorkAreaZone.Opacity = 0;
            WorkAreaZone.Height = 0;
            DoubleAnimation headerminimyse = new DoubleAnimation();
            headerminimyse.From = CorrectionLayout.ActualHeight;
            headerminimyse.To = 230;
            headerminimyse.Duration = TimeSpan.FromSeconds(0.3);
            CorrectionLayout.BeginAnimation(Border.HeightProperty, headerminimyse);
            DoubleAnimation ButtonsToDifSides = new DoubleAnimation();
            ButtonsToDifSides.From = ButtonsUseless.ActualHeight;
            ButtonsToDifSides.To = 33;
            ButtonsToDifSides.Duration = TimeSpan.FromSeconds(0.3);
            ButtonsUseless.BeginAnimation(Border.HeightProperty, ButtonsToDifSides);
            DoubleAnimation ButtonOpacity = new DoubleAnimation();
            ButtonOpacity.From = ButtonsUseless.Opacity;
            ButtonOpacity.To = 1;
            ButtonOpacity.Duration = TimeSpan.FromSeconds(0.3);
            ButtonsUseless.BeginAnimation(Border.OpacityProperty, ButtonOpacity);
        }

        #region BackMoves
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void GoogleSearchPSEVDOLEFT(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Скажу по секрету, я не Гугл;)");
        }

        private void GoogleSearchPSEVDORIGHT(object sender, MouseButtonEventArgs e)
        {
            ProcessStartInfo psiOpt = new ProcessStartInfo(@"cmd.exe", @"/C start https://www.youtube.com/watch?v=dQw4w9WgXcQ /all");
            Process procCommand = Process.Start(psiOpt);

        }
        private bool HelpTooTip = false;
        private void HelpButton_Toogle(object sender, MouseButtonEventArgs e)
        {
            if (HelpTooTip)
            {
                HelpTooTip = false;
                DoubleAnimation tipmaximize = new DoubleAnimation();
                tipmaximize.From = HelpBorder.ActualHeight;
                tipmaximize.To = 0;
                tipmaximize.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.HeightProperty, tipmaximize);
                DoubleAnimation tipmaximizex = new DoubleAnimation();
                tipmaximizex.From = HelpBorder.ActualWidth;
                tipmaximizex.To = 0;
                tipmaximizex.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.WidthProperty, tipmaximize);
                DoubleAnimation tipopacity = new DoubleAnimation();
                tipopacity.From = HelpBorder.Opacity;
                tipopacity.To = 0;
                tipopacity.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.OpacityProperty, tipopacity);
            } else
            {
                HelpTooTip = true;
                DoubleAnimation tipmaximize = new DoubleAnimation();
                tipmaximize.From = HelpBorder.ActualHeight;
                tipmaximize.To = 190;
                tipmaximize.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.HeightProperty, tipmaximize);
                DoubleAnimation tipmaximizex = new DoubleAnimation();
                tipmaximizex.From = HelpBorder.ActualWidth;
                tipmaximizex.To = 290;
                tipmaximizex.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.WidthProperty, tipmaximizex);
                DoubleAnimation tipopacity = new DoubleAnimation();
                tipopacity.From = HelpBorder.Opacity;
                tipopacity.To = 1;
                tipopacity.Duration = TimeSpan.FromSeconds(0.5);
                HelpBorder.BeginAnimation(Border.OpacityProperty, tipopacity);
            }
        }
        #region Work with file
        private void FileEnter(object sender, DragEventArgs e)
        {
            FileEnterPool.Height = 200;
            CorrectionLayout.Height = 150; //250
            SearchLine.Height = 0;
        }
        #endregion

        private void FileLeave(object sender, DragEventArgs e)
        {
            FileEnterPool.Height = 40;
            CorrectionLayout.Height = 230;
            SearchLine.Height = 25;
        }

        private void FileEnterPool_Drop(object sender, DragEventArgs e)
        {
            FileEnterPool.Height = 40;
            CorrectionLayout.Height = 230;
            SearchLine.Height = 25;
            SearchLine.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            SearchLine.Focus();
        }
    }
}
