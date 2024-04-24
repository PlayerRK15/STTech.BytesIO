﻿using System;

namespace STTech.BytesIO.Core
{
    /// <summary>
    /// 回复(数据收发)匹配委托
    /// </summary>
    /// <typeparam name="TSend">发送数据类型</typeparam>
    /// <typeparam name="TRecv">接收数据类型</typeparam>
    /// <param name="send">发送数据</param>
    /// <param name="recv">接收数据</param>
    /// <returns></returns>
    public delegate bool ReplyMatchHandler<TSend, TRecv>(TSend send, TRecv recv);

    /// <summary>
    /// 单次发送数据的远端响应
    /// </summary>
    public class BaseReply
    {
        /// <summary>
        /// 触发客户端
        /// </summary>
        public IBytesClient Client { get; }

        /// <summary>
        /// 响应状态
        /// </summary>
        public ReplyStatus Status { get; }

        /// <summary>
        /// 内部错误
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// 响应数据
        /// </summary>
        protected object Value { get; }

        /// <summary>
        /// 构造失败的响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="status"></param>
        /// <param name="exception">异常信息</param>
        protected BaseReply(IBytesClient client, ReplyStatus status, Exception exception)
        {
            Client = client;
            Status = status;
            Exception = exception;
        }

        /// <summary>
        /// 构造成功的响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        protected BaseReply(IBytesClient client, object value)
        {
            Client = client;
            Status = ReplyStatus.Completed;
            Value = value;
        }
    }



    /// <summary>
    /// 单次发送数据的远端响应
    /// </summary>
    /// <typeparam name="T">响应数据的类型</typeparam>
    public class BaseReply<T> : BaseReply
    {
        /// <summary>
        /// 响应结果
        /// </summary>
        protected T Data => (T)Value;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected BaseReply(IBytesClient client, ReplyStatus status, Exception exception) : base(client, status, exception) { }

        /// <summary>
        /// 构造成功的响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        protected BaseReply(IBytesClient client, T data) : base(client, data) { }

        protected T GetData() => (T)Value;
    }

    /// <summary>
    /// 单次发送数据的远端响应
    /// </summary>
    public class Reply : BaseReply<Response>
    {
        protected Reply(IBytesClient client, ReplyStatus status, Exception exception) : base(client, status, exception) { }
        protected Reply(IBytesClient client, Response data) : base(client, data) { }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <returns></returns>
        public Response GetResponse() => GetData();
    }

    /// <summary>
    /// 单次发送数据的远端响应
    /// </summary>
    /// <typeparam name="T">响应数据的类型</typeparam>
    public class Reply<T> : Reply where T : Response
    {
        public Reply(IBytesClient client, ReplyStatus status, Exception exception) : base(client, status, exception) { }
        public Reply(IBytesClient client, T data) : base(client, data) { }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <returns></returns>
        public new T GetResponse() => (T)base.GetResponse();
    }

    /// <summary>
    /// 单次发送数据的远端响应（字节数组）
    /// </summary>
    public class ReplyBytes : BaseReply<MemoryBlock>
    {
        public ReplyBytes(IBytesClient client, ReplyStatus status, Exception exception) : base(client, status, exception) { }
        public ReplyBytes(IBytesClient client, MemoryBlock data) : base(client, data) { }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <returns></returns>
        public ArraySegment<byte> GetBytes() => GetData().Segment;
    }


    /// <summary>
    /// 响应状态
    /// </summary>
    public enum ReplyStatus
    {
        /// <summary>
        /// 完成
        /// </summary>
        Completed,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 被迫中断
        /// </summary>
        Interrupted,
    }
}
