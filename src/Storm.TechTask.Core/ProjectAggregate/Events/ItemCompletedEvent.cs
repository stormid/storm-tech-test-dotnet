﻿
using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Core.ProjectAggregate.Events
{
    public class ItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem ItemCompleted { get; set; }
        public Project Project { get; set; }

        public ItemCompletedEvent(Project project,
            ToDoItem itemCompleted)
        {
            Project = project;
            ItemCompleted = itemCompleted;
        }
    }
}
