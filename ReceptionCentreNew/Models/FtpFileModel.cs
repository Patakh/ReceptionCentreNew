using System.Net;
using FluentFTP;
using ReceptionCentreNew; 

public class FtpFileModel
{
    FtpClient client; 

    private void FtpConnetion(string ftpServer, string ftpLogin, string ftpPassword)
    { 
        client = new FtpClient();
        client.Host = ftpServer;
        client.Credentials = new NetworkCredential(ftpLogin, ftpPassword);
        client.Connect();
    }

    public bool FileSave(byte[] uploadImage, string ftpServer, string ftpLogin, string ftpPassword, string ftpFolder, string fileName, string subFolder)
    {
        try
        {
            FtpConnetion(ftpServer, ftpLogin, ftpPassword);
            var dirList = client.GetListing(ftpFolder);
            var folderNotExist = dirList.Count(d => d.Name == subFolder) == 0;
            //Загружаем файл на сервер
            if (folderNotExist)
                client.CreateDirectory(ftpFolder + "/" + subFolder);
            client.UploadBytes(uploadImage, ftpFolder + "/" + subFolder + "/" + fileName);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            //отключаемся от FTP сервера
            client.Disconnect();
        }
    }


    public byte[] OpenURI(string ftpServer, string ftpLogin, string ftpPassword, string ftpFolder, string fileName)
    {
        byte[] buffer = new byte[32768];
        try
        {
            FtpConnetion(ftpServer, ftpLogin, ftpPassword);
            using (MemoryStream ms = new())
            {
                using (Stream istream = client.OpenRead(ftpFolder + "/" + fileName, FtpDataType.Binary))
                {

                    int read;
                    try
                    {
                        while ((read = istream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        return ms.ToArray();
                    }
                    catch (Exception ex)
                    {
                        string exp = ex.Message;
                        return buffer;
                    }
                    finally
                    {
                        istream.Close();
                        client.Disconnect();
                    }
                }
            }
        }
        catch
        {
            return buffer;
        }
    }

    public byte[] OpenFile(string ftpServer, string ftpLogin, string ftpPassword, string ftpFolder, string fileName)
    {
        FtpConnetion(ftpServer, ftpLogin, ftpPassword);
        return null;
    }

    public FtpStatusCode RemoveFile(string ftpServer, string ftpLogin, string ftpPassword, string ftpFolder, string fileName, string subFolder)
    => FtpStatusCode.CommandOK;

    public HttpStatusCode UploadFileFtp(byte[] uploadFile, string ftpServer, string ftpLogin, string ftpPass, string fileName)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.34.9/RECEPTION/FILES/" + fileName);
            request.Credentials = new NetworkCredential(ftpLogin, CRPassword.Decrypt(ftpPass));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = uploadFile.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(uploadFile, 0, uploadFile.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            string res = $"Загрузка файла завершена. Статус: {response.StatusDescription}";
            response.Close();
            return HttpStatusCode.OK;
        }
        catch
        {
            return HttpStatusCode.InternalServerError;
        }
    }

    public HttpStatusCode UploadCallFtp(byte[] uploadFile, string ftpServer, string ftpLogin, string ftpPass, string fileName)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.34.9/RECEPTION/" + fileName);
            request.Credentials = new NetworkCredential(ftpLogin, CRPassword.Decrypt(ftpPass));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = uploadFile.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(uploadFile, 0, uploadFile.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            string res = $"Загрузка файла завершена. Статус: {response.StatusDescription}";
            response.Close();
            return HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            string path = @"C:\inetpub\wwwroot\ais_reception_jitsi\Content\log\log-savecall.txt";
            Logger.Log(path, $"Ошибка при загрузке на ФТП {ex.Message}");
            return HttpStatusCode.InternalServerError;
        }
    }
}