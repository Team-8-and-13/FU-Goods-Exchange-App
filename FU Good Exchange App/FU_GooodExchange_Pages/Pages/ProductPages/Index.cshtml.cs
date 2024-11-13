using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FU_GooodExchange_Pages;
using FU_GooodExchange_Pages.Data;

namespace FU_GooodExchange_Pages.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly FU_GooodExchange_Pages.Data.AppDbContext _context;

        public IndexModel(FU_GooodExchange_Pages.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.User).ToListAsync();
        }
    }
}
