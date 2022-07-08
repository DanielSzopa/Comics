﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Comics.ApplicationCore.Features.Registration
{
    [ApiController]
    public class RegisterAccountEndpoint : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegisterAccountEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ActionResult<string>> RegisterAccount([FromBody] RegisterAccountRequest registerAccountRequest)
        {
            await _mediator.Send(registerAccountRequest);
            return Ok(" ");
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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class RegisterAccountHandler : IRequestHandler<RegisterAccountRequest>
    {
        public async Task<Unit> Handle(RegisterAccountRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
