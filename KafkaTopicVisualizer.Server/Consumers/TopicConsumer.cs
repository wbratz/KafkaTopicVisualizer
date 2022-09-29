using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;

namespace KafkaTopicVisualizer.Server.Consumers;

public sealed class TopicConsumer<T>
{
	private readonly ConsumerConfig _config;
	private readonly string _topic;
	private readonly List<T> _messages;
	private readonly IAsyncDeserializer<T> _deserializer;

	public TopicConsumer(ConsumerConfig config, string topic, IAsyncDeserializer<T> deserializer)
	{
		_config = config;
		_topic = topic;
		_deserializer = deserializer;
		_messages = new List<T>();
	}

	public List<T> Messages => _messages;

	public async Task Listen(CancellationToken cancellationToken)
	{

		using var consumer = new ConsumerBuilder<string, T>(_config)
			.SetValueDeserializer(_deserializer.AsSyncOverAsync())
			.Build();

		consumer.Subscribe(_topic);

		try
		{
			while (true)
			{
				var cr = consumer.Consume(cancellationToken);
				_messages.Add(cr.Message.Value);
			}
		}
		catch (OperationCanceledException)
		{
			//noop
		}
		catch (Exception ex)
		{
			var msg = ex.Message;
		}
		finally
		{
			consumer.Close();
		}
	}
}
