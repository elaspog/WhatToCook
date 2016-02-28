using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Model.Entities
{
    public class RecipeDbContext : DbContext
    {
        private const string DATABASE_FILENAME = "RecipeApp.db";
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients{ get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<RecipeCategoryRelation> RecipeCategoyRelations { get; set; }
        public DbSet<RecipeIngredientRelation> RecipeIngredientRelations { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        

        public RecipeDbContext() : base()
        {
            // Migrations are not yet enabled in EF7, so use an
            // API to create the database if it doesn't exist
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptions builder)
        {
            //C:\Users\Péter\AppData\Local\Packages\WhatToCook_jy76rtqp63c6p\LocalState
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            builder.UseSQLite("Filename=" + System.IO.Path.Combine(folder, DATABASE_FILENAME));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // configure category
            var categoryEntity = builder.Entity<Category>();

            // The EF7 SQLite provider currently doesn't support generated values
            // so setting the keys to be generated from developer code
            categoryEntity
                .Property(c => c.Id)
                .GenerateValueOnAdd(false);

            categoryEntity
                .Property(c => c.Name)
                .MaxLength(100)
                .Required(true);

            categoryEntity
                .Property(c => c.Description)
                .MaxLength(500);

            categoryEntity
                .Property(c => c.IconPath)
                .MaxLength(100);

            // configure ingredients
            var ingredientEntity = builder.Entity<Ingredient>();

            ingredientEntity
                .Property(r => r.Id)
                .GenerateValueOnAdd(false);

            ingredientEntity
                .Property(r => r.Name)
                .MaxLength(100);

            // configure recipes
            var recipeEntity = builder.Entity<Recipe>();

            recipeEntity
                .Property(r => r.Id)
                .GenerateValueOnAdd(false);

            recipeEntity
                .Property(r => r.Name)
                .MaxLength(100);

            // configure recipe step
            var stepEntity = builder.Entity<RecipeStep>();

            stepEntity
                .Property(ir => ir.Id)
                .GenerateValueOnAdd(false);

            // configure recipe category relations
            builder.Entity<RecipeCategoryRelation>()
                .Property(rc => rc.Id)
                .GenerateValueOnAdd(false);

            // configure recipe ingredient relations
            builder.Entity<RecipeIngredientRelation>()
                .Property(rc => rc.Id)
                .GenerateValueOnAdd(false);

            // configure meta
            builder.Entity<Meta>()
                .Property(m => m.Id)
                .GenerateValueOnAdd(false);
        }

        /// <summary>
        /// This is kind of a hack, because SQLite does not support auto increment id.
        /// </summary>
        /// <returns></returns>
        public int GenerateNewId ()
        {
            var currentIdMeta = (from meta in this.Metas
                                 where meta.Name == Meta.CURRENT_ID
                                 select meta).FirstOrDefault();

            if(currentIdMeta == null)
            {
                // start indexing from 1000 (hack, should find the current biggest id)
                currentIdMeta = new Meta {Id = 1, Name = Meta.CURRENT_ID, Value = "1000" };
                this.Metas.Add(currentIdMeta);
            }

            var nextId = Int32.Parse(currentIdMeta.Value) + 1;
            currentIdMeta.Value = nextId.ToString();
            this.SaveChanges();
            
            return nextId;
        }
    }
}
