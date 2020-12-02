using DemoProject.Domain.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Bootstrapper.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly DemoProjectDBContext _context;

        public UnitOfWorkBehavior(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(DemoProjectDBContext));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            await _context.SaveChangesAsync();
            return response;
        }
    }
}
