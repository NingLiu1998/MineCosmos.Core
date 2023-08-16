using RabbitMQ.Client;
using System;

namespace MineCosmos.Core.EventBus
{
    /// <summary>
    /// RabbitMQ持久连接
    /// 接口
    /// </summary>
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
