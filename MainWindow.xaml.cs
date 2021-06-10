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

			// Check if the selected directory is a valid Sunday Rivals League directory.
			if (IsDirectoryValidLeague(selectedLeagueDirectory) == false)
			{
				SelectedLeagueFolderText.Text = "Selected directory is not a valid league folder...";
				selectedLeagueDirectory = null;
			}
			CheckToEnableExecuteButton();

			if (selectedLeagueDirectory == null)
				return;

			SelectedLeagueFolderText.Text = selectedLeagueDirectory.FullName;
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

			string leagueSlotPath = GetSelectedLeagueSlotFolderPath(selectedLeagueSlot);

			// Delete league folder in selected slot.
			DirectoryInfo leagueDirectory = new DirectoryInfo(leagueSlotPath);
			if (leagueDirectory.Exists)
				leagueDirectory.Delete(true);

			DirectoryInfo targetLeagueDirectory = new DirectoryInfo(GetSelectedLeagueSlotFolderPath(selectedLeagueSlot));

			// Copy selected league folder into selected league slot.
			CopyAll(selectedLeagueDirectory, targetLeagueDirectory);

			// Rename .lge file to the correct slot.
			FileInfo leagueFile = targetLeagueDirectory.GetFiles("*.lge")[0];
			leagueFile.MoveTo(GetInGameLeagueFileFullName(selectedLeagueSlot));

			StatusText.Text = $"Finished! League was copied into league slot {selectedLeagueSlot} in Sunday Rivals.";
		}

		/// <summary>
		/// Determines if provided directory is a valid Sunday Rivals League Folder.
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		private bool IsDirectoryValidLeague(DirectoryInfo directory)
		{
			if (directory.GetFiles("*.lge").Length == 0 && directory.GetFiles("*.ros").Length != 32)
				return false;

			return true;
		}

		private void SelectLeagueSlot(int slot)
		{
			selectedLeagueSlot = slot;
			SelectedLeagueSlotText.Text = $"Selected League Slot {slot}";
			StatusText.Text = "New league slot selected. Waiting to execute...";
		}

		private void CheckToEnableExecuteButton()
		{
			ExecuteButton.IsEnabled = selectedLeagueSlot != 0 && selectedLeagueDirectory != null;
		}

		private string GetSelectedLeagueSlotFolderPath(int leagueSlot)
		{
			return $"{sundayRivalsLeaguesPath}\\league_{leagueSlot}";
		}

		private string GetInGameLeagueFileFullName(int leagueSlot)
		{
			return $"{GetSelectedLeagueSlotFolderPath(leagueSlot)}\\league_{leagueSlot}.lge";
		}

		private void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			if (source.FullName.ToLower() == target.FullName.ToLower())
			{
				return;
			}

			// Check if the target directory exists, if not, create it.
			if (Directory.Exists(target.FullName) == false)
			{
				Directory.CreateDirectory(target.FullName);
			}

			// Copy each file into it's new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
				fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}
	}
}
