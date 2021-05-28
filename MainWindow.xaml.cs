using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
		private string sundayRivalsLeaguesPath;

		private int selectedLeagueSlot = 0;
		private DirectoryInfo selectedLeagueDirectory;


		public MainWindow()
		{
			InitializeComponent();

			sundayRivalsLeaguesPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\LocalLow\\26k\\Sunday Rivals\\data\\leagues";
			DirectoryInfo leaguesDirectory = new DirectoryInfo(sundayRivalsLeaguesPath);
			StatusText.Text = "Hello! Waiting to execute...";

			if (leaguesDirectory.Exists == false)
			{
				StatusText.Text = "Couldn't find Sunday Rivals data directory! Please install Sunday Rivals.";
			}

			SelectedLeagueFolderText.Text = "League folder not selected...";
			CheckToEnableExecuteButton();
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

		private void Select_League_Folder_Button_Click(object sender, RoutedEventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					selectedLeagueDirectory = new DirectoryInfo(dialog.SelectedPath);
				}
			}

			SelectedLeagueFolderText.Text = selectedLeagueDirectory.FullName;

			CheckToEnableExecuteButton();
		}

		private void Select_League_Slot_1_Button_Click(object sender, RoutedEventArgs e)
		{
			SelectLeagueSlot(1);

			CheckToEnableExecuteButton();
		}

		private void Select_League_Slot_2_Button_Click(object sender, RoutedEventArgs e)
		{
			SelectLeagueSlot(2);

			CheckToEnableExecuteButton();
		}

		private void Select_League_Slot_3_Button_Click(object sender, RoutedEventArgs e)
		{
			SelectLeagueSlot(3);

			CheckToEnableExecuteButton();
		}

		private void Select_League_Slot_4_Button_Click(object sender, RoutedEventArgs e)
		{
			SelectLeagueSlot(4);

			CheckToEnableExecuteButton();
		}

		private void Execute_Button_Click(object sender, RoutedEventArgs e)
		{
			StatusText.Text = "Executing...";

			// Delete league folder in selected slot.
			DirectoryInfo leagueDirectory = new DirectoryInfo(GetSelectedLeagueSlotFolderPath());
			if (leagueDirectory.Exists)
				leagueDirectory.Delete(true);

			// Copy selected league folder into selected league slot.
			CopyFilesRecursively(selectedLeagueDirectory.FullName, GetSelectedLeagueSlotFolderPath());

			StatusText.Text = $"Finished! League was loaded into league slot {selectedLeagueSlot} in Sunday Rivals.";
		}

		private void SelectLeagueSlot(int slot)
		{
			selectedLeagueSlot = slot;
			SelectedLeagueSlotText.Text = $"Selected League Slot {slot}";
		}

		private void CheckToEnableExecuteButton()
		{
			ExecuteButton.IsEnabled = selectedLeagueSlot != 0 && selectedLeagueDirectory != null;
		}

		private string GetSelectedLeagueSlotFolderPath()
		{
			return $"{sundayRivalsLeaguesPath}\\league_{selectedLeagueSlot}";
		}

		private void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			//Now Create all of the directories
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
			}

			//Copy all the files & Replaces any files with the same name
			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
			}
		}
	}
}
