using FlightReservation.Application.Interfaces.Repositories;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Domain.Primitives;
using MediatR;

namespace FlightReservation.Application.Features.Users;

public record LoginCommand(string Mobile, string Password ) : IRequest<LoginResponse>;
public record LoginResponse(string Token);
// Features/Flights/CreateFlightHandler.cs
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtService _jwtService; 
    private readonly IEmailService _emailService;
    
    public LoginCommandHandler(IUserRepository userRepository, IEncryptionService encryptionService, IJwtService jwtService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _jwtService = jwtService;
        _emailService = emailService;
    }
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetUser(ct,request.Mobile);
        
        if (user is null)
            throw new FlightReservationException(400,"User not found.");

        var hashedPassword = _encryptionService.Encrypt(request.Password);

        if (!user.Password.Equals(hashedPassword))
            throw new FlightReservationException(400,"Combination of username and password are not correct.");
        
        var token = _jwtService.GenerateToken(user.Id);
        
        Task.Run(() => _emailService.SendEmailAsync(user.Email,
            "Succesful login", 
            $@"
                    Dear {user.FirstName} {user.LastName},
                    You've successfully logged in.
                "));
        
        return new LoginResponse(token);
    }
}