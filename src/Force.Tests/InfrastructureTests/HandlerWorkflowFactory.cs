using System.Threading.Tasks;
using Force.Ccc;
using Force.Validation;
using Force.Workflow;

namespace Force.Tests.InfrastructureTests
{
    public class HandlerWorkflowFactory<TRequest, TResult>: IWorkflow<TRequest, TResult>
    {
        public Result<TResult, FailureInfo> Process(TRequest request, IServiceFactory sp)
        {
            var wf = CreateDefaultWorkflow<TRequest, TResult>(sp);
            return wf.Process(request, sp);
        }
        
        public static HandlerWorkflow<TRequest, TResponse> CreateDefaultWorkflow<TRequest, TResponse>(
            IServiceFactory sp)
        {
            var validator = sp.GetService<IValidator<TRequest>>();
            var uow = sp.GetService<IUnitOfWork>();
            
            return validator == null
                ? new HandlerWorkflow<TRequest, TResponse>(new UnitOfWorkWorkflowStep<TRequest, TResponse>(uow))
                : new HandlerWorkflow<TRequest, TResponse>(
                    new ValidateWorkflowStep<TRequest, TResponse>(validator),
                    new UnitOfWorkWorkflowStep<TRequest, TResponse>(uow));
        }
    }
    
    public class HandlerAsyncWorkflowFactory<TRequest, TResult>: IAsyncWorkflow<TRequest, TResult>
    {
        public Task<Result<TResult, FailureInfo>> ProcessAsync(TRequest request, IServiceFactory sp)
        {
            var wf = CreateDefaultWorkflowAsync<TRequest, TResult>(sp);
            return wf.ProcessAsync(request, sp);
        }
        
        public static AsyncHandlerWorkflow<TRequest, TResponse> CreateDefaultWorkflowAsync<TRequest, TResponse>(
            IServiceFactory sp)
        {
            var validatorAsync = sp.GetService<IAsyncValidator<TRequest>>();
            var uow = sp.GetService<IUnitOfWork>();
            
            return validatorAsync == null
                ? new AsyncHandlerWorkflow<TRequest, TResponse>(new UnitOfWorkWorkflowStep<TRequest, TResponse>(uow))
                : new AsyncHandlerWorkflow<TRequest, TResponse>(
                    new UnitOfWorkWorkflowStep<TRequest, TResponse>(uow),
                    new ValidateWorkflowAsyncStep<TRequest, TResponse>(validatorAsync));
        }
    }
}