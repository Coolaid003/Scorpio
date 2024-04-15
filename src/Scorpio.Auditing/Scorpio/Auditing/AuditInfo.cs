﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Scorpio.Auditing
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditInfo
    {

        /// <summary>
        /// The name of the application or service writing audit logs.
        /// Default: null.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImpersonatorUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ExecutionDuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<AuditActionInfo> Actions { get; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Exception> Exceptions { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> ExtraProperties { get; }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> Comments { get; }

        /// <summary>
        /// 
        /// </summary>
        public AuditInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
            Actions = new List<AuditActionInfo>();
            Exceptions = new List<Exception>();
            Comments = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TWapper"></typeparam>
        /// <returns></returns>
        public TWapper CreateWapper<TWapper>() where TWapper : AuditInfoWapper => Activator.CreateInstance(typeof(TWapper), this) as TWapper;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"AUDIT LOG:");
            sb.AppendLine($"- {"User",-20}: {CurrentUser} ");
            sb.AppendLine($"- {"ExecutionDuration",-20}: {ExecutionDuration}");

            if (ExtraProperties.Any())
            {
                foreach (var property in ExtraProperties)
                {
                    sb.AppendLine($"- {property.Key,-20}: {property.Value}");
                }
            }
            if (Actions.Any())
            {
                sb.AppendLine("- Actions:");
                foreach (var action in Actions)
                {
                    sb.AppendLine(action.ToString());
                }
            }

            if (Exceptions.Any())
            {
                sb.AppendLine("- Exceptions:");
                foreach (var exception in Exceptions)
                {
                    sb.AppendLine($"  - {exception.Message}");
                    sb.AppendLine($"    {exception}");
                }
            }
            return sb.ToString();
        }

    }
}
