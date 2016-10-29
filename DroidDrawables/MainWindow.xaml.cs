using System;
using System.IO;
using System.Threading;
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
using DroidDrawables.Models;
using DroidDrawables.Preferences;

namespace DroidDrawables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Session m_session;

        public MainWindow()
        {
            InitializeComponent();
            this.txtInputFolder.Text = AppSettings.ResourceFolder;
        }

        private void btnBrowseInput_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = this.txtInputFolder.Text;
                dlg.ShowDialog();
                this.txtInputFolder.Text = dlg.SelectedPath;
                this.txtOutputFolder.Text = dlg.SelectedPath;
            }
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowDialog();
                this.txtOutputFolder.Text = dlg.SelectedPath;
            }
        }

        public void AddLogMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);

            if (Thread.CurrentThread.IsBackground)
            {
                lstLog.Dispatcher.Invoke(() => lstLog.Items.Add(message));
            }
            else
            {
                lstLog.Items.Add(message);
            }
        }

        public void UpdateUIOnCompletion()
        {
            if (Thread.CurrentThread.IsBackground)
            {
                btnGenerate.Dispatcher.Invoke( () => btnGenerate.IsEnabled = true);
            }
            else
            {
                btnGenerate.IsEnabled = true;
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(txtInputFolder.Text))
            {
                System.Windows.MessageBox.Show(Properties.Resources.MSG_INVALID_INPUT_DIR);
                return;
            }

            if (!Directory.Exists(txtOutputFolder.Text))
            {
                //System.Windows.MessageBox.Show(Properties.Resources.MSG_INPUT_AS_OUTPUT);
                txtOutputFolder.Text = txtInputFolder.Text;
            }
            btnGenerate.IsEnabled = false;
            lstLog.Items.Clear();
            m_session = new Session(txtInputFolder.Text, txtOutputFolder.Text);
            DrawableGenerator drawableGenerator = new DrawableGenerator(m_session, this);
            drawableGenerator.GenerateDrawables();
            //Preserve the last used path
            AppSettings.ResourceFolder = txtInputFolder.Text;
        }
    }
}
