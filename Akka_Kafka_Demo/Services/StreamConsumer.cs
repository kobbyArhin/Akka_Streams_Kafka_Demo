using Akka.Actor;
using Akka.Configuration;
using Akka.Streams;
using Akka.Streams.Kafka.Dsl;
using Akka.Streams.Kafka.Settings;
using Confluent.Kafka;
using System;
using Config = Akka.Configuration.Config;

namespace Akka_Kafka_Demo.Services
{
    class StreamConsumer
    {
        public void Play()
        {
            Config fallbackConfig = ConfigurationFactory.ParseString(@"
                    akka.suppress-json-serializer-warning=true
                    akka.loglevel = DEBUG
                ").WithFallback(ConfigurationFactory.FromResource<ConsumerSettings<object, object>>("Akka.Streams.Kafka.reference.conf"));

            var system = ActorSystem.Create("DemoActor", fallbackConfig);
            var materializer = system.Materializer();

            var consumerSettings = ConsumerSettings<Null, string>.Create(system, null, null)
                .WithBootstrapServers("localhost:9092")
                .WithGroupId("group1");

            var subscription = Subscriptions.Topics("demo");

            var cocons = KafkaConsumer.PlainSource(consumerSettings, subscription)
                .RunForeach(result =>
                {
                    Console.WriteLine($"Consumer: {result.Topic}/{result.Partition} {result.Offset}: {result.Value}");
                }, materializer);

            Console.ReadLine();
        }
    }
}
