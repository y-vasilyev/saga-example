using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Saga;
using SagaConsole.Commands;
using SagaConsole.Saga;
using SagaConsole.StateManagment;

namespace SagaConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var sagaStateMachine = new BankGuaranteeStateMachine();
            var repository = new InMemorySagaRepository<BankguaranteeSaga>();
            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                x.ReceiveEndpoint(host, "bg_state", e =>
                {
                    e.StateMachineSaga(sagaStateMachine, repository);
                });

                x.UseInMemoryOutbox(); // for testing, to make it easy

            });

            Console.WriteLine("Hello World!");

           Task.Run(() => { busControl.Start(); });
            
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            busControl.Publish(new CreateGuaranteeCommand { CorrelationId = id }).Wait();
            busControl.Publish(new UploadPackage1Command { CorrelationId = id }).Wait();

            busControl.Publish(new CreateGuaranteeCommand { CorrelationId = id2 }).Wait();

            busControl.Publish(new UploadPackage2Command { CorrelationId = id }).Wait();
            busControl.Publish(new BgApproved { CorrelationId = id }).Wait();

            busControl.Publish(new UploadPackage1Command { CorrelationId = id2 }).Wait();

            Console.ReadLine();
        }
    }
}
