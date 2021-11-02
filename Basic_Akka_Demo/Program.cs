using Akka.Actor;
using Basic_Akka_Demo.ActorModel;
using System;

namespace Basic_Akka_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("DemoSystem");
            var demo = system.ActorOf<DemoActor>("demo");
            demo.Tell(new Demo("Actor 1 message"));
            Console.Read();
        }
    }
}
