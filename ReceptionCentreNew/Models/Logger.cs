using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ReceptionCentreNew.Models
{
    public class Logger
    {
        public static void Log(string path, string txt)
        {
            string writePath = path;
            string text = txt;
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss fff") + "-" + text);
                }
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception e)
            {
                using (ReceptionCentreContext db = new ReceptionCentreContext())
                {
                    var model = db.DataSystemErrors.Add(new DataSystemErrors { EmployeesName = "Система", ErrorDate = DateTime.Now, ErrorInnerException = "", ErrorMessage = e.Message, StackTrace = "-", SprEmployeesId = Guid.Parse("3155fc8f-7bd4-4ba9-97e4-969c3ccd5a58") });
                    db.SaveChanges();
                }
            }
        }
    }
}