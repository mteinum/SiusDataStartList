using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiusDataStartList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private readonly string SiusDataFolder = ConfigurationManager.AppSettings["SiusDataFolder"];

        private void Form1_Load(object sender, EventArgs e)
        {
            var directory = new DirectoryInfo(SiusDataFolder);

            foreach (var file in directory.GetFiles("*_stl.csv"))
            {
                this.listBoxFiles.Items.Add(file);
            }
        }

        private string[] GetFileContent(FileInfo fi)
        {
            return File.ReadAllLines(fi.FullName, System.Text.Encoding.GetEncoding(1252));
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            var x = (FileInfo)listBoxFiles.SelectedItem;

            var toWrite = new List<string>();

            foreach (var line in GetFileContent(x))
            {
                var parts = line.Split(';');

                var name = parts[2];
                var firstName = parts[3];

                if (name.StartsWith(firstName))
                {
                    parts[2] = name.Substring(firstName.Length + 1);

                    var newLine = string.Join(";", parts);

                    toWrite.Add(newLine);
                }
                else
                {
                    toWrite.Add(line);
                }
            }

            File.WriteAllLines(x.FullName, toWrite, System.Text.Encoding.GetEncoding(1252));

            MessageBox.Show("Done!");
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var x = (FileInfo)listBoxFiles.SelectedItem;

            var sb = new StringBuilder();
            sb.AppendLine("Start Number;Name;FirstName;DisplayName;Nation;Team;TargetNumber;Relay");

            foreach (var line in GetFileContent(x))
            {
                var parts = line.Split(';');
                var startNumber = parts[1];
                var name = parts[2];
                var FirstName = parts[3];
                var DisplayName = parts[4];
                var Nation = parts[5];
                var Team = parts[8];
                var TargetNumber = parts[10];
                var Relay = parts[11];

                var shooter =
                    $"{startNumber};{name};{FirstName};{DisplayName};{Nation};{Team};{TargetNumber};{Relay}";

                sb.AppendLine(shooter);
            }

            var content = sb.ToString();
            Clipboard.SetText(content, TextDataFormat.UnicodeText);
        }
    }
}
