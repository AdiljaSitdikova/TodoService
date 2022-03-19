﻿namespace TodoApi.Models
{
    #region snippet
    public class TodoItemDTO: IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
    #endregion
}
