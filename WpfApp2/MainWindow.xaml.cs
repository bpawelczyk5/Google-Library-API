// MainWindow.xaml.cs
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        // Konstruktor dla MainWindow
        public MainWindow()
        {
            InitializeComponent();

            // Dodanie obsługi zdarzenia dla przesuwania okna poprzez kliknięcie i przeciąganie myszą
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

            // Ustawienie początkowej zawartości na widok logowania
            MainContent.Content = new LoginView();
        }

        // Obsługa zdarzenia przyciśnięcia lewego przycisku myszy
        public void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Sprawdzenie, czy przyciśnięty został lewy przycisk myszy
            if (e.ChangedButton == MouseButton.Left)
            {
                // Przesunięcie okna w przypadku przeciągnięcia myszą
                this.DragMove();
            }
        }
    }
}
