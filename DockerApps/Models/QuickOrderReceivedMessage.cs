using System;

namespace DockerApps.Models
{
    public class QuickOrderReceivedMessage
    {
        public QuickOrder Order { get; set; }
        public int CustomerId { get; set; }
        public Guid OrderId { get; set; }
    }
}
