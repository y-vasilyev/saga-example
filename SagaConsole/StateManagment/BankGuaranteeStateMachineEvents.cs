using Automatonymous;
using Automatonymous.Binders;
using SagaConsole.Commands;
using SagaConsole.Saga;

namespace SagaConsole.StateManagment
{
    public sealed partial class BankGuaranteeStateMachine : MassTransitStateMachine<BankguaranteeSaga>
    {
        public BankGuaranteeStateMachine()
        {
            BuildStateMachine();
            Initially(WhenStartWorkflowCommandReceived());
            During(AwaitingFinDocs, WhenAwaitingFinDocs());
            During(AwaitingOrgDocs, WhenAwaitingOrgDocs());
            During(AwaitingDecisionAboutBg,
                WhenBgApproved(),
                WhenBgDeclined());
        }

        private EventActivityBinder<BankguaranteeSaga, CreateGuaranteeCommand>
                      WhenStartWorkflowCommandReceived()
        {
            return When(StartWorkflowCommandReceived)
                .Then(ctx => ctx.Instance.SaveConfigurationRequestInfo(ctx.Data))
                //.Send(TaskManagerQueue, ctx => new CreateTaskCommand(ctx.Instance))
                .TransitionTo(AwaitingFinDocs);
        }

        private EventActivityBinder<BankguaranteeSaga, UploadPackage1Command>
                       WhenAwaitingFinDocs()
        {
            return When(Package1Uploaded)
                .Then(ctx => ctx.Instance.UploadPackage1(ctx.Data))                
                .TransitionTo(AwaitingOrgDocs);
        }

        private EventActivityBinder<BankguaranteeSaga, UploadPackage2Command>
                       WhenAwaitingOrgDocs()
        {
            return When(Package2Uploaded)
                .Then(ctx => ctx.Instance.UploadPackage2(ctx.Data))
                .TransitionTo(AwaitingDecisionAboutBg);
        }

        private EventActivityBinder<BankguaranteeSaga, BgApproved>
                       WhenBgApproved()
        {
            return When(BgApproved)
                .Then(ctx => ctx.Instance.MarkBgAsApproved(ctx.Data))
                .Finalize();
        }

        private EventActivityBinder<BankguaranteeSaga, BgRejected>
                       WhenBgDeclined()
        {
            return When(BgRejected)
                .Then(ctx => ctx.Instance.MarkBgAsDeclined(ctx.Data))
                .TransitionTo(Rejected);
        }
    }
}