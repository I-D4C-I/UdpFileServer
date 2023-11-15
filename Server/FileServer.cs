using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using UdpFileServer.Utilities;

namespace UdpFileServer.Server
{

    internal class FileServer
    {
        private const int REMOTE_PORT = 5002;
        private const int MAX_FILE_SIZE = 8192;

        private static FileDetails _file = new FileDetails();
        private static IPAddress _remoteIPAddress;
        private static UdpClient _sender = new UdpClient();
        private static IPEndPoint _endPoint;

        private static FileStream _fileStream;

        public static void SetRemoteIP(IPAddress RemoteIPAddress)
        {
            _remoteIPAddress = RemoteIPAddress;
            _endPoint = new IPEndPoint(_remoteIPAddress, REMOTE_PORT);
        }

        public static Result CreateFileStream(string FilePath)
        {
            if (!new FileInfo(FilePath).Exists)
                return Result.Fail($"Файл не найден: {FilePath}");

            _fileStream = new(FilePath, FileMode.Open, FileAccess.Read);

            if(_fileStream.Length > MAX_FILE_SIZE)
                return Result.Fail("Файл должен весить меньше 8кБ");
            

            return Result.Success("Файл найден");
        }

        public static Result SendFileInfo()
        {
            _file.FileName = _fileStream.Name;
            var dotIndex = _fileStream.Name.IndexOf('.');
            _file.FileType = _fileStream.Name[dotIndex..];
            _file.FileSize = _fileStream.Length;
            try
            {
                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                using MemoryStream stream = new MemoryStream();

                fileSerializer.Serialize(stream, _file);

                stream.Position = 0;
                byte[] bytes = stream.ToArray();

                stream.Read(bytes, 0, bytes.Length);

                _sender.Send(bytes, bytes.Length, _endPoint);                
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Success("Отправка деталей файла");
        }

        public static Result SendFile() 
        {
            byte[] bytes = new byte[_fileStream.Length];
            try
            {
                _fileStream.Read(bytes, 0, bytes.Length);
                _sender.Send(bytes, bytes.Length, _endPoint);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
            finally
            {
                _fileStream.Close();
                _sender.Close();
            }
            return Result.Success("Файл успешно отправлен");
        }

        public static string GetStreamLenght() => _fileStream.Length.ToString();
        
    }
}
