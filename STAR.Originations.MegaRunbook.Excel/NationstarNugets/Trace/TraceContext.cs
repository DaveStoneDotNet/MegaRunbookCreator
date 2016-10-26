// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.Wcf.TraceContext
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using System;
using System.Threading;

namespace STAR.Enterprise.Trace.Wcf
{
    public class TraceContext
    {
        private static TraceContext _instance;

        public static TraceContext Current
        {
            get
            {
                return LazyInitializer.EnsureInitialized<TraceContext>(ref TraceContext._instance, (Func<TraceContext>)(() => new TraceContext()));
            }
        }

        private TraceContext()
        {
        }

        public void Capture(string key, object value)
        {
            CorrelationState current = CorrelationState.Current;
            if (current == null)
                return;
            string key1 = key;
            object obj = value;
            current.AddCustomData(key1, obj);
        }

        public void Capture(string key, Func<object> selector)
        {
            CorrelationState current = CorrelationState.Current;
            if (current == null)
                return;
            string key1 = key;
            Func<object> selector1 = selector;
            current.AddCustomData(key1, selector1);
        }

        public void AddTag(string tag, string value)
        {
            CorrelationState current = CorrelationState.Current;
            if (current == null)
                return;
            string tag1 = tag;
            string str = value;
            current.AddTag(tag1, str);
        }
    }
}
