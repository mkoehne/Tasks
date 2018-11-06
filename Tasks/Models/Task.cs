using System;
using Realms;

namespace Tasks.Models
{
    public class Task : RealmObject
    {
        public Task()
        {
        }

        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool Done { get; set; }
    }
}
