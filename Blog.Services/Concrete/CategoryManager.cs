using AutoMapper;
using Blog.Data.Abstract;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using Blog.Services.Abstract;
using Blog.Shared.Utilities.Results.Abstract;
using Blog.Shared.Utilities.Results.ComplexType;
using Blog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
             if (categoryAddDto != null)
            {
                var category = _mapper.Map<Category>(categoryAddDto);
                category.CreatedByName = createdByName;
                category.ModifiedByName = createdByName;
                category.CreatedDate = DateTime.Now;
                category.ModifiedDate = DateTime.Now;
                await _unitOfWork.Categories.AddAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla eklendi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı.");
        }

        public async Task<IResult> Delete(int categoryId, string modifiedByName)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                await _unitOfWork.Categories.UpdateAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı.");

        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
            if (category != null)
                return new DataResult<CategoryDto>(ResultStatus.Error, null, "Karakter bulunamadı");

            return new DataResult<CategoryDto>(ResultStatus.Error, null, "Kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count > -1)
                return new DataResult<CategoryListDto>(ResultStatus.Success,
                    new CategoryListDto
                    {
                        Categories = categories,
                        ResultStatus = ResultStatus.Success
                    });
            return new DataResult<CategoryListDto>(ResultStatus.Error, null, "Hiç bir kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
            if (categories.Count > -1)
                return new DataResult<CategoryListDto>(ResultStatus.Success,
                    new CategoryListDto
                    {
                        Categories = categories,
                        ResultStatus = ResultStatus.Success
                    });
            return new DataResult<CategoryListDto>(ResultStatus.Error, null, "Hiç bir kategori bulunamadı.");
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                await _unitOfWork.Categories.DeleteAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla veritabanında silindi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı.");
        }

        public async Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifyiedByName)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);
            category.ModifiedByName = modifyiedByName;
            await _unitOfWork.Categories.UpdateAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
            return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla güncellenmiştir.");
        }
    }
}
