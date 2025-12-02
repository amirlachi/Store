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
        ResultDto<ResultProductForSiteDto> Execute(Ordering ordering,string SearchKey,long? CategoryId,int Page, int PageSize);
    }

    public enum Ordering
    {
        NotOrder = 0,
        /// <summary>
        /// پربازدیدترین
        /// </summary>
        MostVisited = 1,
        /// <summary>
        /// پرفروش ترین
        /// </summary>
        BestSelling = 2,
        /// <summary>
        /// محبوب ترین
        /// </summary>
        MostPopular = 3,
        /// <summary>
        /// جدیدترین
        /// </summary>
        TheNewest = 4,
        /// <summary>
        /// ارزان ترین
        /// </summary>
        Cheapest = 5,
        /// <summary>
        /// گران ترین
        /// </summary>
        TheMostExpensive = 6,
    }

    public class GetProductForSiteService : IGetProductForSiteService
    {
        private readonly IDataBaseContext _context;

        public GetProductForSiteService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultProductForSiteDto> Execute(Ordering ordering, string SearchKey, long? CategoryId, int Page, int PageSize)
        {
            int totalRow = 0;
            var productQuery = _context.Products
                .Include(p => p.ProductImages).AsQueryable();

            if (CategoryId != null)
            {
                productQuery = productQuery.Where(p => p.CategoryId == CategoryId || p.Category.ParentCategoryId == CategoryId).AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(SearchKey))
            {
                productQuery = productQuery.Where(p => p.Name.Contains(SearchKey) || p.Brand.Contains(SearchKey));
            }

            switch (ordering)
            {
                case Ordering.NotOrder:
                    productQuery = productQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;

                case Ordering.MostVisited:
                    productQuery = productQuery.OrderByDescending(p => p.ViewCount).AsQueryable();
                    break;

                case Ordering.BestSelling:
                    break;

                case Ordering.MostPopular:
                    break;

                case Ordering.TheNewest:
                    productQuery = productQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;

                case Ordering.Cheapest:
                    productQuery = productQuery.OrderBy(p => p.Price).AsQueryable();
                    break;

                case Ordering.TheMostExpensive:
                    productQuery = productQuery.OrderByDescending(p => p.Price).AsQueryable();
                    break;

                default:
                    break;
            }

            var product = productQuery.ToPaged(Page, PageSize, out totalRow);

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
