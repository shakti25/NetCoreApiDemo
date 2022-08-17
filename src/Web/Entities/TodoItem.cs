﻿using System.ComponentModel.DataAnnotations;

namespace RToora.DemoApi.Web.Entities;
public class TodoItem
{
    public long Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
}
