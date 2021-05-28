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

namespace SundayRivalsCustomLeagueHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItem_About_Click(object sender, RoutedEventArgs e)
		{
			AboutWindow window = new AboutWindow();
			window.Show();
		}

		private void MenuItem_Help_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow window = new HelpWindow();
			window.Show();
		}
	}
}
