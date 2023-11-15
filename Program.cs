using System.IO;
using System.Net;
using UdpFileServer.Server;

namespace UdpFileServer
{
    internal class Program
    {
                
        [STAThread]
        static void Main(string[] args)
        {            
            try
            {
                //Получаем удаленный IP-адрес и создаем endPoint
                Console.WriteLine("Введите удаленный IP-адрес:");
                var remoteIPAddress = IPAddress.Parse(Console.ReadLine()); //127.0.0.1
                FileServer.SetRemoteIP(remoteIPAddress);

                while (true)
                {
                    Console.WriteLine("Введите путь к файлу и его имя:");
                    var result = FileServer.CreateFileStream(Console.ReadLine());
                    if (!result.IsSuccessful)
                    {
                        Console.WriteLine(result);
                        continue;
                    }
                    result = FileServer.SendFileInfo();
                    if (!result.IsSuccessful)
                    {
                        Console.WriteLine(result);
                        continue;
                    }
                    Thread.Sleep(2000);
                    Console.WriteLine("Отправка файлов размером" + FileServer.GetStreamLenght() + " байт");
                    result = FileServer.SendFile();
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }
    }
}