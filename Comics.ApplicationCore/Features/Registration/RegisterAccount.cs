using Comics.ApplicationCore.Data;
using Comics.ApplicationCore.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Comics.ApplicationCore.Features.Registration
{
    
    [ApiController]
    [Route("api/account")]
    public class RegisterAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegisterAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterAccountRequest registerAccountRequest)
        {
            await _mediator.Send(registerAccountRequest);
            return Ok();
        }
    }

    public class RegisterAccountRequestValidator : AbstractValidator<RegisterAccountRequest>
    {
        public RegisterAccountRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required")
                .MinimumLength(2)
                .WithMessage("Minimum length is 2")
                .MaximumLength(50)
                .WithMessage("Maximum length is 50");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required")
                .MinimumLength(2)
                .WithMessage("Minimum length is 2")
                .MaximumLength(50)
                .WithMessage("Maximum length is 50");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName is required")
                .MinimumLength(3)
                .WithMessage("Minimum length is 3")
                .MaximumLength(15)
                .WithMessage("Maximum length is 15");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is invalid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,24}$")
                .WithMessage("Password requirements: \n At least 8 characters \n Max 24 characteres \n At least one number \n Inclusion of at least one special character, e.g., ! @ # ? ]");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

        }
    }

    public class RegisterAccountRequest : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterAccountRequestHandler : IRequestHandler<RegisterAccountRequest>
    {
        private readonly ComicsDbContext _dbContext;

        public RegisterAccountRequestHandler(ComicsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(RegisterAccountRequest request, CancellationToken cancellationToken)
        {
            var user = MapRegisterAccountRequestToUserEntity(request);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

        private User MapRegisterAccountRequestToUserEntity(RegisterAccountRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password
            };
            return user;
        }
    }
}
