// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Activation.Strategies;
using NLog;

namespace TA.SnapCap.Server
    {
    internal class LoggedActivationStrategy : ActivationStrategy
        {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public override void Activate(Ninject.Activation.IContext context,
            Ninject.Activation.InstanceReference reference)
            {
            if (reference.Instance is ILogger)
                {
                //_logger = (ILogger)reference.Instance;
                }
            _logger.Debug("Ninject Activate: " + reference.Instance.GetType());
            base.Activate(context, reference);
            }

        public override void Deactivate(Ninject.Activation.IContext context,
            Ninject.Activation.InstanceReference reference)
            {
            _logger.Debug("Ninject DeActivate: " + reference.Instance.GetType());
            base.Deactivate(context, reference);
            }
        }
    }