using Demo1.Model;
using System;
using System.Linq;
using System.Security.Cryptography;

public static class HashHelper
{
    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            string saltedPassword = password + salt; // Combine the password and salt
            byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16]; // Salt length can be adjusted
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    public static void UpdatePasswords()
    {
        using (var context = new PBL3_demoEntities())
        {
            var accounts = context.Accounts.ToList();

            foreach (var account in accounts)
            {
                if (account.Salt == null)
                {
                    string salt = GenerateSalt(); // Generate a new salt
                    string hashedPassword = HashPassword(account.accountPassword, salt); // Hash the password with the new salt

                    account.Salt = salt; // Update the salt value
                    account.accountPassword = hashedPassword; // Update the hashed password

                    context.SaveChanges(); // Save the changes to the database
                }
            }
        }
    }
}
