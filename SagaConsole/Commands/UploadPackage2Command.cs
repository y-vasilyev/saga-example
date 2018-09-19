using System;

namespace SagaConsole.Commands
{
    public class UploadPackage2Command
    {
        public Guid CorrelationId { get; set; }
    }

    public class BgApproved : IBgInfo
    {
        public Guid CorrelationId { get; set; }
    }

    public class BgRejected : IBgInfo
    {
        public Guid CorrelationId { get; set; }

        public string Comment { get; set; }
    }

    public interface IBgInfo
    {
        Guid CorrelationId { get; set; }
    }
}