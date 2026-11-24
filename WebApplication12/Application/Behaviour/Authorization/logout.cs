using Application.Interfaces.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.Authorization
{
    public class LogoutCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }

    public class LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<LogoutCommand, bool>
    {
        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<RefreshTockensEntity> tokens =
                await refreshTokenRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            foreach (RefreshTockensEntity token in tokens)
            {
                token.IsUsed = true;
                await refreshTokenRepository.UpdateAsync(token);
            }

            return true;
        }
    }
}