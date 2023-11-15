using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using UdpFileServer.Utilities;

namespace UdpFileServer.Client
{
    internal class FileClient
    {
        private static FileDetails _file;
        private static int localPort = 5002;
        private static UdpClient _client = new UdpClient(localPort);
        private static IPEndPoint _clientEndPoint;

        private static FileStream _fileStream;
        private static byte[] _fileBytes = new byte[0];


        private static Result GetFileDetails()
        {
            try
            {
                _fileBytes = _client.Receive(ref _clientEndPoint);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileDetails));
                using MemoryStream stream = new MemoryStream();

                stream.Write(_fileBytes, 0, _fileBytes.Length);
                stream.Position = 0;

                _file = (FileDetails)xmlSerializer.Deserialize(stream);
                return Result.Success($"Получен файл {_file.FileName}, имеющий размер {_file.FileSize} байт");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public static Result ReceiveFile()
        {
            try
            {
                _fileBytes = _client.Receive(ref _clientEndPoint);

                _fileStream = new FileStream("temp." + _file.FileType, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                _fileStream.Write(_fileBytes, 0, _fileBytes.Length);
                return Result.Success("Файл сохранен");
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
