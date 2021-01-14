using System;
using System.Security.Cryptography;

namespace Temporary_Test_Project
{
    class Program
    {
        private static void Log(object info)
        {
            Console.WriteLine(info);
            Console.WriteLine();
        }

        public class Activity
        {
            public Activity()
            {
                this.Id = Guid.NewGuid(); // ef sets this?
                this.DateBooked = DateTime.Now;
            }
            public Activity(
                            string name,
                            string imageUrl,
                            DateTime bookedForDate,
                            DateTime startTime,
                            DateTime endTime,
                            int itemQuantity,
                            int currentUserCount,
                            int minUserCount,
                            int maxUserCount,
                            string description,
                            string location,
                            string organizerName
            )
            {
                this.Id = Guid.NewGuid(); // ef sets this?

                this.Name = name;
                this.ImageUrl = imageUrl;

                this.DateBooked = DateTime.Now;
                this.BookedForDate = bookedForDate;
                this.StartTime = startTime;
                this.EndTime = endTime;

                this.ItemQuantity = itemQuantity;
                this.CurrentUserCount = currentUserCount;
                this.MinUserCount = minUserCount;
                this.MaxUserCount = maxUserCount;

                this.Description = description;
                this.Location = location;
                this.OrganizerName = organizerName;
            }


            public Guid Id { get; set; }

            public string Name { get; set; }
            public string ImageUrl { get; set; }

            public DateTime DateBooked { get; set; }
            public DateTime BookedForDate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }


            public int ItemQuantity { get; set; }
            public int CurrentUserCount { get; set; }
            public int MinUserCount { get; set; }
            public int MaxUserCount { get; set; }

            public string Description { get; set; }
            public string Location { get; set; }
            public string OrganizerName { get; set; }
            public string Duration
            {
                get
                {
                    TimeSpan duration = new TimeSpan(0, 0, 0);
                    DateTime defaultDate = new DateTime(); // 1/1/0001

                    if (this.EndTime > defaultDate && this.StartTime > defaultDate)
                    {
                        duration = this.EndTime - this.StartTime;
                    }

                    var durationDay = duration.Days;
                    int durationHour = duration.Hours;
                    int durationMinute = duration.Minutes;

                    string day = durationDay >= 1 ? $"{durationDay} day/s" : "";
                    string hour = durationHour >= 1 ? $"{durationHour} hour/s" : "";
                    string minute = durationMinute >= 1 ? $"{durationMinute} minute/s" : "";

                    string finalDuration = (durationDay == 0 && durationHour == 0 && durationMinute == 0) ?
                            "Can't calculate duration, invalid start time or end time." :
                            $"{day} {hour} {minute}";

                    return finalDuration;
                }
            }
        }

        public static class SecurePasswordHasher
        {
            /// <summary>
            /// Size of salt.
            /// </summary>
            private const int SaltSize = 16;

            /// <summary>
            /// Size of hash.
            /// </summary>
            private const int HashSize = 20;

            /// <summary>
            /// Creates a hash from a password.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="iterations">Number of iterations.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password, int iterations)
            {
                // Create salt
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] salt;
                    rng.GetBytes(salt = new byte[SaltSize]);
                    using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                    {
                        var hash = pbkdf2.GetBytes(HashSize);
                        // Combine salt and hash
                        var hashBytes = new byte[SaltSize + HashSize];
                        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                        // Convert to base64
                        var base64Hash = Convert.ToBase64String(hashBytes);

                        // Format hash with extra information
                        return $"$ActivityScheduler|V1${iterations}${base64Hash}";
                    }
                }

            }

            /// <summary>
            /// Creates a hash from a password with 10000 iterations
            /// </summary>
            /// <param name="password">The password.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password)
            {
                return Hash(password, 10000);
            }

            /// <summary>
            /// Checks if hash is supported.
            /// </summary>
            /// <param name="hashString">The hash.</param>
            /// <returns>Is supported?</returns>
            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("$ActivityScheduler|V1$");
            }

            /// <summary>
            /// Verifies a password against a hash.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="hashedPassword">The hash.</param>
            /// <returns>Could be verified?</returns>
            public static bool Verify(string password, string hashedPassword)
            {
                // Check hash
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("The hashtype is not supported");
                }

                // Extract iteration and Base64 string
                var splittedHashString = hashedPassword.Replace("$ActivityScheduler|V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];

                // Get hash bytes
                var hashBytes = Convert.FromBase64String(base64Hash);

                // Get salt
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Create hash with given salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    // Get result
                    for (var i = 0; i < HashSize; i++)
                    {
                        if (hashBytes[i + SaltSize] != hash[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

            }
        }

        static void Main(string[] args)
        {
            Log(DateTime.Now);

            var testActivity = new Activity();
            testActivity.StartTime = DateTime.Now;
            testActivity.EndTime = DateTime.Now.AddDays(0).AddHours(0).AddMinutes(0);

            Log(testActivity.Duration);


            string userPwd = "testPassword";

            // Hash
            var hashedPwd = SecurePasswordHasher.Hash(userPwd);

            // Verify
            var isMatch = SecurePasswordHasher.Verify(userPwd, hashedPwd);

            Log($"Hash: {hashedPwd}");
            Log($"Password matches with the hash: {isMatch}");
        }
    }
}
