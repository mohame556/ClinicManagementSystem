using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Clinc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SystemController : Controller
    {
        private readonly IConfiguration _configuration;

        public SystemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Backup()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConString");

                string backupFolder = @"D:\ClinicBackups";

                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string backupFile = Path.Combine(
                    backupFolder,
                    $"Clinic_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = $@"
                    BACKUP DATABASE [clinic]
                    TO DISK = N'{backupFile}'
                    WITH INIT,
                    NAME='Clinic Backup';";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "تم إنشاء النسخة الاحتياطية بنجاح.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index", "Users");
        }
    }
}