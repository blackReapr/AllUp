﻿using AllUp.Data;
using AllUp.Models;
using AllUp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AllUp.Controllers;

public class CartController : Controller
{
    private readonly AllUpDbContext _context;

    public CartController(AllUpDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddCartAsync(int? id)
    {
        if (id == null) return BadRequest();
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);
        if (product == null) return BadRequest();
        List<CartVM> cart;
        string basket = HttpContext.Request.Cookies["cart"];
        if (basket.IsNullOrEmpty())
        {
            cart = new();
        }
        else
        {
            cart = JsonSerializer.Deserialize<List<CartVM>>(basket);
            if (cart.Exists(p => p.Id == id))
            {
                cart.Find(p => p.Id == id).Count++;
            }
            else
            {
                cart.Add(new() { Id = product.Id, Count = 1, ExTax = product.ExTax, Image = product.MainImage, Name = product.Name, Price = product.Price });
            }
        }
        HttpContext.Response.Cookies.Append("cart", JsonSerializer.Serialize(cart));

        return PartialView("_CartPartial", cart);
    }
}
