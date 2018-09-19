using Automatonymous;
using SagaConsole.Commands;

namespace SagaConsole.StateManagment
{
    public sealed partial class BankGuaranteeStateMachine
    {
        public State AwaitingFinDocs { get; set; }
        public State AwaitingOrgDocs { get; set; }
        public State AwaitingDecisionAboutBg { get; set; }
        public State Approved { get; set; }
        public State Rejected { get; set; }

        public Event<CreateGuaranteeCommand> StartWorkflowCommandReceived { get; set; }
        public Event<UploadPackage1Command> Package1Uploaded { get; set; }
        public Event<UploadPackage2Command> Package2Uploaded { get; set; }


        public Event<BgApproved> BgApproved { get; set; }

        public Event<BgRejected> BgRejected { get; set; }


        private void BuildStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => StartWorkflowCommandReceived,
                   x => x.CorrelateById(ctx => ctx.Message.CorrelationId)
                        .SelectId(context => context.Message.CorrelationId));

            Event(() => Package1Uploaded, x => x.CorrelateById(ctx =>
                ctx.Message.CorrelationId));

            Event(() => Package2Uploaded, x => x.CorrelateById(ctx =>
                ctx.Message.CorrelationId));


            Event(() => BgApproved, x => x.CorrelateById(ctx =>
                ctx.Message.CorrelationId));


            Event(() => BgRejected, x => x.CorrelateById(ctx =>
                ctx.Message.CorrelationId));
        }
    }
}