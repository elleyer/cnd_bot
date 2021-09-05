using System;
using System.ComponentModel.DataAnnotations;

namespace CndBot.Core
{
    public class EventDataModel
    {
        [Key]
        public virtual int Id { get; set; }

        public string EventName { get; set; }
        public string EventDescription { get; set; }
        
        public DateTime EventDate { get; set; }

        public long PublisherUserId { get; set; }
    }
}