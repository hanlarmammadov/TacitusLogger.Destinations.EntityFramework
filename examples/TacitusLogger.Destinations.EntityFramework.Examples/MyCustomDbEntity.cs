using System; 

namespace TacitusLogger.Destinations.EntityFramework.Examples
{
    class MyCustomDbEntity
    {
        public string LogId { get; set; }
        public string Context { get; set; }
        public LogType LogType { get; set; }
        public string Description { get; set; }
        public DateTime LogDate { get; set; }
    }
}
