using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Activation.Strategies;
using NLog;

namespace TA.SnapCap.Server
    {
    class LoggedActivationStrategy : ActivationStrategy
        {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public override void Activate(Ninject.Activation.IContext context, Ninject.Activation.InstanceReference reference)
            {
            if (reference.Instance is ILogger)
                {
                //_logger = (ILogger)reference.Instance;
                }
            _logger.Debug("Ninject Activate: " + reference.Instance.GetType());
            base.Activate(context, reference);
            }

        public override void Deactivate(Ninject.Activation.IContext context, Ninject.Activation.InstanceReference reference)
            {
            _logger.Debug("Ninject DeActivate: " + reference.Instance.GetType());
            base.Deactivate(context, reference);
            }
        }
    }
