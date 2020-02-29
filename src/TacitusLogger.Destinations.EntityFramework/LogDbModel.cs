using System;

namespace TacitusLogger.Destinations.EntityFramework
{
    public class LogDbModel
    {
        public string LogId { get; set; }
        public string Context { get; set; }
        public string Source { get; set; }
        public LogType LogType { get; set; }
        public string Description { get; set; }
        public string TagsJson { get; set; }
        public string LogItemsJson { get; set; }
        public DateTime LogDate { get; set; }
    }
}
