using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductForSite
{
    public interface IGetProductForSiteService
    {
        ResultDto<ResultProductForSiteDto> Execute(long? CategoryId,int Page);
    }

    public class GetProductForSiteService : IGetProductForSiteService
    {
        private readonly IDataBaseContext _context;

        public GetProductForSiteService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultProductForSiteDto> Execute(long? CategoryId, int Page)
        {
            int totalRow = 0;
            var productQuery = _context.Products
                .Include(p => p.ProductImages).AsQueryable();

            if (CategoryId != null)
            {
                productQuery = productQuery.Where(p => p.CategoryId == CategoryId).AsQueryable();
            }

            var product = productQuery.ToPaged(Page, 5, out totalRow);

            Random random = new Random();
            return new ResultDto<ResultProductForSiteDto>
            {
                Data = new ResultProductForSiteDto
                {
                    TotalRow = totalRow,
                    Products = product.Select(p => new ProductForSiteDto
                    {
                        Id = p.Id,
                        Star = random.Next(1, 5),
                        Title = p.Name,
                        ImageSrc = p.ProductImages.FirstOrDefault().Src,
                        Price = p.Price,
                    }).ToList(),
                },
                IsSuccess = true,
            };
        }
    }

    public class ResultProductForSiteDto
    {
        public List<ProductForSiteDto> Products { get; set; }
        public int TotalRow { get; set; }
    }

    public class ProductForSiteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImageSrc { get; set; }
        public int Star { get; set; }
    }
}
