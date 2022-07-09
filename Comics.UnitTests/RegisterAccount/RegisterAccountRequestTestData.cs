using Comics.ApplicationCore.Features.Registration;

namespace Comics.UnitTests.RegisterAccount;

public class RegisterAccountRequestTestData
{
    public static IEnumerable<object[]> RegisterAccountRequest_ValidData()
        {
            var models = new List<RegisterAccountRequest>()
            {
                new RegisterAccountRequest()
                {
                    FirstName = "Daniel",
                    LastName = "Szopa",
                    UserName = "Provader2",
                    Email = "daniel@test.com",
                    Password = "Daniel123!",
                    ConfirmPassword = "Daniel123!",
                },
                new RegisterAccountRequest()
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    UserName = "Provader1",
                    Email = "daniel44@test.com",
                    Password = "test123!S",
                    ConfirmPassword = "test123!S",
                },
                new RegisterAccountRequest()
                {
                    FirstName = "Bartek",
                    LastName = "Nowak",
                    UserName = "Boa",
                    Email = "daniel2@test.com",
                    Password = "aAbbb123!",
                    ConfirmPassword = "aAbbb123!",
                }
            };

            return models.Select(x => new object[] { x });
        }

    public static IEnumerable<object[]> RegisterAccountRequest_InvalidData()
        {
            var models = new List<RegisterAccountRequest>()
            {
                new RegisterAccountRequest()
                {
                    FirstName = "",
                    LastName = "Szopa",
                    UserName = "Provader2",
                    Email = "daniel@test.com",
                    Password = "Daniel123",
                    ConfirmPassword = "Daniel123",
                },
                new RegisterAccountRequest()
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    UserName = "Provader1",
                    Email = "daniel44test.com",
                    Password = "test123",
                    ConfirmPassword = "test123",
                },
                new RegisterAccountRequest()
                {
                    FirstName = "Bartek",
                    LastName = "Nowak",
                    UserName = "Boa",
                    Email = "daniel2@test.com",
                    Password = "bbb123a",
                    ConfirmPassword = "bbb123",
                }
            };

            return models.Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> Invalid_Passwords()
        {
            var passwords = new List<string>()
            {
                "test123",
                "test123!",
                "Test123",
                "TestTest!"
            };

            return passwords.Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> Valid_Passwords()
        {
            var passwords = new List<string>()
            {
                "Test123!",
                "-Daniel123-",
                "Wrrr233!",
                "TestTestdadadadadadada!1"
            };

            return passwords.Select(x => new object[] { x });
        }
}