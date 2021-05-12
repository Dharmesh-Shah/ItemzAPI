// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

# nullable enable

namespace ItemzApp.API.Entities
{
    public class ItemzType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid(); // If Database Guid is not provided as ID then a new one will be created by default.

        [Required]
        [MaxLength(128)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Status { get; set; } = "Active";

        [MaxLength(1028)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(128)]
        public string CreatedBy { get; set; } = "Some User";

        [Required]
        public DateTimeOffset CreatedDate { get; set; } = DateTime.Now;

        public List<ItemzTypeJoinItemz>? ItemzTypeJoinItemz { get; set; }

        public Project? Project { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public bool IsSystem { get; set; } = false;

    }
}

# nullable disable
