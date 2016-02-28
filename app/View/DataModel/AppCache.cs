using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.Infrastructure.EventArgs;
using WhatToCook.ViewModels;

namespace WhatToCook.DataModel
{
    public static class AppCache
    {
        private static CategoryListViewModel categories;
        private static List<RecipeViewModel> recipes;

        public static RecipeListViewModel Search(string text)
        {
            var matches = new RecipeListViewModel();
            foreach (var r in recipes)
            {
                // check Name and Description
                var isMatch = r.Name.Contains(text) || r.Description.Contains(text);
                if(!isMatch)
                {
                    r.LoadDetails();
                    // check Ingredients
                    foreach(var ingredient in r.Ingredients)
                    {
                        if(ingredient.Name.Contains(text) || ingredient.Unit.Contains(text))
                        {
                            isMatch = true;
                            break;
                        }
                    }

                    if(!isMatch)
                    {
                        // check Steps
                        foreach(var step in r.Steps)
                        {
                            if(step.Step.Contains(text))
                            {
                                isMatch = true;
                                break;
                            }
                        }
                    }
                }

                if(isMatch)
                {
                    matches.Add(r);
                }
            }

            return matches;
        }

        public static RecipeListViewModel Search(string text,string categoryId)
        {

            var category = categories.FirstOrDefault(c => c.Id.ToString().Equals(categoryId));

            var matches = new RecipeListViewModel();
            foreach (var r in category.RelatedRecipes)
            {
                // check Name and Description
                var isMatch = r.Name.Contains(text) || r.Description.Contains(text);
                if (!isMatch)
                {
                    r.LoadDetails();
                    // check Ingredients
                    foreach (var ingredient in r.Ingredients)
                    {
                        if (ingredient.Name.Contains(text) || ingredient.Unit.Contains(text))
                        {
                            isMatch = true;
                            break;
                        }
                    }

                    if (!isMatch)
                    {
                        // check Steps
                        foreach (var step in r.Steps)
                        {
                            if (step.Step.Contains(text))
                            {
                                isMatch = true;
                                break;
                            }
                        }
                    }
                }

                if (isMatch)
                {
                    matches.Add(r);
                }
            }

            return matches;
        }

        public static RecipeListViewModel GetFavourites()
        {
            var favourites = new RecipeListViewModel();
            foreach(var r in recipes)
            {
                if(r.IsFavourite)
                {
                    favourites.Add(r);
                }
            }
            return favourites;
        }

        public static CategoryListViewModel GetCategories()
        {
            if (categories == null)
            {
                categories = new CategoryListViewModel();
                recipes = new List<RecipeViewModel>();
                using (var context = new Model.Entities.RecipeDbContext())
                {
                    var recipeNodes = (from category in context.Categories
                                       from relation in context.RecipeCategoyRelations.Where(relation => relation.CategoryId == category.Id).DefaultIfEmpty()
                                       from recipe in context.Recipes.Where(recipe => recipe != null && relation != null && recipe.Id == relation.RecipeId).DefaultIfEmpty()
                                       select new
                                       {
                                           // category
                                           categoryId = category.Id,
                                           categoryName = category.Name,
                                           categoryDescription = category.Description,
                                           categoryIconPath = category.IconPath,
                                           categoryBgColor = category.BackgroundColor,
                                           // recipe
                                           recipeId = recipe != null ? (int?)recipe.Id : null,
                                           recipeName = recipe != null ? recipe.Name : null,
                                           recipeDescription = recipe != null ? recipe.Description : null,
                                           recipeImagePath = recipe != null ? recipe.ImagePath : null,
                                           recipeUnitPrice = recipe != null ? (int?)recipe.UnitPrice : null,
                                           recipePreparationTime = recipe != null ? (int?)recipe.PreparationTime : null,
                                           recipeDifficulty = recipe != null ? (int?)recipe.Difficulty : null,
                                           recipeStars = recipe != null ? (int?)recipe.Stars : null,
                                           recipeIsFavourite = recipe != null ? (bool?)recipe.IsFavourite : null
                                       }).ToList();

                    CategoryViewModel currentCategory = null;
                    foreach (var recipeNode in recipeNodes)
                    {
                        if (currentCategory == null || currentCategory.Id != recipeNode.categoryId)
                        {
                            currentCategory = new CategoryViewModel(new Model.Entities.Category
                            {
                                Id = recipeNode.categoryId,
                                Name = recipeNode.categoryName,
                                Description = recipeNode.categoryDescription,
                                IconPath = recipeNode.categoryIconPath,
                                BackgroundColor = recipeNode.categoryBgColor
                            });

                            categories.Add(currentCategory);
                        }

                        if (recipeNode.recipeId.HasValue)
                        {
                            var recipeViewModel = recipes.FirstOrDefault(r => r.Id == recipeNode.recipeId.Value);
                            // add to cache
                            if (recipeViewModel == null)
                            {
                                recipeViewModel = new RecipeViewModel()
                            {
                                Id = recipeNode.recipeId.Value,
                                Name = recipeNode.recipeName,
                                Description = recipeNode.recipeDescription,
                                ImagePath = recipeNode.recipeImagePath,
                                UnitPrice = recipeNode.recipeUnitPrice.Value,
                                PreparationTime = recipeNode.recipePreparationTime.Value,
                                Difficulty = recipeNode.recipeDifficulty.Value,
                                Stars = recipeNode.recipeStars.Value,
                                IsFavourite = recipeNode.recipeIsFavourite.Value
                            };
                                recipes.Add(recipeViewModel);
                                
                            }
                            // add to the current category anyway
                            currentCategory.RelatedRecipes.Add(recipeViewModel);
                        }
                    }
                }
            }
            return categories;
        }

        public static MenuListViewModel GetMenuList()
        {
            var menuList = new MenuListViewModel();
            using (var context = new Model.Entities.RecipeDbContext())
            {
                foreach (var menuItem in context.MenuItems)
                {
                    var menuItemViewModel = new MenuItemViewModel(menuItem);
                    menuItemViewModel.Recipe = recipes.First(r => r.Id == menuItem.RecipeId);
                    menuItemViewModel.Recipe.LoadDetails();
                    menuItemViewModel.Recipe.AddMenuItem(menuItem);
                    menuList.Add(menuItemViewModel);
                }
            }
            return menuList;
        }

    }


}
