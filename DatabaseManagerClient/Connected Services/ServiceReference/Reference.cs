﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManagerClient.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IAuthentication")]
    public interface IAuthentication {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Registration", ReplyAction="http://tempuri.org/IAuthentication/RegistrationResponse")]
        bool Registration(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Registration", ReplyAction="http://tempuri.org/IAuthentication/RegistrationResponse")]
        System.Threading.Tasks.Task<bool> RegistrationAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Login", ReplyAction="http://tempuri.org/IAuthentication/LoginResponse")]
        string Login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Login", ReplyAction="http://tempuri.org/IAuthentication/LoginResponse")]
        System.Threading.Tasks.Task<string> LoginAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Logout", ReplyAction="http://tempuri.org/IAuthentication/LogoutResponse")]
        bool Logout(string token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthentication/Logout", ReplyAction="http://tempuri.org/IAuthentication/LogoutResponse")]
        System.Threading.Tasks.Task<bool> LogoutAsync(string token);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAuthenticationChannel : DatabaseManagerClient.ServiceReference.IAuthentication, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthenticationClient : System.ServiceModel.ClientBase<DatabaseManagerClient.ServiceReference.IAuthentication>, DatabaseManagerClient.ServiceReference.IAuthentication {
        
        public AuthenticationClient() {
        }
        
        public AuthenticationClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthenticationClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Registration(string username, string password) {
            return base.Channel.Registration(username, password);
        }
        
        public System.Threading.Tasks.Task<bool> RegistrationAsync(string username, string password) {
            return base.Channel.RegistrationAsync(username, password);
        }
        
        public string Login(string username, string password) {
            return base.Channel.Login(username, password);
        }
        
        public System.Threading.Tasks.Task<string> LoginAsync(string username, string password) {
            return base.Channel.LoginAsync(username, password);
        }
        
        public bool Logout(string token) {
            return base.Channel.Logout(token);
        }
        
        public System.Threading.Tasks.Task<bool> LogoutAsync(string token) {
            return base.Channel.LogoutAsync(token);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IDatabaseManagerService")]
    public interface IDatabaseManagerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddDigitalInputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddDigitalInputTagResponse")]
        bool AddDigitalInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddDigitalInputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddDigitalInputTagResponse")]
        System.Threading.Tasks.Task<bool> AddDigitalInputTagAsync(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddDigitalOutputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddDigitalOutputTagResponse")]
        bool AddDigitalOutputTag(string token, string name, string description, string ioAddress, double initValue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddDigitalOutputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddDigitalOutputTagResponse")]
        System.Threading.Tasks.Task<bool> AddDigitalOutputTagAsync(string token, string name, string description, string ioAddress, double initValue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAnalogInputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAnalogInputTagResponse")]
        bool AddAnalogInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAnalogInputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAnalogInputTagResponse")]
        System.Threading.Tasks.Task<bool> AddAnalogInputTagAsync(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAnalogOutputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAnalogOutputTagResponse")]
        bool AddAnalogOutputTag(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAnalogOutputTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAnalogOutputTagResponse")]
        System.Threading.Tasks.Task<bool> AddAnalogOutputTagAsync(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAlarm", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAlarmResponse")]
        bool AddAlarm(string token, string name, string type, int priority, double limit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/AddAlarm", ReplyAction="http://tempuri.org/IDatabaseManagerService/AddAlarmResponse")]
        System.Threading.Tasks.Task<bool> AddAlarmAsync(string token, string name, string type, int priority, double limit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/RemoveAlarm", ReplyAction="http://tempuri.org/IDatabaseManagerService/RemoveAlarmResponse")]
        bool RemoveAlarm(string token, int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/RemoveAlarm", ReplyAction="http://tempuri.org/IDatabaseManagerService/RemoveAlarmResponse")]
        System.Threading.Tasks.Task<bool> RemoveAlarmAsync(string token, int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/PrintAlarmsForTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/PrintAlarmsForTagResponse")]
        string PrintAlarmsForTag(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/PrintAlarmsForTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/PrintAlarmsForTagResponse")]
        System.Threading.Tasks.Task<string> PrintAlarmsForTagAsync(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/RemoveTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/RemoveTagResponse")]
        bool RemoveTag(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/RemoveTag", ReplyAction="http://tempuri.org/IDatabaseManagerService/RemoveTagResponse")]
        System.Threading.Tasks.Task<bool> RemoveTagAsync(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/ChangeOutputValue", ReplyAction="http://tempuri.org/IDatabaseManagerService/ChangeOutputValueResponse")]
        bool ChangeOutputValue(string token, string tagName, double value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/ChangeOutputValue", ReplyAction="http://tempuri.org/IDatabaseManagerService/ChangeOutputValueResponse")]
        System.Threading.Tasks.Task<bool> ChangeOutputValueAsync(string token, string tagName, double value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/GetOutputValue", ReplyAction="http://tempuri.org/IDatabaseManagerService/GetOutputValueResponse")]
        double GetOutputValue(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/GetOutputValue", ReplyAction="http://tempuri.org/IDatabaseManagerService/GetOutputValueResponse")]
        System.Threading.Tasks.Task<double> GetOutputValueAsync(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/TurnScanOn", ReplyAction="http://tempuri.org/IDatabaseManagerService/TurnScanOnResponse")]
        bool TurnScanOn(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/TurnScanOn", ReplyAction="http://tempuri.org/IDatabaseManagerService/TurnScanOnResponse")]
        System.Threading.Tasks.Task<bool> TurnScanOnAsync(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/TurnScanOff", ReplyAction="http://tempuri.org/IDatabaseManagerService/TurnScanOffResponse")]
        bool TurnScanOff(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/TurnScanOff", ReplyAction="http://tempuri.org/IDatabaseManagerService/TurnScanOffResponse")]
        System.Threading.Tasks.Task<bool> TurnScanOffAsync(string token, string tagName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/GetStringForPrintingTags", ReplyAction="http://tempuri.org/IDatabaseManagerService/GetStringForPrintingTagsResponse")]
        string GetStringForPrintingTags(string token, string ioType, string adType, bool value, bool scan);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDatabaseManagerService/GetStringForPrintingTags", ReplyAction="http://tempuri.org/IDatabaseManagerService/GetStringForPrintingTagsResponse")]
        System.Threading.Tasks.Task<string> GetStringForPrintingTagsAsync(string token, string ioType, string adType, bool value, bool scan);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDatabaseManagerServiceChannel : DatabaseManagerClient.ServiceReference.IDatabaseManagerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DatabaseManagerServiceClient : System.ServiceModel.ClientBase<DatabaseManagerClient.ServiceReference.IDatabaseManagerService>, DatabaseManagerClient.ServiceReference.IDatabaseManagerService {
        
        public DatabaseManagerServiceClient() {
        }
        
        public DatabaseManagerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DatabaseManagerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DatabaseManagerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DatabaseManagerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool AddDigitalInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff) {
            return base.Channel.AddDigitalInputTag(token, name, description, driver, ioAddress, scanTime, scanOnOff);
        }
        
        public System.Threading.Tasks.Task<bool> AddDigitalInputTagAsync(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff) {
            return base.Channel.AddDigitalInputTagAsync(token, name, description, driver, ioAddress, scanTime, scanOnOff);
        }
        
        public bool AddDigitalOutputTag(string token, string name, string description, string ioAddress, double initValue) {
            return base.Channel.AddDigitalOutputTag(token, name, description, ioAddress, initValue);
        }
        
        public System.Threading.Tasks.Task<bool> AddDigitalOutputTagAsync(string token, string name, string description, string ioAddress, double initValue) {
            return base.Channel.AddDigitalOutputTagAsync(token, name, description, ioAddress, initValue);
        }
        
        public bool AddAnalogInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units) {
            return base.Channel.AddAnalogInputTag(token, name, description, driver, ioAddress, scanTime, scanOnOff, lowLimit, highLimit, units);
        }
        
        public System.Threading.Tasks.Task<bool> AddAnalogInputTagAsync(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units) {
            return base.Channel.AddAnalogInputTagAsync(token, name, description, driver, ioAddress, scanTime, scanOnOff, lowLimit, highLimit, units);
        }
        
        public bool AddAnalogOutputTag(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units) {
            return base.Channel.AddAnalogOutputTag(token, name, description, ioAddress, initValue, lowLimit, highLimit, units);
        }
        
        public System.Threading.Tasks.Task<bool> AddAnalogOutputTagAsync(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units) {
            return base.Channel.AddAnalogOutputTagAsync(token, name, description, ioAddress, initValue, lowLimit, highLimit, units);
        }
        
        public bool AddAlarm(string token, string name, string type, int priority, double limit) {
            return base.Channel.AddAlarm(token, name, type, priority, limit);
        }
        
        public System.Threading.Tasks.Task<bool> AddAlarmAsync(string token, string name, string type, int priority, double limit) {
            return base.Channel.AddAlarmAsync(token, name, type, priority, limit);
        }
        
        public bool RemoveAlarm(string token, int id) {
            return base.Channel.RemoveAlarm(token, id);
        }
        
        public System.Threading.Tasks.Task<bool> RemoveAlarmAsync(string token, int id) {
            return base.Channel.RemoveAlarmAsync(token, id);
        }
        
        public string PrintAlarmsForTag(string token, string tagName) {
            return base.Channel.PrintAlarmsForTag(token, tagName);
        }
        
        public System.Threading.Tasks.Task<string> PrintAlarmsForTagAsync(string token, string tagName) {
            return base.Channel.PrintAlarmsForTagAsync(token, tagName);
        }
        
        public bool RemoveTag(string token, string tagName) {
            return base.Channel.RemoveTag(token, tagName);
        }
        
        public System.Threading.Tasks.Task<bool> RemoveTagAsync(string token, string tagName) {
            return base.Channel.RemoveTagAsync(token, tagName);
        }
        
        public bool ChangeOutputValue(string token, string tagName, double value) {
            return base.Channel.ChangeOutputValue(token, tagName, value);
        }
        
        public System.Threading.Tasks.Task<bool> ChangeOutputValueAsync(string token, string tagName, double value) {
            return base.Channel.ChangeOutputValueAsync(token, tagName, value);
        }
        
        public double GetOutputValue(string token, string tagName) {
            return base.Channel.GetOutputValue(token, tagName);
        }
        
        public System.Threading.Tasks.Task<double> GetOutputValueAsync(string token, string tagName) {
            return base.Channel.GetOutputValueAsync(token, tagName);
        }
        
        public bool TurnScanOn(string token, string tagName) {
            return base.Channel.TurnScanOn(token, tagName);
        }
        
        public System.Threading.Tasks.Task<bool> TurnScanOnAsync(string token, string tagName) {
            return base.Channel.TurnScanOnAsync(token, tagName);
        }
        
        public bool TurnScanOff(string token, string tagName) {
            return base.Channel.TurnScanOff(token, tagName);
        }
        
        public System.Threading.Tasks.Task<bool> TurnScanOffAsync(string token, string tagName) {
            return base.Channel.TurnScanOffAsync(token, tagName);
        }
        
        public string GetStringForPrintingTags(string token, string ioType, string adType, bool value, bool scan) {
            return base.Channel.GetStringForPrintingTags(token, ioType, adType, value, scan);
        }
        
        public System.Threading.Tasks.Task<string> GetStringForPrintingTagsAsync(string token, string ioType, string adType, bool value, bool scan) {
            return base.Channel.GetStringForPrintingTagsAsync(token, ioType, adType, value, scan);
        }
    }
}
