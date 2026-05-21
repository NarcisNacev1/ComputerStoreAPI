using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Product> ProductsWithCategories()
        => _context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category);

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await ProductsWithCategories().AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await ProductsWithCategories().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetByNameAsync(string name)
        => await ProductsWithCategories()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        => await ProductsWithCategories()
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        var existingLinks = _context.ProductCategories.Where(pc => pc.ProductId == product.Id);
        _context.ProductCategories.RemoveRange(existingLinks);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Products.AnyAsync(p => p.Id == id);
}