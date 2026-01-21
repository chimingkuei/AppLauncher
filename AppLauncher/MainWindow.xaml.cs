using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static AppLauncher.BaseLogRecord;

namespace AppLauncher
{
    #region Config Class
    public class SerialNumber
    {
        [JsonProperty("Window_X_val")]
        public string Window_X_val { get; set; }
        [JsonProperty("Window_Y_val")]
        public string Window_Y_val { get; set; }
        [JsonProperty("Window_Width_val")]
        public string Window_Width_val { get; set; }
        [JsonProperty("Window_Height_val")]
        public string Window_Height_val { get; set; }
        [JsonProperty("Gap_Width_val")]
        public string Gap_Width_val { get; set; }
        [JsonProperty("Gap_Height_val")]
        public string Gap_Height_val { get; set; }
        [JsonProperty("RTSP_Path_val")]
        public string RTSP_Path_val { get; set; }
        [JsonProperty("VLCPath_val")]
        public string VLCPath_val { get; set; }
    }

    public class Model
    {
        [JsonProperty("SerialNumbers")]
        public SerialNumber SerialNumbers { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("Models")]
        public List<Model> Models { get; set; }
    }
    #endregion

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Function
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("請問是否要關閉？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        #region Config
        private SerialNumber SerialNumberClass()
        {
            SerialNumber serialnumber_ = new SerialNumber
            {
                Window_X_val = Window_X.Text,
                Window_Y_val = Window_Y.Text,
                Window_Width_val = Window_Width.Text,
                Window_Height_val = Window_Height.Text,
                Gap_Width_val = Gap_Width.Text,
                Gap_Height_val = Gap_Height.Text,
                RTSP_Path_val = RTSP_Path.Text,
                VLCPath_val = VLCPath.Text,
        };
            return serialnumber_;
        }

        private void LoadConfig(int model, int serialnumber, bool encryption = false)
        {
            List<RootObject> Parameter_info = config.Load(encryption);
            if (Parameter_info != null)
            {
                Window_X.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Window_X_val;
                Window_Y.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Window_Y_val;
                Window_Width.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Window_Width_val;
                Window_Height.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Window_Height_val;
                Gap_Width.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Gap_Width_val;
                Gap_Height.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.Gap_Height_val;
                RTSP_Path.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.RTSP_Path_val;
                VLCPath.Text = Parameter_info[model].Models[serialnumber].SerialNumbers.VLCPath_val;
            }
            else
            {
                // 結構:2個Models、Models下在各2個SerialNumbers
                SerialNumber serialnumber_ = SerialNumberClass();
                List<Model> models = new List<Model>
                {
                    new Model { SerialNumbers = serialnumber_ },
                    new Model { SerialNumbers = serialnumber_ }
                };
                List<RootObject> rootObjects = new List<RootObject>
                {
                    new RootObject { Models = models },
                    new RootObject { Models = models }
                };
                config.SaveInit(rootObjects, encryption);
            }
        }

        private void SaveConfig(int model, int serialnumber, bool encryption = false)
            => config.Save(model, serialnumber, SerialNumberClass(), encryption);
        #endregion

        #region Dispatcher Invoke 
        public string DispatcherGetValue(System.Windows.Controls.TextBox control)
        {
            string content = "";
            this.Dispatcher.Invoke(() =>
            {
                content = control.Text;
            });
            return content;
        }

        public void DispatcherSetValue(string content, System.Windows.Controls.TextBox control)
        {
            this.Dispatcher.Invoke(() =>
            {
                control.Text = content;
            });
        }
        #endregion

        private void WriteVersionToXml()
        {
            // 取得程式名稱（不含副檔名）
            string appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "UnknownApp";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;  // 執行檔目錄
            string assemblyInfoPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, @"..\..\..\Properties\AssemblyInfo.cs"));
            if (File.Exists(assemblyInfoPath))
            {
                // 讀取 AssemblyInfo.cs
                string content = File.ReadAllText(assemblyInfoPath);
                // 使用正則抓取 AssemblyFileVersion
                Regex regex = new Regex(@"\[assembly:\s*AssemblyFileVersion\s*\(\s*""(?<version>[\d\.]+)""\s*\)\s*\]");
                Match match = regex.Match(content);
                if (match.Success)
                {
                    string versionStr = match.Groups["version"].Value; // 例如 "1.2.3.45"
                    // 分割版本號
                    string[] parts = versionStr.Split('.');
                    string major = parts.Length > 0 ? parts[0] : "0";
                    string minor = parts.Length > 1 ? parts[1] : "0";
                    string patch = parts.Length > 2 ? parts[2] : "0";
                    string build = parts.Length > 3 ? parts[3] : "0";
                    // 建立 XML
                    XDocument doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", null),
                        new XElement("VersionInfo",
                            new XElement("Application",
                                new XAttribute("name", appName),
                                new XElement("Version",
                                    new XAttribute("major", major),
                                    new XAttribute("minor", minor),
                                    new XAttribute("patch", patch),
                                    new XAttribute("build", build)
                                )
                            )
                        )
                    );
                    // 寫入 XML 檔案
                    string outputPath = "AssemblyVersion.xml";
                    doc.Save(outputPath);
                }
            }
        }
        #endregion

        #region Parameter and Init
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WriteVersionToXml();
            LoadConfig(0, 0);
        }
        BaseConfig<RootObject> config = new BaseConfig<RootObject>();
        BaseLogRecord logger = new BaseLogRecord();
        AppLaunchHandler alh = new AppLaunchHandler();
        #endregion

        #region Main Screen
        private void Main_Btn_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.Button).Name)
            {
                case nameof(Open_RTSP):
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "CSV files|*.csv";
                        if (openFileDialog.ShowDialog() == true)
                        {
                            RTSP_Path.Text = openFileDialog.FileName;
                        }
                        break;
                    }
                case nameof(Open_VLCPath):
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "VLC files|*.exe";
                        if (openFileDialog.ShowDialog() == true)
                        {
                            VLCPath.Text = openFileDialog.FileName;
                        }
                        break;
                    }
                case nameof(VLC):
                    {
                        string vlcPath = VLCPath.Text;
                        if (string.IsNullOrWhiteSpace(vlcPath))
                        {
                            MessageBox.Show("請輸入vlc.exe路徑!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        string csvPath = RTSP_Path.Text;
                        if (string.IsNullOrWhiteSpace(csvPath))
                        {
                            MessageBox.Show("請輸入RTSP URL CSV路徑!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        // 一行一個 URL，排除空行
                        string[] rtspUrls = File.ReadAllLines(csvPath)
                                                .Select(line => line.Trim())
                                                .Where(line => !string.IsNullOrWhiteSpace(line))
                                                .ToArray();
                        if (rtspUrls.Length == 0)
                        {
                            Console.WriteLine("CSV 檔案中沒有有效的 RTSP URL。");
                            return;
                        }
                        // UI 設定（你原本的輸入框）
                        int startX = Convert.ToInt32(Window_X.Text);
                        int startY = Convert.ToInt32(Window_Y.Text);
                        int width = Convert.ToInt32(Window_Width.Text);
                        int height = Convert.ToInt32(Window_Height.Text);
                        int gapX = Convert.ToInt32(Gap_Width.Text);
                        int gapY = Convert.ToInt32(Gap_Height.Text);
                        int screenW = 1920;
                        int screenH = 1080;
                        int curX = startX;
                        int curY = startY;
                        foreach (string rtspUrl in rtspUrls)
                        {
                            // 啟動 VLC
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = vlcPath,
                                Arguments = $"\"{rtspUrl}\"",
                                UseShellExecute = false
                            };
                            Process.Start(startInfo);
                            // 等待 VLC 開啟
                            Thread.Sleep(1000);
                            // VLC 視窗標題（去掉帳密）
                            string windowTitle = $"{rtspUrl.Replace("admin0930:Asher19910930@", "")} - VLC 媒體播放器";
                            // 聚焦與移動
                            alh.ActivateWindow(windowTitle);
                            Thread.Sleep(300);
                            System.Drawing.Rectangle position = new System.Drawing.Rectangle(curX, curY, width, height);
                            alh.SetWindowsPosWrapper(windowTitle, position);
                            // 下一個位置
                            curX += width + gapX;
                            if (curX + width > screenW)
                            {
                                curX = startX;
                                curY += height + gapY;
                                if (curY + height > screenH)
                                    break;
                            }
                        }
                        logger.WriteLog("VLC 視窗排列完成！", LogLevel.General, richTextBoxGeneral);
                        break;
                    }
                case nameof(Save_Config):
                    {
                        SaveConfig(0, 0);
                        logger.WriteLog("儲存參數！", LogLevel.General, richTextBoxGeneral);
                        break;
                    }
            }
        }

        private void About_Click(object sender, MouseButtonEventArgs e)
        {
            string filePath = "AssemblyVersion.xml";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("未找到版本號 XML!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                XDocument doc = XDocument.Load(filePath);
                XElement versionElement = doc.Root?.Element("Application")?.Element("Version");
                if (versionElement != null)
                {
                    string major = versionElement.Attribute("major")?.Value ?? "0";
                    string minor = versionElement.Attribute("minor")?.Value ?? "0";
                    string patch = versionElement.Attribute("patch")?.Value ?? "0";
                    string build = versionElement.Attribute("build")?.Value ?? "0";
                    string version = $"{major}.{minor}.{patch}.{build}";
                    MessageBox.Show($"版本號︰{version}", "版本", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("XML 中未找到版本號!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"讀取版本號失敗: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            e.Handled = true; // 阻止切換到這個 Tab 的內容
        }
        #endregion


    }
}
