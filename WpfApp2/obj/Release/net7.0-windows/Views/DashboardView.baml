   M S B A M L   `   `   `  ���� B  >WpfApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2      WpfApp2.Dashboard   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  !localclr-namespace:WpfApp2  $	��700��$	��900��   MaterialDesignBody#���   ��Z�   � 	Resources  #   WpfApp2.ListToStringConverter  ListToStringConverter��%   &	         .��#  MaterialDesignThemes.Wpf.Card    MaterialDesignPaper#���     UniformCornerRadius$ 15Q�$��25q�.  (MaterialDesignThemes.Wpf.ElevationAssist   	Elevation$	 Dp4=�.���  � ColumnDefinitions �� $	��135 ��� $
��381* ��� $
��334* � � RowDefinitions �� $��70 ��� .+�)  #MaterialDesignThemes.Wpf.DialogHost  -     Name$ RecordDialogHost��$��Center=�$��Center=�   DialogContent �� #���    MaterialDesignDivider#���  $��1q� �� CornerRadius$ 8��._�� $	��400��.+��� $��16q�.+��� $	��100��$	��150��$��0,0,0,8q����� *SelectedBook.coverLink+�� $��SemiBold"�$��18(�$��0,0,0,8q����� *SelectedBook.title+�� $��0,0,0,8q�.+��� $
��Bold"�$��Autorzy:���� ���� *SelectedBook.authors+ �� 	Converter# [ �� $��0,0,0,8q�.+��� $
��Bold"�$��Data wydania:���� ���� *SelectedBook.publishedDate+�� $��0,0,0,8q�.+��� $
��Bold"�$��Wydawca:���� ���� *SelectedBook.publisher+�� $��0,0,0,8q�.+��� $
��Bold"�$��ISBN:���� ���� *" SelectedBook.industryIdentifiers+# [ �� $��0,0,0,8q�.+��� $
��Bold"�$��Liczba stron:���� ���� *SelectedBook.pageCount+�� $��0,0,0,8q�.+��� $
��Bold"�$��Średnia ocen:���� ���� *!SelectedBook.averageRatingValue+	 �� StringFormat$	 {0:N1}���� $
��Auto=�$	��200��.���� $��0,0,0,8q�.+��� $
��Bold"�$��Opis:���� $
��Wrap=����� *SelectedBook.description+�� -   $��Zamknij��  -   $ 
DialogHost��
   CloseOnClickAway$

 True��.��'  !MaterialDesignThemes.Wpf.PopupBox     PopupUniformCornerRadius$ 10Q�$��Right=�$��0,30,-694,15q�   PlacementMode$ BottomAndAlignRightEdges=�   	StaysOpen$ False��$��25��   PopupContent. �� .+��� $��	16 10 0 6q� �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Center=�$��Tryb Ciemny��d� -    d�Name$ themeToggle�� � Cursor$
 Hand�� � ToolTip$ Enable Dark Mode��$��12 0 8 0q� d� 	IsChecked �� *IsDarkTheme+�� -    ��Name$ Btn_Exit��$ Close Application��$��Zamknij aplikację���� $��
Biblioteka��$	��Top=�$
��Left=�
����� �$��26(�$��	14,27,0,0q�$��SemiBold"�$��32��$	��121���� $��1��.+��� -    ��Name$ TitleCheckBox��$��	10,55,0,0q�$��Tytuł��$��30��$��14(�  MenuCheckBoxStyle#��[ $	��120���� -   $ AuthorscheckBox��$��Autorzy��$��33��$��10,0,0,0q�$��14(�#��[ $	��120���� -   $ PublishedDatecheckBox��$��Data wydania��$��30��$��10,0,0,0q�$��14(�#��[ $	��120���� -	   $ PublishercheckBox��$��Wydawca��$��10,0,0,0q�$��30��$��14(�#��[ $	��120���� -
   $ ISBNCheckBox��$��10,0,0,0q�$
��ISBN��$��30��$��14(�#��[ $	��120���� -   $ PageCountbcheckBox��$��Liczba stron��$��30��$��10,0,0,0q�$��14(�#��[ $	��120���� -   $ averageRatingcheckBox��$��Średnia ocen��$��10,0,0,0q�$��30��$��14(�#��[ $	��120���� -   $ DescriptionbcheckBox��$
��Opis��$��33��$��10,0,0,0q�$��14(�#��[ $	��120��$y�	Collapsed=��� -    ��Name$ input��$	��346��$��40��$��Center=�$��Center=�$	��Top=�$��1��$��0,24,0,0q��� -    ��Name$ SzukajComboBox��$��40��$��2��$��Center=�$��23,24,196,0q�$	��Top=��� -   $	��100��$��40��$��
167,22,0,0q�$��2��$
��Left=�$	��Top=�.���� $	��5,0q�Szukaj�� -    ��Name$
 List��$��1��$��2��$��1��$��	5,0,22,10q�$��Stretch=����� *Items+ �� View �� .P���  �� CellTemplate �� .R��� $��80��$	��120������ *	coverLink+�� $��Tytuł�� �� DisplayMemberBinding �� *title+�� $��Autorzy�� �� .R��� ���� *	authors+# [ �� $��Data wydania�� �� *publishedDate+�� $��Wydawca�� �� *	publisher+�� $
��ISBN�� �� .R��� ���� *industryIdentifiers+# [ �� $��Liczba stron�� �� *	pageCount+�� $��Średnia ocen�� �� *averageRatingValue+$	 {0:N1}��