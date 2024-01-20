// RegistrationView.xaml.cs
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    public partial class RegistrationView : UserControl
    {

        // Konstruktor dla RegistrationView
        public RegistrationView()
        {
            InitializeComponent();
        }

        // Właściwość do śledzenia aktualnego motywu
        public bool IsDarkTheme { get; set; }
        public Func<SqlConnection> sqlConnectionFactory { get; set; }
        public Func<PaletteHelper> paletteHelperFactory { get; set; }

        public readonly PaletteHelper paletteHelper = new PaletteHelper();

        // Obsługa zdarzenia dla przełączania między jasnym a ciemnym motywem
        public void toggleTheme(object sender, RoutedEventArgs e)
        {
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
        }

        // Obsługa zdarzenia dla wyjścia z aplikacji
        public void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Obsługa zdarzenia dla przełączania się do widoku logowania
        public void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainContent.Content = new LoginView();
        }

        // Metoda pomocnicza do sprawdzania poprawności hasła
        public bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool UpperCase = false;
            bool LowerCase = false;
            bool Digit = false;
            bool SpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    UpperCase = true;
                }
                else if (char.IsLower(c))
                {
                    LowerCase = true;
                }
                else if (char.IsDigit(c))
                {
                    Digit = true;
                }
                else if (char.IsPunctuation(c) || char.IsSymbol(c))
                {
                    SpecialChar = true;
                }
            }

            return UpperCase && LowerCase && Digit && SpecialChar;
        }

        public async void createBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userNameBoxC.Text.Length == 0)
            {
                warningLabel.Text = "Proszę wprowadź nazwę użytkownika.";
            }
            else if (passwordBoxC.Password.Length == 0)
            {
                warningLabel.Text = "Proszę wprowadź hasło.";
            }
            else if (userNameBoxC.Text.Length < 3)
            {
                warningLabel.Text = "Nazwa użytkownika musi zawierać co najmniej 3 znaki";
            }
            else if (passwordBoxC.Password.Length < 8)
            {
                warningLabel.Text = "Hasło musi zawierać co najmniej 8 znaków";
            }
            else if (passwordBoxC.Password != verifyBox.Password)
            {
                warningLabel.Text = "Hasła nie są takie same.";
            }
            else if (!IsPasswordValid(passwordBoxC.Password))
            {
                warningLabel.Text = "Hasło musi zawierać wielkie i małe litery, cyfry oraz znaki specjalne.";
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-RV3JHLM;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");

                try
                {
                    sqlConnection.Open();
                    // Sprawdzenie, czy nazwa użytkownika już istnieje
                    String usernameCheckQuery = "SELECT 1 FROM dbo.users WITH(NOLOCK) WHERE username = @username";
                    SqlCommand sqlC = new SqlCommand(usernameCheckQuery, sqlConnection);
                    sqlC.CommandType = System.Data.CommandType.Text;
                    sqlC.Parameters.AddWithValue("@username", userNameBoxC.Text);
                    int exists = Convert.ToInt32(sqlC.ExecuteScalar());
                    if (exists > 0)
                    {
                        warningLabel.Text = "Ten użytkownik istnieje.";
                        return;
                    }

                    // Wstawienie nowego użytkownika do bazy danych
                    String query2 = "INSERT INTO dbo.users (username, password) VALUES (@username, @password);";
                    using (SqlCommand sqlCom2 = new SqlCommand(query2, sqlConnection))
                    {
                        sqlCom2.CommandType = System.Data.CommandType.Text;
                        sqlCom2.Parameters.AddWithValue("@username", userNameBoxC.Text);
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordBoxC.Password);
                        sqlCom2.Parameters.AddWithValue("@password", hashedPassword);
                        await sqlCom2.ExecuteNonQueryAsync();
                    }

                    // Wyświetlenie komunikatu o sukcesie
                    warningLabel.Foreground = Brushes.Lime;
                    warningLabel.Text = "Konto zostało stworzone!";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
