   M S B A M L   `   `   `  ���� K  GLexical Analizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Lexical_AnalizerLexical_Analizer  !    Lexical_Analizer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:Lexical_Analizer       � Title$  Lexical Analizer��$	��750��$
��1000��$	��300��$	��600�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=�.��� -   .+��� -    J� Opacity$ 1Q�
������  J� 	AllowDrop . ._�� .+��� -   $��17��$��\Icons\home.png��$
��Left=�$	��Top=� � Cursor$
 Hand��$��5q�� $	��Top=�$��Right=�$��5q�.+��� -   $��\Icons\help.png��$��17��$��17��$��Right=�$	��Top=�$
 Hand��� -    �Name$ 
HelpBorder��$��0��.+��� $	��260��$	��190��$��0,0,30,0q�
���   �$	��0.6q� �� CornerRadius$ 21��$	 0.3Q��� $��0,0,30,0q�$	 0.8Q�.+��� $��16(�$��Алфавит��$��Center=��� $��15(�$��5,0,0,0q�$A��;Латинские буквы любого регистра���� $��15(�$��5,0,0,0q�$,��&Знак присваивания ':='���� $��15(�$��5,0,0,0q�$#��Константы '1' и '0'���� $��15(�$��5,0,0,0q�$1��+Знаки и слова операций: ���� $��15(�$��5,0,0,0q�$��  > xor, or, and, not���� -    ��Name$ KASTIL��$��Center=�$��Bottom=�$ 0Q��� $��14��$��14��$��Right=�$��Bottom=�$��0,0,5,5q�$��\Icons\cross.png���� -   $��14��$��14��$��Right=�$��Bottom=�$��0,0,5,5q�$ 0Q�	 K� ZIndex$	 1��$
 Hand���� 
 �� Orientation$
 Vertical=�.+�� -   $ Seacrher��$��Center=�.+��� $
 Vertical=�.+��� -	    ��Name$ CorrectionLayout��$	��230���� -
    ��Name$ CentralLogo��$��\Icons\suai-main.png��$	��310��$��Center=�� .+��� -   $ FileEnterPool��$��40��$	��530��
���   �$	��0.6q�$ 21��$
 0.17Q�$��0,5,0,25q�._�� -   $ FileEnteredTip��$��Center=�$��Center=�$ 0Q�.+��� $
 Vertical=�.+��� $��80��$	 0.7Q�$��\Icons\file.png���� $��17(�$:��4Поместите файл в эту область��$��Center=�$��0,10,0,0q��� -   $��17��$��35��$��\Icons\Trash.png��$
 Hand��$	 0.3Q�$��Right=�$��
0,-20,15,0q� � ToolTip$ Начать анализ���� -   $��15��$��35��$��\Icons\Seacrhc.png��$
 Hand��$	 0.3Q�$
��Left=�$��
15,-20,0,0q�$ Начать анализ���� -   $ 
SearchLine��$��15(�
������ $��25��$��Center=�$��40,-17,40,0q�$��0q�$	��450��$
 0.85Q��� -    ��Name$ ButtonsUseless��$
 
Horizontal=�$��Center=�$	��250��.+��� -   $	��120��$��33�����#f4f4f2$ 5��$
 Hand��._�� .+��� $	��120��$��30��$ 0Q��� $"��Поиск в системе��$��Center=�$��Center=�$��14(�
���   �$	 0.8Q�$��Normal"��� -   $ RightButton��$	��120��$��33�����#f4f4f2$ 5��$��10,0,0,0q�$
 Hand��._�� .+��� $	��120��$��30��$ 0Q��� $��Мне повезет!��$��Center=�$��Center=�$��14(�
���   �$	 0.8Q�$��Normal"�� .+��� -   $ WorkAreaZone��$��15��$	��520��
������ 
���   �$	��0.6q�$ 21��$ 0Q�$	��Top=��� -   $ WorkArea��$
 Vertical=�$	��400��$��Center=�$��0,5,0,0q�