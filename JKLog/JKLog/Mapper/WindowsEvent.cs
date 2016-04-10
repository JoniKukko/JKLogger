﻿using System;
using JKLog.Interface;
using System.Diagnostics;
using JKLog.Configuration;
using System.Text;
using JKLog.Model;

namespace JKLog.Mapper
{
    public class WindowsEvent : IWritable
    {
        private static string staticSource = ConfigurationManager.GetValue(typeof(WindowsEvent), "source");



        public void WriteEntry(IEntry entry)
        {
            if (entry.Category != "JKLog" && (entry.Context as Type) != typeof(WindowsEvent))
            {
                try
                {
                    // if there is a match between entry type and windows type
                    EventLogEntryType type = EventLogEntryType.Information;
                    if (Enum.IsDefined(typeof(EventLogEntryType), (int)entry.Type))
                        type = (EventLogEntryType)entry.Type;

                    // context to byte array
                    byte[] toBytes = new byte[] { };
                    if (entry.Context != null)
                        toBytes = Encoding.ASCII.GetBytes(entry.Context.ToString());

                    EventLog.WriteEntry(staticSource, entry.Message, type, (int)entry.Type, (short)entry.Type, toBytes);
                }
                catch (Exception)
                {
                    JKLogger.StaticFailureAudit("Failed to open Windows Event source. Source is not registered, configured or there is an internal failure in WindowsEvent mapper.", typeof(WindowsEvent), "JKLog");
                }
            }
        }



        public static void RegisterSource()
        {
            try
            {
                if (staticSource == null)
                    JKLogger.StaticFailureAudit("Failed to open Windows Event source. Configuration not found.", typeof(WindowsEvent), "JKLog");

                else if (EventLog.SourceExists(staticSource))
                    JKLogger.StaticInformation("Windows Event source is already registered.", null, "JKLog");

                else
                {
                    EventLog.CreateEventSource(staticSource, staticSource);
                    JKLogger.StaticSuccessAudit("Windows Event source is created. Restart the application to allow it to be registered.", typeof(WindowsEvent), "JKLog");
                    return;
                }
            }
            catch (Exception)
            {
                JKLogger.StaticFailureAudit("Failed to create Windows Event source. Call RegisterSource() while running as Administrator.", typeof(WindowsEvent), "JKLog");
            }
        }
    }
}