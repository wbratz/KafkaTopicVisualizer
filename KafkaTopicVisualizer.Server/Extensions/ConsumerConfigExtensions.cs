using Confluent.Kafka;

namespace KafkaTopicVisualizer.Server.Extensions;

public static class ConsumerConfigExtensions
{
	public static void ParseEnums(this ConsumerConfig config, ConsumeConfig requestConfig)
	{
		if (Enum.TryParse<SecurityProtocol>(requestConfig.SecurityProtocol, out var securityProtocol))
		{
			config.SecurityProtocol = securityProtocol;
		}

		if (Enum.TryParse<AutoOffsetReset>(requestConfig.AutoOffsetReset, out var autoOffsetReset))
		{
			config.AutoOffsetReset = autoOffsetReset;
		}

		if (Enum.TryParse<SaslMechanism>(requestConfig.SaslMechanism, out var saslMechanism))
		{
			config.SaslMechanism = saslMechanism;
		}
	}

	public static ConsumerConfig Build(this ConsumerConfig config, ConsumeConfig requestConfig)
	{
		config = new ConsumerConfig
		{
			BootstrapServers = requestConfig.BootstrapServers,
			GroupId = requestConfig.GroupId,
			SaslUsername = requestConfig.SaslUserName,
			SaslPassword = requestConfig.SaslPassword
		};

		config.ParseEnums(requestConfig);

		return config;
	}
}
