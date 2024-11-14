using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FU_GooodExchange_Pages;
using FU_GooodExchange_Pages.Data;

namespace FU_GoodExchange_Pages.Pages.ProductPages
{
    public class DetailsModel : PageModel
    {
        private readonly FU_GooodExchange_Pages.Data.AppDbContext _context;

        public DetailsModel(FU_GooodExchange_Pages.Data.AppDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }
    }
}
