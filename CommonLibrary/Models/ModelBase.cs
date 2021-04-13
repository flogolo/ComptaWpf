using System;

namespace CommonLibrary.Models
{
    public abstract class ModelBase : IModel
    {
        public virtual long Id { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}