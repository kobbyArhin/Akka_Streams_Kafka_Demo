using Akka;
using Akka.Actor;
using Akka.Configuration;
using Akka.Streams;
using Akka.Streams.Dsl;
using Akka.Streams.Kafka.Dsl;
using Akka.Streams.Kafka.Messages;
using Akka.Streams.Kafka.Settings;
using Confluent.Kafka;
using System;
using System.Linq;
using Config = Akka.Configuration.Config;

namespace Akka_Kafka_Demo.Services
{
    class StreamProducer
    {
        //public static async Task Play()
        public void Play()
        {
            Config fallbackConfig = ConfigurationFactory.ParseString(@"
                    akka.suppress-json-serializer-warning=true
                    akka.loglevel = DEBUG
                ").WithFallback(ConfigurationFactory.FromResource<ConsumerSettings<object, object>>("Akka.Streams.Kafka.reference.conf"));

            var system = ActorSystem.Create("DemoActor", fallbackConfig);
            var materializer = system.Materializer();

            var producerSettings = ProducerSettings<Null, string>.Create(system, null, null)
                .WithBootstrapServers("localhost:9092");

            Source.From(Enumerable.Range(1, 10))
                .Select(c => c.ToString())
                .Select(elem => ProducerMessage.Single(new ProducerRecord<Null, string>("demo", $"Producer Message No: {elem}")))
                .Via(KafkaProducer.FlexiFlow<Null, string, NotUsed>(producerSettings))
                .Select(result =>
                {
                    var response = result as Result<Null, string, NotUsed>;
                    Console.WriteLine($"Producer: {response.Metadata.Topic}/{response.Metadata.Partition} {response.Metadata.Offset}: {response.Metadata.Value}");
                    return result;
                })
                .RunWith(Sink.Ignore<IResults<Null, string, NotUsed>>(), materializer);

            Console.ReadKey();

            system.Terminate();
        }
    }
}
