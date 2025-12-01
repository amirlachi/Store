using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Store.Application.Interfaces.Contexts;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Queries.GetProductForSite;
using Store.Application.Services.Products.Queries.GetAllCategories;
using Store.Application.Services.Products.Queries.GetCategories;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.Services.Products.Queries.GetProductDetailForSite;

namespace Store.Application.Services.Products.FacadPattern
{
    public class ProductFacad : IProductFacad
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public ProductFacad(IDataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;
        }

        private AddNewCategoryService _addNewCategory;
        public AddNewCategoryService AddNewCategoryService
        {
            get
            {
                return _addNewCategory = _addNewCategory ?? new AddNewCategoryService(_context);
            }
        }

        private IGetCategoriesService _getCategories;
        public IGetCategoriesService GetCategoriesService
        {
            get
            {
                return _getCategories = _getCategories ?? new GetCategoriesService(_context);
            }
        }

        private AddNewProductService _addNewProduct;
        public AddNewProductService AddNewProductService
        {
            get
            {
                return _addNewProduct = _addNewProduct ?? new AddNewProductService(_context, _environment);
            }
        }

        private IGetAllCategoriesService _getAllCategories;
        public IGetAllCategoriesService GetAllCategoriesService
        {
            get
            {
                return _getAllCategories = _getAllCategories ?? new GetAllCategoriesService(_context);
            }
        }

        private IGetProductForAdminService _getProductForAdmin;
        public IGetProductForAdminService GetProductForAdminService
        {
            get
            {
                return _getProductForAdmin = _getProductForAdmin ?? new GetProductForAdminService(_context);
            }
        }

        private IGetProductDetailForAdminService _getProductDetailForAdmin;
        public IGetProductDetailForAdminService GetProductDetailForAdminService
        {
            get
            {
                return _getProductDetailForAdmin = _getProductDetailForAdmin ?? new GetProductDetailForAdminService(_context);
            }
        }

        private IGetProductForSiteService _getProductForSite;
        public IGetProductForSiteService GetProductForSiteService
        {
            get
            {
                return _getProductForSite = _getProductForSite ?? new GetProductForSiteService(_context);
            }
        }

        private IGetProductDetailForSiteService _getProductDetailForSite;
        public IGetProductDetailForSiteService GetProductDetailForSiteService
        {
            get
            {
                return _getProductDetailForSite = _getProductDetailForSite ?? new GetProductDetailForSiteService(_context);
            }
        }
    }
}
