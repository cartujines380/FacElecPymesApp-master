using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Sipecom.FactElec.Pymes.Agentes.Soap.Base
{
    public class MessageLoggingInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var buffer = reply.CreateBufferedCopy(int.MaxValue);
            reply = buffer.CreateMessage();

            var copy = buffer.CreateMessage();

            var fullInputMessage = copy.ToString();
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var buffer = request.CreateBufferedCopy(int.MaxValue);

            request = buffer.CreateMessage();

            var copy = buffer.CreateMessage();

            var fullOutputMessage = copy.ToString();

            return null;
        }
    }
}
