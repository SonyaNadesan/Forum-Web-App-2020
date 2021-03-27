using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse<Category> Create(string nameInUrl, string displayName)
        {
            var response = new ServiceResponse<Category>();

            nameInUrl = UrlParamFriednlyGeneratorService.GetTextForParamUse(nameInUrl);
            displayName = UrlParamFriednlyGeneratorService.GetTextForParamUse(displayName);

            var category = _unitOfWork.CategoryRepository.GetAll().SingleOrDefault(c => c.NameInUrl == nameInUrl || c.DisplayName == displayName);

            if (category != null)
            {
                response.ErrorMessage = "Category already exists. Name In URL; " + nameInUrl + ", Display Name: " + displayName;
                return response;
            }

            var newCategory = new Category()
            {
                Id = Guid.NewGuid(),
                NameInUrl = nameInUrl,
                DisplayName = displayName,
                Threads = new List<Thread>()
            };

            response.Result = newCategory;

            try
            {
                _unitOfWork.CategoryRepository.Add(newCategory);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
                return response;
            }

            return response;
        }

        public ServiceResponse<Category> Delete(Category category)
        {
            var response = new ServiceResponse<Category>(category);

            var categoryFromDb = _unitOfWork.CategoryRepository.Get(category.Id);

            if (categoryFromDb == null)
            {
                response.ErrorMessage = "Category Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.CategoryRepository.Delete(category.Id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Category> Edit(Category category)
        {
            var response = new ServiceResponse<Category>(category);

            var categoryFromDb = _unitOfWork.CategoryRepository.Get(category.Id);

            if (categoryFromDb == null)
            {
                response.ErrorMessage = "Category Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.CategoryRepository.Edit(category);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Category> Get(Guid categoryId)
        {
            var response = new ServiceResponse<Category>();

            var category = _unitOfWork.CategoryRepository.Get(categoryId);

            response.Result = category;

            if (category == null)
            {
                response.ErrorMessage = "Category not found";
                return response;
            }

            return response;
        }

        public ServiceResponse<IEnumerable<Category>> GetAll()
        {
            var response = new ServiceResponse<IEnumerable<Category>>();

            var categories = _unitOfWork.CategoryRepository.GetAll().ToList();

            response.Result = categories;

            if (categories == null)
            {
                response.ErrorMessage = "Sorry,something went wrong.";
                return response;
            }

            return response;
        }
    }
}
