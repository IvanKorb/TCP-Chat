namespace Server.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MessageContext : DbContext
    {
      
        public MessageContext()
            : base("name=MessageContext")
        {
        }

        public DbSet<Message> Messages { get; set; }

    }

  
}