using System;

namespace SagaConsole.Commands
{
    public class CreateGuaranteeCommand
    {
        public Guid CorrelationId { get; set; }
        public int Step { get; set; }
        public string Name { get; set; }
        public bool IsIp { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Comission { get; set; }
    }
}