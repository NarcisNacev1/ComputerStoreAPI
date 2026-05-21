using AutoMapper;
using ComputerStore.Application.DTOs.Category;
using ComputerStore.Application.DTOs.Product;
using ComputerStore.Domain.Entities;

namespace ComputerStore.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryResponseDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src =>
                    src.ProductCategories.Select(pc => pc.Category.Name).ToList()));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
    }
}