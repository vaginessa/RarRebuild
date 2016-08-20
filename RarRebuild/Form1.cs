using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RarRebuild
{
    public partial class Form1 : Form
    {
        string rarExe = Path.GetTempPath() + "Rar.exe";
        string workDir = Path.GetTempPath() + "workDir";
        FileInfo file;

        // WinRAR 註冊資訊
        byte[] key =
        {
            0x52, 0x41, 0x52, 0x20, 0x72, 0x65, 0x67, 0x69, 0x73, 0x74, 0x72, 0x61, 0x74, 0x69, 0x6F, 0x6E,
			0x20, 0x64, 0x61, 0x74, 0x61, 0x0D, 0x0A, 0x52, 0x61, 0x72, 0x52, 0x65, 0x62, 0x75, 0x69, 0x6C,
			0x64, 0x0D, 0x0A, 0x4C, 0x6F, 0x63, 0x61, 0x6C, 0x20, 0x53, 0x69, 0x74, 0x65, 0x20, 0x4C, 0x69,
			0x63, 0x65, 0x6E, 0x73, 0x65, 0x0D, 0x0A, 0x55, 0x49, 0x44, 0x3D, 0x35, 0x32, 0x33, 0x33, 0x34,
			0x30, 0x31, 0x64, 0x36, 0x31, 0x39, 0x64, 0x66, 0x34, 0x32, 0x35, 0x63, 0x63, 0x62, 0x31, 0x0D,
			0x0A, 0x36, 0x34, 0x31, 0x32, 0x32, 0x31, 0x32, 0x32, 0x35, 0x30, 0x63, 0x63, 0x62, 0x31, 0x63,
			0x39, 0x61, 0x62, 0x35, 0x32, 0x32, 0x63, 0x39, 0x65, 0x64, 0x32, 0x64, 0x62, 0x36, 0x63, 0x32,
			0x65, 0x38, 0x32, 0x63, 0x38, 0x37, 0x32, 0x31, 0x34, 0x39, 0x32, 0x31, 0x39, 0x36, 0x34, 0x64,
			0x65, 0x64, 0x64, 0x65, 0x66, 0x34, 0x37, 0x0D, 0x0A, 0x32, 0x35, 0x32, 0x61, 0x64, 0x66, 0x36,
			0x64, 0x38, 0x62, 0x30, 0x36, 0x35, 0x61, 0x39, 0x32, 0x36, 0x35, 0x64, 0x35, 0x36, 0x30, 0x36,
			0x39, 0x34, 0x63, 0x38, 0x38, 0x66, 0x32, 0x30, 0x37, 0x33, 0x34, 0x66, 0x37, 0x66, 0x61, 0x36,
			0x61, 0x64, 0x39, 0x36, 0x38, 0x38, 0x36, 0x35, 0x36, 0x35, 0x34, 0x61, 0x35, 0x37, 0x62, 0x0D,
			0x0A, 0x30, 0x66, 0x30, 0x65, 0x64, 0x66, 0x32, 0x39, 0x35, 0x64, 0x36, 0x30, 0x37, 0x63, 0x65,
			0x36, 0x66, 0x61, 0x36, 0x64, 0x36, 0x62, 0x33, 0x36, 0x36, 0x37, 0x34, 0x30, 0x31, 0x62, 0x34,
			0x31, 0x33, 0x62, 0x64, 0x63, 0x62, 0x63, 0x63, 0x63, 0x61, 0x33, 0x31, 0x35, 0x32, 0x63, 0x38,
			0x61, 0x62, 0x36, 0x61, 0x62, 0x63, 0x35, 0x0D, 0x0A, 0x39, 0x33, 0x31, 0x38, 0x31, 0x38, 0x61,
			0x33, 0x35, 0x36, 0x37, 0x34, 0x61, 0x32, 0x36, 0x66, 0x30, 0x36, 0x35, 0x32, 0x36, 0x31, 0x66,
			0x38, 0x62, 0x63, 0x65, 0x33, 0x32, 0x64, 0x66, 0x62, 0x63, 0x65, 0x36, 0x30, 0x32, 0x30, 0x62,
			0x66, 0x32, 0x61, 0x35, 0x37, 0x36, 0x65, 0x34, 0x65, 0x65, 0x61, 0x38, 0x64, 0x32, 0x35, 0x0D,
			0x0A, 0x66, 0x39, 0x66, 0x31, 0x35, 0x65, 0x64, 0x39, 0x66, 0x30, 0x65, 0x34, 0x33, 0x61, 0x62,
			0x32, 0x36, 0x34, 0x39, 0x65, 0x65, 0x31, 0x30, 0x32, 0x38, 0x65, 0x34, 0x33, 0x39, 0x35, 0x61,
			0x64, 0x30, 0x32, 0x34, 0x64, 0x61, 0x66, 0x33, 0x31, 0x32, 0x63, 0x39, 0x38, 0x31, 0x33, 0x37,
			0x39, 0x32, 0x37, 0x65, 0x37, 0x32, 0x36, 0x0D, 0x0A, 0x39, 0x65, 0x36, 0x32, 0x65, 0x34, 0x30,
			0x61, 0x39, 0x31, 0x31, 0x62, 0x35, 0x36, 0x61, 0x32, 0x39, 0x35, 0x32, 0x62, 0x61, 0x35, 0x35,
			0x61, 0x62, 0x31, 0x35, 0x32, 0x62, 0x38, 0x62, 0x63, 0x33, 0x39, 0x34, 0x32, 0x36, 0x37, 0x61,
			0x31, 0x30, 0x39, 0x63, 0x35, 0x37, 0x39, 0x36, 0x63, 0x36, 0x30, 0x39, 0x36, 0x65, 0x35, 0x0D,
			0x0A, 0x31, 0x38, 0x31, 0x37, 0x30, 0x35, 0x33, 0x31, 0x39, 0x36, 0x61, 0x64, 0x38, 0x38, 0x39,
			0x31, 0x30, 0x63, 0x34, 0x61, 0x35, 0x38, 0x32, 0x35, 0x31, 0x35, 0x31, 0x63, 0x36, 0x37, 0x64,
			0x30, 0x31, 0x38, 0x35, 0x66, 0x39, 0x35, 0x36, 0x32, 0x31, 0x62, 0x30, 0x65, 0x31, 0x36, 0x31,
			0x30, 0x36, 0x34, 0x38, 0x30, 0x39, 0x34, 0x0D, 0x0A
        };
        byte[] rarBin = new byte[550288];
        string errorMessage;

        public Form1()
        {
            InitializeComponent();
            Assembly.GetExecutingAssembly().GetManifestResourceStream("RarRebuild.Resources.Rar.exe").Read(rarBin, 0, 550288);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == true)
                e.Effect = DragDropEffects.All;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            file = new FileInfo(files[0].ToString());
            textBox2.Text = file.FullName;
            Worker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (keyDialog.ShowDialog() == DialogResult.OK)
                radioButton2.Checked = true;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 匯出Rar.exe
            string rarExe = Path.GetTempPath() + "Rar.exe";
            File.WriteAllBytes(rarExe, rarBin);

            // 建立工作目錄
            string workDir = Path.GetTempPath() + "workDir";            
            if (Directory.Exists(workDir))
                Directory.Delete(workDir, true);
            Directory.CreateDirectory(workDir);

            //先判斷檔案是否含有WinRAR.exe
            if (checkFileHasWinRAR())
            {
                stripSFX();
                if (File.Exists(Path.GetTempPath()+"setup.sfx"))
                {
                    extractComment();
                    if (File.Exists(Path.GetTempPath() + "setup.txt") && File.ReadAllLines(Path.GetTempPath()+"setup.txt")[0].Contains("; WinRAR"))
                    {
                        extractAllFiles();
                        packWinRAR();
                    }
                    else
                    {
                        errorMessage = "註解檔案有誤";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    errorMessage = "無法抓取 SFX 檔案";
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                errorMessage = "非 WinRAR 安裝檔";
                e.Cancel = true;
                return;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
                MessageBox.Show(errorMessage);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.facebook.com/profile.php?id=100005653172695");
        }

        private bool checkFileHasWinRAR()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = rarExe,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = Path.GetTempPath(),
                Arguments = "vb -p- " + file.FullName
            };
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            if (output.Contains("WinRAR.exe"))
                return true;
            else
                return false;
        }

        private void stripSFX()
        {
            BinaryReader fs = new BinaryReader(file.OpenRead());
            long sfxSize = 0;
            while (fs.BaseStream.Position < fs.BaseStream.Length - 4)
            {
                if (fs.ReadUInt32() == 0x21726152)
                {
                    sfxSize = fs.BaseStream.Position - 4;
                    break;
                }
                fs.BaseStream.Position += 508;
            }
            if (sfxSize != 0)
            {
                byte[] sfxData = new byte[sfxSize];
                fs.BaseStream.Position = 0;
                sfxData = fs.ReadBytes((int)sfxSize);
                File.WriteAllBytes(Path.GetTempPath() + "setup.sfx", sfxData);
            }
            fs.Close();
        }

        private void extractComment()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = rarExe,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetTempPath(),
                Arguments = "cw -y -p- " + file.FullName + " setup.txt"
            };
            p.Start();
            p.WaitForExit();
        }

        private void extractAllFiles()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = rarExe,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = workDir,
                Arguments = "e -y -p- " + file.FullName
            };
            p.Start();
            p.WaitForExit();
        }

        private void packWinRAR()
        {
            File.AppendAllText(workDir + "\\Uninstall.lst", "rarreg.key", Encoding.ASCII);
            if (radioButton1.Checked)
                File.WriteAllBytes(workDir + "\\rarreg.key", key);
            else
                File.Copy(keyDialog.FileName, workDir + "\\rarreg.key");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = rarExe,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = Application.StartupPath,
                Arguments = "a -y -k -r -ep1 -cfg- -s -m5 -zsetup.txt -sfxsetup.sfx " + file.Name + " " + workDir + "\\*.*"
            };
            file.MoveTo(file.FullName + ".bak");
            p.Start();
            p.WaitForExit();
        }
    }
}
