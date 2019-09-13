using NServiceBus;
using NServiceBus.UniformSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Mvc
{
    public class ReusedComponent
    {
        IUniformSession session;

        public ReusedComponent(IUniformSession session)
        {
            this.session = session;
        }

        public async Task SendCommand(ICommand command)
        {
            await session.Send(command).ConfigureAwait(false);
        }
    }
}
