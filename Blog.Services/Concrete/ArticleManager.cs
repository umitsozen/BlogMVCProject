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

        public async Task<IDataResult<ArticleDto>> Add(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;
            var addarticle = await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveAsync();
            return new DataResult<ArticleDto>(ResultStatus.Success,
                new ArticleDto
                {
                    Article = addarticle,
                    ResultStatus = ResultStatus.Success,
                    message = $"{article.Title} başlıklı makale başarıyla eklenmiştir."
                }
                , $"{article.Title} başlıklı makale başarıyla eklenmiştir.");
        }

        public async Task<IResult> Delete(int articleId, string modifiedByName)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir makale bulunamadı.");
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

            return new DataResult<ArticleDto>(ResultStatus.Error,
                 new ArticleDto
                 {
                     Article = null,
                     ResultStatus = ResultStatus.Error,
                     message = "Böyle bir makale bulunamadı"
                 },
                  "Böyle bir makale bulunamadı");

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
            return new DataResult<ArticleListDto>(ResultStatus.Error,
                 new ArticleListDto
                 {
                     Articles = null,
                     ResultStatus = ResultStatus.Error,
                     message = "Makaleler bulunamadı"
                 },
                  "Makaleler bulunamadı");

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
                return new DataResult<ArticleListDto>(ResultStatus.Error, new ArticleListDto
                {
                    Articles = null,
                    ResultStatus = ResultStatus.Error,
                    message = "Makaleler bulunamadı"
                }, "Makaleler bulunamadı");
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, new ArticleListDto
            {
                Articles = null,
                ResultStatus = ResultStatus.Error,
                message = "Böyle bir kategori bulunamadı"
            }, "Böyle bir kategori bulunamadı");
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
            return new DataResult<ArticleListDto>(ResultStatus.Error, new ArticleListDto
            {
                Articles = null,
                ResultStatus = ResultStatus.Error,
                message = "Makaleler bulunamadı"
            }, "Makaleler bulunamadı");
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
            return new DataResult<ArticleListDto>(ResultStatus.Error, new ArticleListDto
            {
                Articles = null,
                ResultStatus = ResultStatus.Error,
                message = "Makaleler bulunamadı"
            }, "Makaleler bulunamadı");
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);

                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Böyle bir makale bulunamadı.");
        }

        public async Task<IDataResult<ArticleDto>> Update(ArticleyUpdateDto articleUpdateDto, string modifyiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifyiedByName;
            var updatedArticle = await _unitOfWork.Articles.UpdateAsync(article);
            await _unitOfWork.SaveAsync();
            return new DataResult<ArticleDto>(ResultStatus.Success,
                new ArticleDto
                {
                    Article = updatedArticle,
                    ResultStatus = ResultStatus.Success,
                    message = $"{article.Title} başlıklı makale başarıyla güncellenmiştir."
                },
                $"{article.Title} başlıklı makale başarıyla güncellenmiştir.");

        }
    }
}
