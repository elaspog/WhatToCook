using Microsoft.Data.SQLite;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace WhatToCook.DataModel
{
    public class DbInitializer
    {
        public static async void InitDatabaseIfNeeded()
        {
            using (var context = new RecipeDbContext())
            {
                var currentId = 500;
                // hack ==> init, if there aren't any categories :-)
                if (!context.Categories.Any())
                {
                    // load json
                    var dataUri = new Uri("ms-appx:///DataModel/DataBaseInit.json");
                    var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
                    var data = await FileIO.ReadTextAsync(file);
                    var jsonObject = JsonObject.Parse(data);

                    // add categories
                    foreach (var cValue in jsonObject["Categories"].GetArray())
                    {
                        var cObject = cValue.GetObject();
                        context.Categories.Add(new Category
                        {
                            Id = Convert.ToInt32(cObject["Id"].GetNumber()),
                            Name = cObject["Name"].GetString(),
                            IconPath = "ms-appx:///Assets/Categories/" + cObject["Icon"].GetString(),
                            Description = cObject["Description"].GetString(),
                            BackgroundColor = cObject["BackgroundColor"].GetString()
                        });
                    }

                    // add ingredients
                    foreach (var cValue in jsonObject["Ingredients"].GetArray())
                    {
                        var cObject = cValue.GetObject();
                        context.Ingredients.Add(new Ingredient
                        {
                            Id = Convert.ToInt32(cObject["Id"].GetNumber()),
                            Name = cObject["Name"].GetString()
                        });
                    }

                    // add recipes
                    var recipes = jsonObject["Recipes"].GetArray();
                    var index = 0;
                    foreach (var cValue in recipes)
                    {
                        var cObject = cValue.GetObject();
                        var recipeId = currentId++;
                        context.Recipes.Add(new Recipe
                        {
                            Id = recipeId,
                            Name = cObject["Name"].GetString(),
                            Description = cObject["Description"].GetString(),
                            ImagePath = "ms-appx:///Assets/Recipes/" + cObject["Image"].GetString(),
                            UnitPrice = Convert.ToInt32(cObject["UnitPrice"].GetNumber()),
                            PreparationTime = Convert.ToInt32(cObject["PreparationTime"].GetNumber()),
                            Difficulty = Convert.ToInt32(cObject["Difficulty"].GetNumber()),
                            Stars = 0,
                            IsFavourite = false
                        });

                        // add category relations
                        foreach (var categoryId in cObject["CategoryIds"].GetArray())
                        {
                            context.RecipeCategoyRelations.Add(new RecipeCategoryRelation
                            {
                                Id = currentId++,
                                CategoryId = Convert.ToInt32(categoryId.GetNumber()),
                                RecipeId = recipeId
                            });
                        }

                        // add ingredient relations
                        index = 0;
                        foreach (var ingredient in cObject["Ingredients"].GetArray())
                        {
                            var obj = ingredient.GetObject();
                            context.RecipeIngredientRelations.Add(new RecipeIngredientRelation
                            {
                                Id = currentId++,
                                IngredientId = Convert.ToInt32(obj["Id"].GetNumber()),
                                RecipeId = recipeId,
                                Index = index++,
                                Quantity = obj["Quantity"].GetNumber(),
                                Unit = obj["Unit"].GetString()
                            });
                        }

                        // add steps
                        index = 0;
                        foreach (var step in cObject["Steps"].GetArray())
                        {
                            context.RecipeSteps.Add(new RecipeStep
                            {
                                Id = currentId++,
                                Step = step.GetString(),
                                RecipeId = recipeId,
                                Index = index++
                            });
                        }
                    }

                    context.SaveChanges();

                    var relations = context.RecipeIngredientRelations.ToList();
                }
            }
        }
    }
}
