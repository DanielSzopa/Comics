using Comics.ApplicationCore.Data;
using Comics.ApplicationCore.Features.Registration;
using Comics.ApplicationCore.Models;
using Comics.IntegrationTests.Helpers;


namespace Comics.IntegrationTests.RegisterAccount;

public class RegisterAccountTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string _baseRegisterResource = "api/account/register";
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public RegisterAccountTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _factory = webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(service =>
            {
                var dbContextOptions = service.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ComicsDbContext>));
                service.Remove(dbContextOptions);
                service.AddDbContext<ComicsDbContext>(builder => builder.UseInMemoryDatabase("ComicsDbInMemory"));
            });
        });

        _httpClient = _factory.CreateClient();
    }


    
    [Theory]
    [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_ValidData),MemberType = typeof(RegisterAccountRequestTestData))]
    public async Task RegisterAccount_ExecuteRegisterAccountEndpointForValidData_ShouldReturnOk(RegisterAccountRequest request)
    {
        //arrange
        var httpContent = request.ToJsonHttpContent();
        //act
        var response = await _httpClient.PostAsync(_baseRegisterResource,httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_InvalidData),MemberType = typeof(RegisterAccountRequestTestData))]
    public async Task RegisterAccount_ExecuteRegisterAccountEndpointForInvalidData_ShouldReturnBadRequest(RegisterAccountRequest request)
    {
        //arrange
        var httpContent = request.ToJsonHttpContent();
        //act
        var response = await _httpClient.PostAsync(_baseRegisterResource,httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_ValidData),MemberType = typeof(RegisterAccountRequestTestData))]
    public async Task RegisterAccount_ForValidModel_ShouldRegisterEntity(RegisterAccountRequest request)
    {
        //arrange
        await ClearUsersInDb();
        var httpContent = request.ToJsonHttpContent();
        //act
        var response = await _httpClient.PostAsync(_baseRegisterResource, httpContent);
        //assert
        var dbResult = await CountUsersInDb();
        using (var scope = new AssertionScope())
        {
            dbResult.Should().Be(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    [Fact]
    public async Task RegisterAccountRequestValidator_ValidateIfUserNameOrEmailExist_ForInvalidData_ShouldReturnBadRequest()
    {
        //arrange
        var registerAccountRequest = new RegisterAccountRequest()
        {
            FirstName = "Daniel",
            LastName = "Szopa",
            Email = "daniel@test123.com",
            UserName = "daniel123",
            Password = "Test123-xx",
            ConfirmPassword = "Test123-xx"
        };

        var user = new User()
        {
            FirstName = registerAccountRequest.FirstName,
            LastName = registerAccountRequest.LastName,
            Email = registerAccountRequest.Email,
            UserName = registerAccountRequest.UserName,
            Password = registerAccountRequest.Password,
        };

        var dbContext = GetComicsDbContextFromServices();
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        //act
        try
        {
            await _httpClient.PostAsync(_baseRegisterResource, registerAccountRequest.ToJsonHttpContent());
        }
        catch (System.Exception ex)
        {

            throw;
        }
        var response = await _httpClient.PostAsync(_baseRegisterResource, registerAccountRequest.ToJsonHttpContent());
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private ComicsDbContext GetComicsDbContextFromServices()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        var comicsDbContext = scope.ServiceProvider.GetService<ComicsDbContext>();
        return comicsDbContext;
    }
    private async Task ClearUsersInDb()
    {
        var comicsDbContext = GetComicsDbContextFromServices();
        var users = comicsDbContext.Users;

        if (users != null)
        {
            comicsDbContext.RemoveRange(users);
            await comicsDbContext.SaveChangesAsync();
        }
            
    }
    private async Task<int> CountUsersInDb()
    {
        var comicsDbContext = GetComicsDbContextFromServices();
        var result = await comicsDbContext.Users.CountAsync();
        return result;
    }
}