using Comics.ApplicationCore.Features.Registration;

namespace Comics.UnitTests
{
    public class RegisterAccountTests : IClassFixture<RegisterAccountRequestValidator>
    {
        private readonly RegisterAccountRequestValidator _registerAccountRequestValidator;
        private readonly ITestOutputHelper _testOutputHelper;

        public RegisterAccountTests(RegisterAccountRequestValidator registerAccountRequestValidator,ITestOutputHelper testOutputHelper)
        {
            _registerAccountRequestValidator = registerAccountRequestValidator;
            _testOutputHelper = testOutputHelper;
        }

        #region Test Data
        private static IEnumerable<object[]> GetCorrectModelsForValidator()
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

        private static IEnumerable<object[]> GetInvalidModelsForValidator()
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

        private static IEnumerable<object[]> GetInvalidPasswordForValidator()
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

        private static IEnumerable<object[]> GetValidPasswordForValidator()
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
        #endregion

        #region Controller tests
        [Fact]
        public async Task RegisterAccount_ExecuteRegisterController_ShouldReturnStatusOk()
        {
            //arrange
            var mediatorMock = new Mock<IMediator>();
            var registerAccountController = new RegisterAccountController(mediatorMock.Object);
            //act
            var actionResult = await registerAccountController.RegisterAccount(It.IsAny<RegisterAccountRequest>());
            var result = actionResult.Result as OkObjectResult;
            //assert
            using (new AssertionScope())
            {
                result.StatusCode.Should().Be((int)HttpStatusCode.OK);
                result.Value.Should().BeOfType(typeof(string));
            }
        }

        [Fact]
        public async Task RegisterAccount_ExecuteControllerWithMediator_MediatorShouldExecute()
        {
            //arrange
            var mediatorMock = new Mock<IMediator>();
            var registerAccountController = new RegisterAccountController(mediatorMock.Object);

            //act
            await registerAccountController.RegisterAccount(It.IsAny<RegisterAccountRequest>());

            //arrange
            mediatorMock.Verify(x => x.Send(It.IsAny<RegisterAccountRequest>(),It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Validator tests
        [Theory]
        [MemberData(nameof(GetCorrectModelsForValidator))]
        public async Task RegisterAccountRequestValidator_ForCorrectModel_ValidatorShouldNotHaveErrors(RegisterAccountRequest request)
        {
            //arrange
            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);
            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

      
        [Theory]
        [MemberData(nameof(GetInvalidModelsForValidator))]
        public async Task RegisterAccountRequestValidator_ForInvalidModel_ValidatorShouldHaveValidationErrors(RegisterAccountRequest request)
        {
            //arrange
            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);

            //assert
            result.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(GetInvalidPasswordForValidator))]
        public async Task RegisterAccountRequestValidator_ValidatePassword_ForInvalidPassword_ReturnValidationError(string password)
        {
            //arrange
            var request = new RegisterAccountRequest() { Password = password };

            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);
            _testOutputHelper.WriteLine(result.Errors.Where(x => x.PropertyName == nameof(request.Password)).SingleOrDefault().ErrorMessage);

            //assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [MemberData(nameof(GetValidPasswordForValidator))]
        public async Task RegisterAccountRequestValidator_ValidatePassword_ForValidPassword_ReturnValidationError(string password)
        {
            //arrange
            var request = new RegisterAccountRequest() { Password = password };

            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);

            //assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion
    }

}
