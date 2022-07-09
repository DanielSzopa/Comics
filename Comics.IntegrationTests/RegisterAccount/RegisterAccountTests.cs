using Comics.ApplicationCore.Features.Registration;
using Comics.IntegrationTests.Helpers;



namespace Comics.IntegrationTests.RegisterAccount;

public class RegisterAccountTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public RegisterAccountTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }


    
    [Theory]
    [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_ValidData),MemberType = typeof(RegisterAccountRequestTestData))]
    public async Task RegisterAccount_ExecuteRegisterAccountEndpoint_ShouldReturnOk(RegisterAccountRequest request)
    {
        //arrange
        //act
        var response = await _httpClient.PostAsync("api/account/register",request.ToJsonHttpContent());
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_InvalidData),MemberType = typeof(RegisterAccountRequestTestData))]
    public async Task RegisterAccount_ExecuteRegisterAccountEndpoint_ShouldReturnBadRequest(RegisterAccountRequest request)
    {
        //arrange
        //act
        var response = await _httpClient.PostAsync("api/account/register",request.ToJsonHttpContent());
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}