using Comics.ApplicationCore.Data;
using Comics.ApplicationCore.Features.Registration;
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
    public async Task RegisterAccount_ExecuteRegisterAccountEndpoint_ShouldReturnOk(RegisterAccountRequest request)
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
    public async Task RegisterAccount_ExecuteRegisterAccountEndpoint_ShouldReturnBadRequest(RegisterAccountRequest request)
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
    public async Task RegisterAccountRequestHandler_ForValidModel_ShouldRegisterEntity(RegisterAccountRequest request)
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