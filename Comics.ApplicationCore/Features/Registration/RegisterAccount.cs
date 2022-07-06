using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Comics.ApplicationCore.Features.Registration
{
    [ApiController]
    [Route("/api/v1/account")]
    public class RegisterController : Controller
    {
        private readonly IMediator _mediator;

        public RegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterAccountDto registerRequest)
        {
            var request = new ReqisterAccountRequest(registerRequest);
            await _mediator.Send(request);
            return Ok();
        }
    }

    public class RegisterAccountDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ReqisterAccountRequest : IRequest
    {
        public RegisterAccountDto RegisterAccountDto { get; set; }
        public ReqisterAccountRequest(RegisterAccountDto registerAccountDto)
        {
            RegisterAccountDto = registerAccountDto;
        }
    }

    public class ReqisterAccountRequestHandler : IRequestHandler<ReqisterAccountRequest>
    {
        public async Task<Unit> Handle(ReqisterAccountRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
