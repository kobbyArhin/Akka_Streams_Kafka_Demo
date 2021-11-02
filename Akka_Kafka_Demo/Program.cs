using Akka_Kafka_Demo.Services;

namespace Akka_Kafka_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //StreamConsumer consumerStream = new StreamConsumer();
            //consumerStream.Play();
            StreamProducer streamProducer = new StreamProducer();
            streamProducer.Play();
        }
    }
}
