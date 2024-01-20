// LoginView.xaml.cs
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Input;
using BCrypt.Net;

namespace WpfApp2
{
    public partial class LoginView : UserControl
    {
        // Konstruktor dla LoginView
        public LoginView()
        {
            InitializeComponent();
        }

        // Kod motywu aplikacji
        public bool IsDarkTheme { get; set; }
        public readonly PaletteHelper paletteHelper = new PaletteHelper();
        public Func<SqlConnection> SqlConnectionFactory;

        //==============================>

        // Obsługa zdarzenia dla przełączania między jasnym a ciemnym motywem
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

        // Obsługa zdarzenia dla wyjścia z aplikacji
        public void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Obsługa zdarzenia dla przełączania się do widoku rejestracji
        public void SwitchToRegistration_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainContent.Content = new RegistrationView();
        }

        // Obsługa zdarzenia dla przycisku logowania
        public void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-RV3JHLM;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");

            try
            {
                // Otwarcie połączenia z bazą danych, jeśli jest zamknięte
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                // Zapytanie do bazy danych w celu pobrania zapisanego zahashowanego hasła dla danego użytkownika
                String query = "SELECT password FROM dbo.users WHERE username=@username";
                SqlCommand sqlCom = new SqlCommand(query, sqlConnection);
                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.Parameters.AddWithValue("@username", userNameBox.Text);

                string storedHashedPassword = sqlCom.ExecuteScalar() as string;

                // Sprawdzenie poprawności wprowadzonego hasła z zapisanym zahashowanym hasłem
                if (!string.IsNullOrEmpty(storedHashedPassword) && BCrypt.Net.BCrypt.Verify(passwordBox.Password, storedHashedPassword))
                {
                    // Przejście do widoku panelu nawigacyjnego po poprawnym zalogowaniu
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    mainWindow.MainContent.Content = new Dashboard();
                }
                else
                {
                    // Wyświetlenie komunikatu w przypadku błędnego loginu lub hasła
                    warningLabel.Content = "Nazwa albo hasło są niepoprawne!";
                }
            }
            catch (Exception ex)
            {
                // Wyświetlenie komunikatu w przypadku błędu
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Zamknięcie połączenia z bazą danych
                sqlConnection.Close();
            }
        }
    }
}
