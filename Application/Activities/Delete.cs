using MediatR;
using Persistence;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Activities
{
    public class Delete
    {
         public class Command : IRequest 
                {
                    public Guid Id {get; set;}

                }
        
                public class Handler : IRequestHandler<Command>
                {
                    private readonly DataContext _context;
                    public Handler(DataContext context)
                    {
                        _context = context;
                    }
                    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                    {
                        var activity = await _context.Activities.FindAsync(request.Id);
                        if (activity == null)
                        {
                            throw new Exception("Activity not found");
                        }
                        _context.Remove(activity);
                        // Handler logic
                        var success = await _context.SaveChangesAsync() > 0;
                        if (success)
                        {
                            return Unit.Value;
                        }
                        else 
                        {
                            throw new Exception("Problem saving changes");
                        }
                    }
                }
    }
}