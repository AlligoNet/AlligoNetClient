﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace AlligoClient
{
    struct Server
    {
        public string host;
        public string port;
        public string name;

        public Server(string host, string port, string name)
        {
            this.host = host;
            this.port = port;
            this.name = name;
        }
    };

    public partial class Form1 : Form
    {
        [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int AddFontResource(string lpszFilename);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int CreateScalableFontResource(uint fdwHidden, string lpszFontRes, string lpszFontFile, string lpszCurrentPath);

        /* Fonts */
        string[] fonts = { "Roboto-Black.ttf", "Roboto-BlackItalic.ttf", "Roboto-Bold.ttf", "Roboto-BoldItalic.ttf", "Roboto-Italic.ttf", "Roboto-Light.ttf", "Roboto-LightItalic.ttf", "Roboto-Medium.ttf", "Roboto-MediumItalic.ttf", "Roboto-Regular.ttf", "Roboto-Thin.ttf", "Roboto-ThinItalic.ttf",  };

        Server[] servers = new Server[]
        {
            new Server("quentin.alligo.co", "443", "QuentinEU"),
            new Server("sinon.alligo.co", "443", "SinonUSWest"),
            new Server("eugeo.alligo.co", "443", "EugeoUSEast"),
            new Server("insignia.alligo.co", "443", "InsigniaOC")
        };

        private string SoftEtherPath = "C:\\Program Files\\SoftEther VPN Client\\vpncmd.exe";

        //User Creds
        private string userName, userPass;

        static readonly string PassHash = "REDACTED";
        static readonly string saltKey = "REDACTED";
        static readonly string viKey = "REDACTED";

        private string versionCheckURL = "https://updates.alligo.co/VERSION";

        private int selectedServer;

        public Form1()
        {
            InitializeComponent();

            InitCustomFonts();

            FormClosing += Client_Closing;

            if (IsSoftEtherInstalled() == false)
            {
                /* Extract and Run Updater with -install args */
                Stream resource = GetType().Assembly.GetManifestResourceStream("AlligoClient.AlligoUpdater.exe");
                byte[] bytes = new byte[(int)resource.Length];
                resource.Read(bytes, 0, bytes.Length);
                File.WriteAllBytes("AlligoUpdater.exe", bytes);

                Process.Start("AlligoUpdater.exe", "install");

                Environment.Exit(0);
            }
            else
            {
                CheckForUpdates();
            }
        }

        private void InitCustomFonts()
        {            
            /* Extract Fonts */
            for(int i = 0; i < fonts.Length; i++)
            {
                string fontPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), fonts[i]);
                if(!File.Exists(fontPath))
                {
                    /* Create Temp Folder Font */
                    if(!Directory.Exists("\\temp"))
                    {
                        Directory.CreateDirectory("temp");
                    }

                    /* Extract the font */
                    Stream resource = GetType().Assembly.GetManifestResourceStream(string.Format("AlligoClient.{0}", fonts[i]));
                    byte[] bytes = new byte[(int)resource.Length];
                    resource.Read(bytes, 0, bytes.Length);
                    File.WriteAllBytes(string.Format("temp\\{0}", fonts[i]), bytes);

                    /* Copy to Destination */
                    File.Copy(string.Format("temp\\{0}", fonts[i]), fontPath);

                    PrivateFontCollection fontCol = new PrivateFontCollection();
                    fontCol.AddFontFile(fontPath);
                    var actualFontName = fontCol.Families[0].Name;

                    //Add font
                    AddFontResource(fontPath);
                    //Add registry entry  
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts",
                    actualFontName, fonts[i], RegistryValueKind.String);
                }
            }

            if (Directory.Exists("temp"))
            {
                DirectoryInfo directory = new DirectoryInfo("temp");
                Empty(directory);
                Directory.Delete("temp");
            }
        }

        public static void Empty(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        private void CheckForUpdates()
        {
            string clientVersion = AssemblyName.GetAssemblyName("AlligoClient.exe").Version.ToString();
            string newVersion = "0.0.0.0";

            /* Delete Updater if it exists */
            if(File.Exists("AlligoUpdater.exe"))
            {
                File.Delete("AlligoUpdater.exe");
            }

            /* Delete Installer if it exists */
            if (File.Exists("SoftEtherInstall.exe"))
            {
                File.Delete("SoftEtherInstall.exe");
            }

            using (WebClient vs = new WebClient())
            {
                try
                {
                    Stream file = vs.OpenRead(versionCheckURL);
                    StreamReader parse = new StreamReader(file);
                    newVersion = parse.ReadToEnd();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERROR: Failed to connect to update server.");
                    Environment.Exit(0);
                }
            }

            newVersion = newVersion.Trim();

            if (clientVersion.Equals(newVersion))
            {
                //Up to date
                updateLbl.Text = "";
                updateLbl.ForeColor = Color.WhiteSmoke;
                currVersionLbl.ForeColor = Color.WhiteSmoke;

                //Hide Button since not required.
                updateBtn.Enabled = false;
                updateBtn.Hide();

                currVersionLbl.Text = string.Format("v{0}", clientVersion);

                //Init Alligo
                var thread = new Thread(() =>
                    {
                        Init();
                    }

                );
                thread.Start();
            }
            else
            {
                //Not up to date
                updateLbl.Text = "Update Available!";
                currVersionLbl.Hide();


                //Show Update Button
                updateBtn.Show();
                updateBtn.Enabled = true;
                btnConnect.Hide();
                btnConnect.Enabled = false;
                btnDisconnect.Hide();
                btnDisconnect.Enabled = false;
            }
        }

        /* Extracts and Runs Updater */
        private void StartUpdate()
        {
            /* Extract */
            Stream resource = GetType().Assembly.GetManifestResourceStream("AlligoClient.AlligoUpdater.exe");
            byte[] bytes = new byte[(int)resource.Length];
            resource.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes("AlligoUpdater.exe", bytes);

            Process.Start("AlligoUpdater.exe");

            Application.Exit();
        }

        /* Initialise AlligoClient */
        private void Init()
        {
            // Hide Currently unneeded button
            btnDisconnect.Enabled = false;
            btnDisconnect.Hide();

            connectedLbl.Hide();

            //Create Alligo Network Adapter if needed
            RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD NicCreate {0}", "Alligo"));

            // Create Servers from Defaults
            for (int i = 0; i < servers.Length; i++)
            {
                //Create Servers
                RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountCreate {0} /SERVER:{1}:{2} /HUB:HALO /USERNAME:halo /NICNAME:Alligo", servers[i].name, servers[i].host, servers[i].port));
                //Change Password Type
                RunCMD(string.Format("/CLIENT localhost /CMD AccountPasswordSet {0} /PASSWORD:halo /TYPE:standard", servers[i].name)); //DO NOT INCLUDE LOG - SECURITY
            }

            /* Disconnect all first just to be safe */
            for (int i = 0; i < servers.Length; i++)
            {
                RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountDisconnect {0}", servers[i].name));
            }

            LoadCredentials();
        }

        private string GetAlligoIP()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    if (nic.Name.Contains("Alligo"))
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();                     
                        }
                    }
                }
            }
            return "";
        }

        /* Connect to VPN via SoftEther */
        private void Connect(int serverIndex)
        {
            if(rememberCheckBox.Checked)
            {
                SaveCredentials();
            }
            else
            {
                if(File.Exists("credentials.txt"))
                {
                    File.Delete("credentials.txt");
                }
            }
            userName = userName_txt.Text;
            userPass = password_txt.Text;

            // Update Login Info
            RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountUsernameSet {0} /USERNAME:{1}", servers[selectedServer].name, userName));
            RunCMD(string.Format("/CLIENT localhost /CMD AccountPasswordSet {0} /PASSWORD:{1} /TYPE:standard", servers[selectedServer].name, userPass)); //DO NOT INCLUDE LOG - SECURITY
        
            //Connect
            bool success = RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountConnect {0}", servers[selectedServer].name));
            if (success)
            {
                //Show Disconnect Button
                btnDisconnect.Show();
                btnDisconnect.Enabled = true;

                //Hide Connect Button
                btnConnect.Enabled = false;
                btnConnect.Hide();

                System.Threading.Thread.Sleep(4000); //Delay a little just to be sure
                currVersionLbl.Hide();
                connectedLbl.Show();

                connectedLbl.Text = string.Format("Connected: {0}", GetAlligoIP());
            }
        }

        /* Disconnect */
        private void Disconnect(int serverIndex)
        {
            bool success = RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountDisconnect {0}", servers[selectedServer].name));

            // Show Connect Button Again
            btnConnect.Show();
            btnConnect.Enabled = true;

             // Hide Disconnect Button
            btnDisconnect.Enabled = false;
            btnDisconnect.Hide();
            currVersionLbl.Show();
            connectedLbl.Hide();

            MessageBox.Show("Disconnected");
        }


        private void SaveCredentials()
        {
            string[] credentials = { "true", userName_txt.Text, Encrypt(password_txt.Text), serverPickerCB.SelectedIndex.ToString() };

            //Write To file
            System.IO.File.WriteAllLines("credentials.txt", credentials);
        }

        private void LoadCredentials()
        {
            bool saved = false;

            if (File.Exists("credentials.txt"))
            {
                string[] credentials = System.IO.File.ReadAllLines("credentials.txt");

                if (Boolean.TryParse(credentials[0], out saved))
                {
                    rememberCheckBox.Checked = saved;
                }

                userName_txt.Text = credentials[1];
                password_txt.Text = Decrypt(credentials[2]);

                int result;
                if (int.TryParse(credentials[3], out result))
                {
                    serverPickerCB.SelectedIndex = result;
                }
            }
            else
            {
                serverPickerCB.SelectedIndex = 0;
            }
        }

        /* Simply checks if folder exists, crude, needs improving */
        private bool IsSoftEtherInstalled()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartMenu);

            string[] entries = Directory.GetFiles(path);

            foreach(string filename in entries)
            {
                if(filename.Contains("SoftEther"))
                {
                    return true;
                }
            }

            return false;
        }

        /* Executes commands sent to the vpncmd */
        private bool RunCMD(string command)
        {
            string success = "The command completed successfully";
            string output;
            Process cmdProc = new Process();
            cmdProc.StartInfo.FileName = SoftEtherPath;
            cmdProc.StartInfo.Arguments = command;
            cmdProc.StartInfo.UseShellExecute = false;
            cmdProc.StartInfo.RedirectStandardOutput = true;
            cmdProc.StartInfo.CreateNoWindow = true;
            cmdProc.Start();
            output = cmdProc.StandardOutput.ReadToEnd();
            cmdProc.WaitForExit();

            if (output.Contains(success))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Encrypt String */
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PassHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(viKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PassHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(viKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        /* GENERATED BY VS */

        void Client_Closing(object sender, FormClosingEventArgs e)
        {
            RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountDisconnect {0}", servers[selectedServer].name));
            System.Threading.Thread.Sleep(500); //Delay a little just to be sure
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            selectedServer = serverPickerCB.SelectedIndex;

            if( selectedServer < 0 )
            {
                selectedServer = 0;
            }

            Connect(selectedServer);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            StartUpdate();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ServerSelection_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            RunCMD(string.Format("/CLIENT localhost /OUT:AlligoLog.txt /CMD AccountDisconnect {0}", servers[selectedServer].name));
            System.Threading.Thread.Sleep(100); //Delay a little just to be sure
            Application.Exit();
        }

        private void btnDisconnect_Click_1(object sender, EventArgs e)
        {
            Disconnect(selectedServer);
        }

        private void btnMinimise_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
