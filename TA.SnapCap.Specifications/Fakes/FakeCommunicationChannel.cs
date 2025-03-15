// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Reactive.Linq;
using System.Text;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Fakes
    {
    /// <summary>
    ///     A fake communication channel that logs any sent data in <see cref="SendLog" /> and receives a
    ///     fake pre-programmed response passed into the constructor. The class also keeps a count of how
    ///     many times each method of <see cref="ICommunicationChannel" /> was called.
    /// </summary>
    public class FakeCommunicationChannel : ICommunicationChannel
        {
        private readonly IObservable<char> receivedCharacters;
        private readonly StringBuilder sendLog;

        /// <summary>
        ///     Dependency injection constructor. Initializes a new instance of the
        ///     <see cref="SafetyMonitorDriver" /> class.
        /// </summary>
        /// <param name="fakeResponse">Implementation of the injected dependency.</param>
        public FakeCommunicationChannel(string fakeResponse)
            {
            Endpoint = new InvalidEndpoint();
            Response = fakeResponse;
            receivedCharacters = fakeResponse.ToCharArray().ToObservable();
            ObservableReceivedCharacters = receivedCharacters.Concat(Observable.Never<char>());
            sendLog = new StringBuilder();
            IsOpen = true;
            }

        /// <summary>Gets the send log.</summary>
        /// <value>The send log.</value>
        public string SendLog => sendLog.ToString();

        /// <summary>Gets a copy of the fake pre-programmed response.</summary>
        /// <value>The response.</value>
        public string Response { get; }

        public int TimesDisposed { get; set; }

        public int TimesClosed { get; set; }

        public int TimesOpened { get; set; }

        public void Dispose()
            {
            TimesDisposed++;
            }

        public void Open()
            {
            TimesOpened++;
            }

        public void Close()
            {
            TimesClosed++;
            }

        public void Send(string txData)
            {
            sendLog.Append(txData);
            }

        public IObservable<char> ObservableReceivedCharacters { get; }

        public bool IsOpen { get; set; }

        public DeviceEndpoint Endpoint { get; }
        }
    }