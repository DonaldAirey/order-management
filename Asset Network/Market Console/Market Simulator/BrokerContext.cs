namespace Teraque.AssetNetwork
{

    using System;
    using System.ServiceModel;
    using System.Threading;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;
    using Teraque.AssetNetwork.Market.Properties;
    using Teraque.AssetNetwork.WebService;

    /// <summary>
    /// Defines the properties of a simulated broker.
    /// </summary>
    /// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
    class BrokerContext : IDisposable
    {

        /// <summary>
        /// Orders are chunked up in lots this big before being sent to the market.
        /// </summary>
        const Int32 chunkSize = 4;

        /// <summary>
        /// The broker's name.
        /// </summary>
        String nameField;

        /// <summary>
        /// Random number generator.
        /// </summary>
        Random randomField = new Random();

        /// <summary>
        /// The amount of time to wait (in milliseconds) after a communication error before trying again.
        /// </summary>
        const Int32 retryTime = 1000;

        /// <summary>
        /// A map used to route order executions back to the order source.
        /// </summary>
        Dictionary<String, WebServiceClient> sourceRoutingMapField = new Dictionary<String, WebServiceClient>();

        /// <summary>
        /// The broker's symbol.
        /// </summary>
        String symbolField;

        /// <summary>
        /// Used to coordinate thread activity.
        /// </summary>
        Object syncRoot = new Object();

        /// <summary>
        /// Maps the name of the source to a data structure used to manage the connection to that source.
        /// </summary>
        Dictionary<String, SourceContext> sourceContextMap = new Dictionary<String, SourceContext>();

        /// <summary>
        /// Initializes a new instance of the BrokerContext class.
        /// </summary>
        public BrokerContext(BrokerInfo brokerInfo)
        {

            // Initialize the object.
            this.nameField = brokerInfo.Name;
            this.symbolField = brokerInfo.Symbol;

            // This document contains the configuration of the connections used to route orders back to the source of the order.
            XDocument xDocument = XDocument.Load(Environment.ExpandEnvironmentVariables(Settings.Default.SourceConfigurationFile));

            // This will load in the properties of each of sources into a dictionary that is used to manage the stream of messages back to the source that
            // originated the order.
            foreach (XElement xElement in xDocument.Root.Elements("Source"))
            {

                // Don't bother to connect to the source if they aren't active.
                Boolean isActive = Convert.ToBoolean(xElement.Attribute("IsActive").Value);
                if (isActive)
                {

                    // This provides an operating context for the connection back to the source of the order.
                    SourceContext sourceContext = new SourceContext();

                    // This is the name of the source.
                    String tenant = xElement.Attribute("Name").Value;

                    // This properties are used to connect us to a web service that will deliver the messages back to the source.
                    ClientInfo clientInfo = sourceContext.ClientInfo;
                    clientInfo.EndpointName = xElement.Attribute("Endpoint").Value;
                    clientInfo.Password = xElement.Attribute("Password").Value;
                    clientInfo.UserName = xElement.Attribute("UserName").Value;

                    // This thread will asynchronously pull messages out of the message queue and deliver them to the source.
                    Thread thread = new Thread(this.SourceThread);
                    thread.Start(sourceContext);
                    thread.Name = String.Format("{0} connection to {1}", brokerInfo.Symbol, tenant);
                    sourceContext.Thread = thread;

                    // This acts as a routing map to deliver messages back to the source of the orders based on the name of the source.
                    this.sourceContextMap.Add(tenant, sourceContext);

                }

            }

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

            // This will abort each of the communication threads back to the order sources.
            foreach (KeyValuePair<String, SourceContext> keyValuePair in this.sourceContextMap)
                keyValuePair.Value.Thread.Abort();

        }

        /// <summary>
        /// Sends the execution report back to the order source.
        /// </summary>
        /// <param name="tenant">The identifier of the originator of the order.</param>
        /// <param name="message">The execution report.</param>
        public void SendReport(String tenant, Teraque.Message message)
        {

            // Add this execution report to the queue of messages that this broker must return to the order source.  Note that the thread used to send the message 
            // will go to sleep if there are no more messages to be sent, so we need to wake it up if it's waiting.
            lock (this.syncRoot)
            {
                SourceContext sourceContext = this.sourceContextMap[tenant];
                Queue<Teraque.Message> messageQueue = sourceContext.MessageQueue;
                messageQueue.Enqueue(message);
                if (messageQueue.Count == 1)
                    sourceContext.OrderEvents.Set();
            }

        }

        /// <summary>
        /// Thread used to communicate with the source of the orders.
        /// </summary>
        /// <param name="state">The generic thread parameter.</param>
        public void SourceThread(Object state)
        {

            // This data structure provides the context for this thread.  It contains all the information needed to manage a connection to a tenant that has 
            // generated orders and is now waiting for execution reports.
            SourceContext sourceContext = state as SourceContext;

            // Construct a new web service for the source of the order so we can report our executions.
            ClientInfo clientInfo = sourceContext.ClientInfo;
            WebServiceClient webServiceClient = new WebServiceClient(clientInfo.EndpointName);
            webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
            webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;

            // This thread will continue to pull messages out of the queue
            while (true)
            {

                // The thread will wait here until there is an order to process.
                sourceContext.OrderEvents.WaitOne();

                List<Teraque.Message> messages = new List<Teraque.Message>();
                lock (this.syncRoot)
                {
                    Queue<Teraque.Message> messageQueue = sourceContext.MessageQueue;
                    while (messageQueue.Count != 0 && messages.Count < BrokerContext.chunkSize)
                        messages.Add(messageQueue.Dequeue());
                    if (messageQueue.Count == 0)
                        sourceContext.OrderEvents.Reset();
                }

                // This will guarantee that the execution report is returned to the order originator.  If it can't send the message, it will try to reconstruct the
                // web service and try again.
                while (true)
                {

                    try
                    {

                        // If the web service has been faulted (or timed out), then reopen it.  Note that we provide a healthy sleep time so that we don't
                        // constantly spin here when the simulator isn't available.
                        if (webServiceClient.State != CommunicationState.Opened)
                        {
                            webServiceClient = new WebServiceClient(clientInfo.EndpointName);
                            webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
                            webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
                        }

                        // If the message sent to the web service is successful, then we'll break out of the infinite loop.  Otherwise, we'll keep on trying to send
                        // this message until we're terminated externally.
                        webServiceClient.ReportExecution(messages.ToArray());
                        break;

                    }
                    catch
                    {

                        // If the web service has been faulted (or timed out), then reopen it.  Note that we provide a healthy sleep time so that we don't
                        // constantly spin here when the simulator isn't available.
                        if (webServiceClient.State != CommunicationState.Opened)
                        {
                            webServiceClient = new WebServiceClient(clientInfo.EndpointName);
                            webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
                            webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
                            Thread.Sleep(BrokerContext.retryTime);
                        }

                    }

                }

            }

        }

        /// <summary>
        /// The broker's name.
        /// </summary>
        public String Name
        {
            get
            {
                return this.nameField;
            }
        }

        /// <summary>
        /// Random number generator.
        /// </summary>
        public Random Random
        {
            get
            {
                return this.randomField;
            }
        }

        /// <summary>
        /// A map used to route order executions back to the source.
        /// </summary>
        public Dictionary<String, WebServiceClient> SourceRoutingMap
        {
            get
            {
                return this.sourceRoutingMapField;
            }
        }

        /// <summary>
        /// The broker's symbol.
        /// </summary>
        public String Symbol
        {
            get
            {
                return this.symbolField;
            }
        }

    }

}
