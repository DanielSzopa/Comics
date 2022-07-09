using Comics.ApplicationCore.Features.Registration;

namespace Comics.UnitTests.RegisterAccount
{
    public class RegisterAccountTests : IClassFixture<RegisterAccountRequestValidator>
    {
        private readonly RegisterAccountRequestValidator _registerAccountRequestValidator;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly RegisterAccountController _registerAccountController;
        private readonly Mock<IMediator> _mediatorMock;

        public RegisterAccountTests(RegisterAccountRequestValidator registerAccountRequestValidator,ITestOutputHelper testOutputHelper)
        {
            _registerAccountRequestValidator = registerAccountRequestValidator;
            _testOutputHelper = testOutputHelper;
            _mediatorMock = new Mock<IMediator>();
            _registerAccountController = new RegisterAccountController(_mediatorMock.Object);
        }
        
        #region Controller tests
        [Fact]
        public async Task RegisterAccount_ExecuteRegisterEndpoint_ShouldReturnStatusOk()
        {
            //arrange
            //act
            var actionResult = await _registerAccountController.RegisterAccount(It.IsAny<RegisterAccountRequest>());
            var result = actionResult as OkResult;
            //assert
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterAccount_ExecuteEndpointWithMediator_MediatorShouldExecute()
        {
            //arrange
            //act
            await _registerAccountController.RegisterAccount(It.IsAny<RegisterAccountRequest>());

            //arrange
            _mediatorMock.Verify(x => x.Send(It.IsAny<RegisterAccountRequest>(),It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Validator tests
        [Theory]
        [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_ValidData), MemberType = typeof(RegisterAccountRequestTestData))]
        public async Task RegisterAccountRequestValidator_ForCorrectModel_ValidatorShouldNotHaveErrors(RegisterAccountRequest request)
        {
            //arrange
            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);
            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

      
        [Theory]
        [MemberData(nameof(RegisterAccountRequestTestData.RegisterAccountRequest_InvalidData), MemberType = typeof(RegisterAccountRequestTestData))]
        public async Task RegisterAccountRequestValidator_ForInvalidModel_ValidatorShouldHaveValidationErrors(RegisterAccountRequest request)
        {
            //arrange
            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);

            //assert
            result.ShouldHaveAnyValidationError();
        }
        
        [Theory]
        [MemberData(nameof(RegisterAccountRequestTestData.Valid_Passwords), MemberType = typeof(RegisterAccountRequestTestData))]
        public async Task RegisterAccountRequestValidator_ValidatePassword_ForValidPassword_ReturnValidationError(string password)
        {
            //arrange
            var request = new RegisterAccountRequest() { Password = password };

            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(request);

            //assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
        
        [Theory]
        [MemberData(nameof(RegisterAccountRequestTestData.Invalid_Passwords), MemberType = typeof(RegisterAccountRequestTestData))]
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

        #endregion

        #region Handler tests

        public async Task RegisterAccountRequestHandler_ExecuteRequestHandler_ShouldReturnUnitValue()
        {
            //arrange
            var requestHandler = new RegisterAccountRequestHandler();
            
            //act
            var result = await requestHandler.Handle(It.IsAny<RegisterAccountRequest>(), It.IsAny<CancellationToken>());
            
            //arrange
            result.Should().Be(It.IsAny<Unit>());
        }
        

        #endregion
    }

}
