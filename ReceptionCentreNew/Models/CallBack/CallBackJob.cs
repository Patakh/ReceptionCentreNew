using ReceptionCentreNew.Data.Context.App; 
using System.Diagnostics;

namespace ReceptionCentreNew.Models;
public class CallBackJob : IDisposable
{
    private CancellationTokenSource m_cancel;
    private Task m_task;
    private TimeSpan m_interval;
    private bool m_running;

    public CallBackJob(TimeSpan interval)
    {
        m_interval = interval;
        m_running = true;
        m_cancel = new CancellationTokenSource();
        m_task = Task.Run(() => TaskLoop(), m_cancel.Token);
    }

    private async Task TaskLoop()
    {
        string path = @"C:\inetpub\wwwroot\ais_reception_jitsi\Content\log\log-callback.txt";
        while (m_running)
        {
            using (HttpClient client = new())
            {
                CallBackClient callBackClient = new("http://192.168.34.191:3838/", "rest/", "ordered");
                var res = await callBackClient.RefreshCallBack();
                string txt = $"all-{res?.AllCount}-process-{res?.ProcessCount}-error-{res?.ErrorCount}-update-{res?.UpdateCount}-notsyncinbase-{res?.SyncCount}-sync-{res?.SyncCount}-date-{DateTime.Now}";
                Debug.WriteLine(txt);
                Log(path, txt);
            }
            Thread.Sleep(m_interval);
        }
    }

    public void Dispose()
    {
        Debug.WriteLine("Остановка метода");
        m_running = false;

        if (m_cancel != null)
        {
            try
            {
                m_cancel.Cancel();
                m_cancel.Dispose();
            }
            catch
            {
            }
            finally
            {
                m_cancel = null;
            }
        }
    }
    public static void Log(string path, string txt)
    {
        string writePath = path;
        string text = txt;
        try
        {
            using (StreamWriter sw = new(writePath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(text);
            }
            Console.WriteLine("Запись выполнена");
        }
        catch (Exception e)
        {
            using (ReceptionCentreContext db = new())
            {
                var model = db.DataSystemErrors.Add(new DataSystemErrors { EmployeesName = "Система", ErrorDate = DateTime.Now, ErrorInnerException = "", ErrorMessage = e.Message, StackTrace = "-", SprEmployeesId = Guid.Parse("3155fc8f-7bd4-4ba9-97e4-969c3ccd5a58") });
                db.SaveChanges();
            }
        }
    }
}