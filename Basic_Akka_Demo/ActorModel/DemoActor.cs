using Akka.Actor;
using System;
using System.Threading;

namespace Basic_Akka_Demo.ActorModel
{
    class DemoActor : ReceiveActor
    {
        public DemoActor()
        {
            Receive<Demo>(demo =>
            {
                Console.WriteLine("On Thread: [{0}] \nDemo Message: {1}", Thread.CurrentThread.ManagedThreadId, demo.Message);
            });
        }
    }
}
