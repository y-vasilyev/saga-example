using System;

namespace SagaConsole.Commands
{
    public class UploadPackage1Command : IBgInfo
    {

        public Guid CorrelationId { get; set; }
    }
}