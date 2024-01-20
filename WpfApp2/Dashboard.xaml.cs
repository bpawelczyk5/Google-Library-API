// Dashboard.xaml.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;
using static WpfApp2.Dashboard;

namespace WpfApp2
{
    // Konwerter do zamiany listy na ciąg znaków w formie tekstowej
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> authors)
            {
                return string.Join(", ", authors);
            }

            if (value is List<IndustryIdentifier> identifiers)
            {
                return string.Join(", ", identifiers.Select(id => $"{id.type}: {id.identifier}"));
            }

            return value; // Zwraca oryginalną wartość, jeśli nie jest to List<string> lub List<IndustryIdentifier>
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Klasa odpowiadająca za widok panelu nawigacyjnego
    public partial class Dashboard : UserControl
    {
        // Stała przechowująca adres URL do usługi Books API
        public const string bookURL = "https://www.googleapis.com/books/v1/volumes/";

        // Obiekt HttpClient do komunikacji z usługą Books API
        HttpClient bookClient = new HttpClient();

        // Definicja ObservableCollection do przesyłania danych do interfejsu użytkownika
        public ObservableCollection<BookObject> Items { get; set; } = new ObservableCollection<BookObject>();

        // Konstruktor dla klasy Dashboard
        public Dashboard()
        {
            InitializeComponent();

            // Inicjalizacja klienta HttpClient i ustawienie adresu bazowego
            bookClient.BaseAddress = new Uri(bookURL);

            // Inicjalizacja elementów interfejsu użytkownika
            Init();

            // Ustawienie kontekstu danych dla UserControl na sam siebie
            DataContext = this;

            // Ustawienie domyślnych zaznaczeń dla opcji wyświetlania informacji o książce
            TitleCheckBox.IsChecked = true;
            AuthorscheckBox.IsChecked = true;
            PublishedDatecheckBox.IsChecked = true;
            PublishercheckBox.IsChecked = true;
            ISBNCheckBox.IsChecked = true;
            averageRatingcheckBox.IsChecked = true;
            PageCountbcheckBox.IsChecked = true;
            DescriptionbcheckBox.IsChecked = true;
        }

        // Właściwość zależna przechowująca zaznaczoną książkę
        public BookObject SelectedBook
        {
            get { return (BookObject)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }

        // Zdefiniowanie właściwości zależnej dla zaznaczonej książki
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(BookObject), typeof(Dashboard), new PropertyMetadata(null));

        // Metoda inicjalizująca elementy interfejsu użytkownika
        public void Init()
        {
            // Dodanie elementów do ComboBox do wyboru kategorii wyszukiwania
            string[] searchItems = new string[] { "Po wszystim", "Po tytule", "Po autorze" };
            foreach (string item in searchItems)
            {
                SzukajComboBox.Items.Add(item);
            }

            // Ustawienie domyślnej wartości dla ComboBox
            SzukajComboBox.SelectedIndex = 0;
        }

        // Obsługa zdarzenia zmiany zaznaczenia w liście książek
        public void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                // Ustawienie zaznaczonej książki
                SelectedBook = (BookObject)List.SelectedItem;

                // Pobranie pozycji kliknięcia myszy
                var mousePosition = Mouse.GetPosition(Application.Current.MainWindow);

                // Ustawienie pozycji RecordDialogHost
                RecordDialogHost.Margin = new Thickness(mousePosition.X - RecordDialogHost.ActualWidth / 2, mousePosition.Y - RecordDialogHost.ActualHeight / 2, 0, 0);

                // Wyświetlenie okna dialogowego
                RecordDialogHost.IsOpen = true;
            }
        }

        // Obsługa zdarzenia zamykania okna dialogowego
        public void ClosePopup(object sender, RoutedEventArgs e)
        {
            // Zamknięcie okna dialogowego
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        // Obsługa zdarzenia przycisku wyszukiwania książek
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            String urlParameters;
            HttpResponseMessage response;

            // Wartości dla parametrów zapytania zależne od wybranej kategorii wyszukiwania
            {
                string searchFor = SzukajComboBox.SelectedItem.ToString();
                urlParameters = "?q=";

                if (searchFor == "Po autorze")
                {
                    urlParameters += "inauthor:" + input.Text;
                }
                else if (searchFor == "Po tytule")
                {
                    urlParameters += "intitle:" + input.Text;
                }
                else
                {
                    urlParameters += input.Text;
                }

                urlParameters += "&maxResults=" + 25;
            }

            // Wysłanie zapytania do usługi Books API
            response = bookClient.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parsowanie odpowiedzi w formacie JSON
                JObject bookJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                // Wyczyszczenie istniejących elementów przed dodaniem nowych
                Items.Clear();

                // Pobranie listy książek
                JArray books = (JArray)bookJson["items"];

                if (books != null)
                {
                    // Dodanie każdej książki do kolekcji
                    foreach (var book in books)
                    {
                        Items.Add(ParseBook((JObject)book));
                    }
                }
            }
        }

        // Metoda parsująca dane o książce z obiektu JSON
        public BookObject ParseBook(JObject bookJson)
        {
            BookObject book = new BookObject();

            // Pobranie obiektu zawierającego informacje o książce
            JObject volumeInfoObject = (JObject)bookJson["volumeInfo"];
            JObject searchInfoObject = (JObject)bookJson["searchInfo"];

            StringBuilder sb = new StringBuilder();

            // Sprawdzenie zaznaczonych opcji wyświetlania informacji o książce
            if (TitleCheckBox.IsChecked == true)
            {
                book.title = ParseString(volumeInfoObject, "title");
                sb.Append("Tytul: " + book.title + " \r\n");
            }
            if (ISBNCheckBox.IsChecked == true && volumeInfoObject["industryIdentifiers"] != null)
            {
                JArray industryIdentifiersArray = (JArray)volumeInfoObject["industryIdentifiers"];

                foreach (var identifier in industryIdentifiersArray)
                {
                    book.industryIdentifiers.Add(new IndustryIdentifier
                    {
                        type = ParseString((JObject)identifier, "type"),
                        identifier = ParseString((JObject)identifier, "identifier")
                    });
                }
            }
            if (averageRatingcheckBox.IsChecked == true)
            {
                book.averageRatingValue = ParseDouble(volumeInfoObject, "averageRating");
                sb.Append("Ocena: " + book.averageRatingValue + " \r\n");
            }

            if (PublishercheckBox.IsChecked == true)
            {
                book.publisher = ParseString(volumeInfoObject, "publisher");
                sb.Append("Wydawca: " + book.publisher + " \r\n");
            }
            if (PublishedDatecheckBox.IsChecked == true)
            {
                book.publishedDate = ParseString(volumeInfoObject, "publishedDate");
                sb.Append("Data wydania: " + book.publishedDate + " \r\n");
            }
            if (PageCountbcheckBox.IsChecked == true)
            {
                book.pageCount = ParseInt(volumeInfoObject, "pageCount");
                sb.Append("Liczba stron: " + book.pageCount + " \r\n");
            }
            if (AuthorscheckBox.IsChecked == true)
            {
                JArray authorsArray = (JArray)volumeInfoObject["authors"];

                sb.Append("Autorzy: ");

                if (authorsArray != null)
                {
                    foreach (var author in authorsArray)
                    {
                        book.authors.Add(author.ToString());
                        sb.Append(author + ", ");
                    }
                }

                // Usunięcie przecinka i spacji na końcu listy autorów
                if (book.authors.Count > 0)
                {
                    sb.Remove(sb.Length - 2, 2);
                }

                sb.Append("\r\n");
            }
            if (DescriptionbcheckBox.IsChecked == true)
            {
                book.description = ParseString(volumeInfoObject, "description");
                sb.Append(" \r\n");
                sb.Append("Opis: " + book.description + " \r\n");
            }

            // Pobranie linku do okładki, jeśli dostępny
            if (volumeInfoObject["imageLinks"] != null)
            {
                JObject imageLinks = (JObject)volumeInfoObject["imageLinks"];
                book.coverLink = ParseString(imageLinks, "thumbnail");
            }

            return book;
        }

        // Metoda parsująca ciąg znaków z obiektu JSON
        public static string ParseString(JObject bookJson, string toParse)
        {
            if (bookJson == null)
            {
                return "";
            }
            else if (bookJson[toParse] == null)
            {
                return "";
            }
            else return (string)bookJson[toParse];
        }

        // Metoda parsująca liczbę całkowitą z obiektu JSON
        public static int ParseInt(JObject bookJson, string toParse)
        {
            if (bookJson == null)
            {
                return 0;
            }
            else if (bookJson[toParse] == null)
            {
                return 0;
            }
            else return (int)bookJson[toParse];
        }

        // Metoda parsująca liczbę zmiennoprzecinkową z obiektu JSON
        public static double? ParseDouble(JObject bookJson, string toParse)
        {
            if (bookJson == null)
            {
                return null;
            }
            else if (bookJson[toParse] == null)
            {
                return null;
            }
            else if (double.TryParse(bookJson[toParse].ToString(), out double result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        // Klasa reprezentująca obiekt książki
        [Serializable]
        public class BookObject
        {
            public BookObject()
            {
                authors = new List<string>();
                industryIdentifiers = new List<IndustryIdentifier>();
            }
            public string isbn { get; set; }
            public string title { get; set; }
            public List<string> authors { get; set; }
            public string publisher { get; set; }
            public string publishedDate { get; set; }
            public string description { get; set; }
            public int pageCount { get; set; }
            public string coverLink { get; set; }
            public double? averageRating { get; set; } // Użycie double dla ocen liczbowych
            public double? averageRatingValue { get; set; } // Użycie double dla ocen liczbowych

            public List<IndustryIdentifier> industryIdentifiers { get; set; }
        }

        // Klasa reprezentująca identyfikator branżowy książki
        public class IndustryIdentifier
        {
            public string type { get; set; }
            public string identifier { get; set; }
        }

        // Kod motywu aplikacji
        public bool IsDarkTheme { get; set; }
        public readonly PaletteHelper paletteHelper = new PaletteHelper();
        //==============================>

        // Metoda do przełączania motywu jasnego/ciemnego
        public void toggleTheme(object sender, RoutedEventArgs e)
        {
            // Kod motywu aplikacji
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
            //==============================>
        }

        // Metoda do zamknięcia aplikacji
        public void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
