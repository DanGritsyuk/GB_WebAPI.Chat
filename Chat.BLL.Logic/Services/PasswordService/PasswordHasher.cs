using System.Security.Cryptography;
using System.Text;

namespace HireWise.BLL.Logic.Services
{
    public class PasswordHasher
    {
        public string HashPassword(string password) =>
            ComputeMD5Hash(password);

        public bool VerifyPassword(string password, string hashedPassword) =>
            ComputeMD5Hash(password) == hashedPassword;

        private static string ComputeMD5Hash(string input)
        {
            // Создаем новый экземпляр MD5
            using (MD5 md5 = MD5.Create())
            {
                // Преобразуем строку в массив байтов
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Вычисляем хеш
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Преобразуем байты в шестнадцатеричную строку
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Форматируем в шестнадцатеричный вид
                }
                return sb.ToString();
            }
        }
    }
}