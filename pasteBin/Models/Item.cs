using System;

namespace pasteBin.Models
{
    public class Item
    {
        public Item()
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now;
        }
        public string Id { get; set; }

        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string Ip { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

    }
}