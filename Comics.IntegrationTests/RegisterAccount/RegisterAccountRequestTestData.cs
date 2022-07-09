using Comics.ApplicationCore.Features.Registration;

namespace Comics.IntegrationTests.RegisterAccount;

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
                UserName = "Provader",
                Email = "daniel@test.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            },
            new RegisterAccountRequest()
            {
                FirstName = "Bartek",
                LastName = "Nowak",
                UserName = "Balcerzak",
                Email = "Badaniel@test.com",
                Password = "America123!",
                ConfirmPassword = "America123!"
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
                FirstName = "Daniel",
                LastName = "Szopa",
                UserName = "Provader",
                Email = "daniel@test.com",
                Password = "Test123aadsds!",
                ConfirmPassword = "Test123!"
            },
            new RegisterAccountRequest()
            {
                FirstName = "Bartek",
                LastName = "Nowak",
                UserName = "Balcerzak",
                Email = "Badaniel.com",
                Password = "America123!",
                ConfirmPassword = "America123!"
            }
        };
        return models.Select(x => new object[] { x });
    }
}