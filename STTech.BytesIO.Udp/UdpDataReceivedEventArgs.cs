﻿using STTech.BytesIO.Core;
using System.Net.Sockets;

namespace STTech.BytesIO.Udp
{
    /// <summary>
    /// UDP数据接收事件参数
    /// </summary>
    public class UdpDataReceivedEventArgs : DataReceivedEventArgs
    {
        /// <summary>
        /// 接收到的字节数组
        /// </summary>
        public override MemoryBlock Data => new MemoryBlock(ReceiveResult.Buffer);

        /// <summary>
        /// UDP接收结果
        /// </summary>
        public UdpReceiveResult ReceiveResult { get; }

        public UdpDataReceivedEventArgs(UdpReceiveResult receiveResult)
        {
            ReceiveResult = receiveResult;
        }
    }
}
