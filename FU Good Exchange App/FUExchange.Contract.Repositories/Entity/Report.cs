﻿using FUExchange.Core.Base;
using FUExchange.Repositories.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Report : BaseEntity
    {
        public Guid? UserId { get; set; } = Guid.Empty;
        [Required]
        public string Reason { get; set; } = string.Empty;
        public bool Status { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
