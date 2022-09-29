using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Grpc.Core;
using KafkaTopicVisualizer.Server.Consumers;
using KafkaTopicVisualizer.Server.Extensions;

namespace KafkaTopicVisualizer.Server.Streams;

public sealed class InsuranceVerification : InsuranceVerificationStream.InsuranceVerificationStreamBase
{
	public override async Task Consume(ConsumeRequest request, IServerStreamWriter<VerificationList> responseStream, ServerCallContext context)
	{
		var deserializer = new ProtobufDeserializer<Verification>();

		var config = new ConsumerConfig()
			.Build(request.ConsumerConfig);

		var consumer = new TopicConsumer<Verification>(config, request.Topic, deserializer);
		Task.Run(() => consumer.Listen(context.CancellationToken));

		while (!context.CancellationToken.IsCancellationRequested)
		{
			var reply = new VerificationList();

			reply.Verifications.AddRange(consumer.Messages);

			await responseStream.WriteAsync(reply);
			await Task.Delay(TimeSpan.FromSeconds(1));
		}
	}
}
