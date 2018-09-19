using System;
using Automatonymous;
using SagaConsole.Commands;
using SagaConsole.Enums;

namespace SagaConsole.Saga
{
    public class BankguaranteeSaga
    : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public int Step { get; set; }
        public string Inn { get; set; }
        public string Name { get; set; }

        public bool IsIp { get; set; }

        public decimal? Amount { get; set; }
        public decimal? Comission { get; set; }

        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }


        public void SaveConfigurationRequestInfo(CreateGuaranteeCommand command)
        {
            CorrelationId = command.CorrelationId;
            Step = command.Step;
            Name = command.Name;
            IsIp = command.IsIp;
            Amount = command.Amount;
            Comission = command.Comission;

            Console.WriteLine($"Bg created {CorrelationId}");
        }

        public void UploadPackage1(UploadPackage1Command command)
        {
           UpdateBgInfo(command, BgStatus.New);
           Console.WriteLine($"UploadPackage1 {CorrelationId}");
        }

        public void MarkBgAsApproved(BgApproved taskInfo)
        {
            UpdateBgInfo(taskInfo, BgStatus.Approved);
            Console.WriteLine($"MarkBgAsApproved {CorrelationId}");
            //CompletedOn = RuntimeContext.Current.DateTimeOffset.Now;
        }

        public void MarkBgAsDeclined(BgRejected taskInfo)
        {
            UpdateBgInfo(taskInfo, BgStatus.Rejected, taskInfo.Comment);
            Console.WriteLine($"MarkBgAsApproved {CorrelationId}");
            //CompletedOn = RuntimeContext.Current.DateTimeOffset.Now;
        }

        private void UpdateBgInfo(IBgInfo taskInfo, BgStatus bgStatus, string comment = null)
        {
            // Тут что-то делаем
        }

        public void UploadPackage2(UploadPackage2Command objData)
        {
            Console.WriteLine($"UploadPackage2 {CorrelationId}");
        }
    }
}