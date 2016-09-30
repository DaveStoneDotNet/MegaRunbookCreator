// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.Wcf.CorrelationState
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace STAR.Enterprise.Trace.Wcf
{
    internal class CorrelationState
    {
        private const string CorrelationStateIncomingPropertiesKey = "STAR.Enterprise.Trace.Wcf.CorrelationState";
        private IDictionary<string, Func<object>> _customData;
        private IDictionary<string, string> _tags;

        public Stopwatch Stopwatch { get; set; }

        public string OperationName { get; set; }

        public object[] Inputs { get; set; }

        public static CorrelationState Current
        {
            get
            {
                CorrelationState correlationState = (CorrelationState)null;
                OperationContext current = OperationContext.Current;
                if ((current != null ? current.IncomingMessageProperties : (MessageProperties)null) != null)
                    correlationState = current.IncomingMessageProperties["STAR.Enterprise.Trace.Wcf.CorrelationState"] as CorrelationState;
                return correlationState;
            }
            internal set
            {
                OperationContext current = OperationContext.Current;
                if (current == null)
                    return;
                current.IncomingMessageProperties.Add("STAR.Enterprise.Trace.Wcf.CorrelationState", (object)value);
            }
        }

        public void AddCustomData(string key, object value)
        {
            this.AddCustomData(key, (Func<object>)(() => value));
        }

        public void AddCustomData(string key, Func<object> selector)
        {
            if (string.IsNullOrWhiteSpace(key) || selector == null)
                return;
            LazyInitializer.EnsureInitialized<IDictionary<string, Func<object>>>(ref this._customData, (Func<IDictionary<string, Func<object>>>)(() => (IDictionary<string, Func<object>>)new Dictionary<string, Func<object>>()));
            this._customData[key] = selector;
        }

        public void AddTag(string tag, string value)
        {
            LazyInitializer.EnsureInitialized<IDictionary<string, string>>(ref this._tags, (Func<IDictionary<string, string>>)(() => (IDictionary<string, string>)new Dictionary<string, string>()));
            this._tags[tag] = value;
        }

        public string GetTagText()
        {
            if (this._tags == null)
                return (string)null;
            return string.Join(" ", this._tags.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(kv => string.Format("{0}: '{1}'.", (object)kv.Key, (object)kv.Value))));
        }

        public string TagMessage(string message)
        {
            string tagText = this.GetTagText();
            if (!string.IsNullOrEmpty(tagText))
                return message + ".  " + tagText;
            return message;
        }

        public Dictionary<string, object> GetTraceData(string contextDetails = null, object returnValue = null)
        {
            object[] inputs = this.Inputs;
            Dictionary<string, object> dictionary1;
            if (inputs == null || !((IEnumerable<object>)inputs).Any<object>())
            {
                dictionary1 = new Dictionary<string, object>();
            }
            else
            {
                IEnumerable<KeyValuePair<string, object>> source = ((IEnumerable<object>)inputs).Select<object, KeyValuePair<string, object>>((Func<object, int, KeyValuePair<string, object>>)((o, idx) => new KeyValuePair<string, object>(string.Format("{0}: {1}", (object)(idx + 1), (object)((o != null ? o.GetType().Name : (string)null) ?? "Empty")), o)));
                Func<KeyValuePair<string, object>, string> func = (Func<KeyValuePair<string, object>, string>)(x => x.Key);
                Func<KeyValuePair<string, object>, string> keySelector = null;
                dictionary1 = source.ToDictionary<KeyValuePair<string, object>, string, object>(keySelector, (Func<KeyValuePair<string, object>, object>)(x => x.Value));
            }
            Dictionary<string, object> dictionary2 = dictionary1;
            if (returnValue != null)
                dictionary2["Response"] = returnValue;
            if (this._customData != null)
            {
                foreach (KeyValuePair<string, Func<object>> keyValuePair in (IEnumerable<KeyValuePair<string, Func<object>>>)this._customData)
                {
                    try
                    {
                        dictionary2[keyValuePair.Key] = keyValuePair.Value();
                    }
                    catch
                    {
                    }
                }
            }
            if (!string.IsNullOrEmpty(contextDetails))
                dictionary2["Context"] = (object)contextDetails;
            return dictionary2;
        }
    }
}
