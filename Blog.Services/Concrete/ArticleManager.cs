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
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;
            await _unitOfWork.Articles.AddAsync(article).ContinueWith(t => _unitOfWork.SaveAsync());
            return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla eklenmiştir.");
        }

        public async Task<IResult> Delete(int articleId, string modifiedByName)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                 await   _unitOfWork.Articles.UpdateAsync(article).ContinueWith(t=>_unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silindi.");
            }
            return new Result(ResultStatus.Error,"Böyle bir makale bulunamadı.");
        }

        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);
            if (article != null)
                return new DataResult<ArticleDto>(ResultStatus.Success,
                    new ArticleDto
                    {
                        Article = article,
                        ResultStatus = ResultStatus.Success
                    });

            return new DataResult<ArticleDto>(ResultStatus.Error, null, "Böyle bir makale bulunamadı");

        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
            if (articles != null && articles.Count > -1)
                return new DataResult<ArticleListDto>(ResultStatus.Success,
                    new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");

        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var categoryResult = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (categoryResult)
            {
                var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive && a.CategoryId == categoryId, a => a.User, a => a.Category);
                if (articles != null && articles.Count > -1)
                    return new DataResult<ArticleListDto>(ResultStatus.Success,
                        new ArticleListDto
                        {
                            Articles = articles,
                            ResultStatus = ResultStatus.Success
                        });
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Böyle bir kategori bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeleted()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted, a => a.User, a => a.Category);
            if (articles != null && articles.Count > -1)
                return new DataResult<ArticleListDto>(ResultStatus.Success,
                    new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
            if (articles != null && articles.Count > -1)
                return new DataResult<ArticleListDto>(ResultStatus.Success,
                    new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
               
                await _unitOfWork.Articles.DeleteAsync(article).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir makale bulunamadı.");
        }

        public async Task<IResult> Update(ArticleyUpdateDto articleUpdateDto, string modifyiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifyiedByName; 
            await _unitOfWork.Articles.UpdateAsync(article).ContinueWith(t => _unitOfWork.SaveAsync());
            return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla güncellenmiştir.");

        }
    }
}
