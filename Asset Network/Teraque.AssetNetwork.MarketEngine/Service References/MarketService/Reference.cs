﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Teraque.AssetNetwork.MarketService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SimulatorParameters", Namespace="http://schemas.datacontract.org/2004/07/Teraque.AssetNetwork")]
    [System.SerializableAttribute()]
    public partial class SimulatorParameters : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double EquityFrequencyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsExchangeSimulatorRunningField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsPriceSimulatorRunningField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double EquityFrequency {
            get {
                return this.EquityFrequencyField;
            }
            set {
                if ((this.EquityFrequencyField.Equals(value) != true)) {
                    this.EquityFrequencyField = value;
                    this.RaisePropertyChanged("EquityFrequency");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsExchangeSimulatorRunning {
            get {
                return this.IsExchangeSimulatorRunningField;
            }
            set {
                if ((this.IsExchangeSimulatorRunningField.Equals(value) != true)) {
                    this.IsExchangeSimulatorRunningField = value;
                    this.RaisePropertyChanged("IsExchangeSimulatorRunning");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsPriceSimulatorRunning {
            get {
                return this.IsPriceSimulatorRunningField;
            }
            set {
                if ((this.IsPriceSimulatorRunningField.Equals(value) != true)) {
                    this.IsPriceSimulatorRunningField = value;
                    this.RaisePropertyChanged("IsPriceSimulatorRunning");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MarketService.IMarketService")]
    public interface IMarketService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMarketService/ExecuteOrder", ReplyAction="http://tempuri.org/IMarketService/ExecuteOrderResponse")]
        void ExecuteOrder(Teraque.Message[] messages);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMarketService/GetSimulatorParameters", ReplyAction="http://tempuri.org/IMarketService/GetSimulatorParametersResponse")]
        Teraque.AssetNetwork.MarketService.SimulatorParameters GetSimulatorParameters();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMarketService/SetSimulatorParameters", ReplyAction="http://tempuri.org/IMarketService/SetSimulatorParametersResponse")]
        void SetSimulatorParameters(Teraque.AssetNetwork.MarketService.SimulatorParameters simulationParameters);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMarketServiceChannel : Teraque.AssetNetwork.MarketService.IMarketService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MarketServiceClient : System.ServiceModel.ClientBase<Teraque.AssetNetwork.MarketService.IMarketService>, Teraque.AssetNetwork.MarketService.IMarketService {
        
        public MarketServiceClient() {
        }
        
        public MarketServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MarketServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MarketServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MarketServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void ExecuteOrder(Teraque.Message[] messages) {
            base.Channel.ExecuteOrder(messages);
        }
        
        public Teraque.AssetNetwork.MarketService.SimulatorParameters GetSimulatorParameters() {
            return base.Channel.GetSimulatorParameters();
        }
        
        public void SetSimulatorParameters(Teraque.AssetNetwork.MarketService.SimulatorParameters simulationParameters) {
            base.Channel.SetSimulatorParameters(simulationParameters);
        }
    }
}
