using AutoFixture;
using Comics.ApplicationCore.Data;
using Comics.ApplicationCore.Features.Registration;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Comics.UnitTests.RegisterAccount
{
    public class RegisterAccountTests
    {
        private readonly RegisterAccountRequestValidator _registerAccountRequestValidator;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly RegisterAccountController _registerAccountController;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ComicsDbContext _dbContext;

        public RegisterAccountTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mediatorMock = new Mock<IMediator>();
            _registerAccountController = new RegisterAccountController(_mediatorMock.Object);
            var builder = new DbContextOptionsBuilder().UseInMemoryDatabase("ComicsDbInMemory");
            _dbContext = new ComicsDbContext(builder.Options);
            _registerAccountRequestValidator = new RegisterAccountRequestValidator(_dbContext);
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
        public async Task RegisterAccountRequestValidator_ForValidModel_ValidatorShouldNotHaveErrors(RegisterAccountRequest request)
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
            var fixture = new Fixture();
            var requestFixture = fixture.Create<RegisterAccountRequest>();
            requestFixture.Password = password;
            var request = new RegisterAccountRequest() { Password = password };

            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(requestFixture);

            //assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
        
        [Theory]
        [MemberData(nameof(RegisterAccountRequestTestData.Invalid_Passwords), MemberType = typeof(RegisterAccountRequestTestData))]
        public async Task RegisterAccountRequestValidator_ValidatePassword_ForInvalidPassword_ReturnValidationError(string password)
        {
            //arrange
            var fixture = new Fixture();
            var requestFixture = fixture.Create<RegisterAccountRequest>();
            requestFixture.Password = password;
            var request = new RegisterAccountRequest() { Password = password };

            //act
            var result = await _registerAccountRequestValidator.TestValidateAsync(requestFixture);
            _testOutputHelper.WriteLine(result.Errors.Where(x => x.PropertyName == nameof(request.Password)).SingleOrDefault().ErrorMessage);

            //assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region Handler tests

        [Fact]
        public async Task RegisterAccountRequestHandler_ExecuteRequestHandler_ShouldReturnUnitValue()
        {
            //arrange
            var fixture = new Fixture();
            var validatorMock = new Mock<IValidator<RegisterAccountRequest>>();
            var validationResult = new ValidationResult();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterAccountRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            
            var requestHandler = new RegisterAccountRequestHandler(_dbContext, validatorMock.Object);

            //act
            var result = await requestHandler.Handle(fixture.Create<RegisterAccountRequest>(), It.IsAny<CancellationToken>());
            
            //arrange
            result.Should().Be(It.IsAny<Unit>());
        }
        
        #endregion
        
    }

}
