﻿using System.ComponentModel.DataAnnotations;

namespace TheRaven.Shared.Entity;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; }
}
