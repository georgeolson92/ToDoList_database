using System.Collections.Generic;
using System;
using Nancy;
using ToDoList.Objects;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
      };
      Get["/tasks"] = _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };
      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/tasks/new"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["task_form.cshtml", AllCategories];
      };
      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["task-description"], Request.Form["category-id"], Request.Form["due-date"]);
        newTask.Save();
        return View["success.cshtml"];
      };
      Post["/tasks/delete"] = _ => {
       Task.DeleteAll();
       return View["cleared.cshtml"];
     };
     Post["/categories/delete"] = _ => {
      Category.DeleteAll();
      return View["cleared.cshtml"];
    };
     Get["/categories/{id}"] = parameters => {
       Dictionary<string, object> model = new Dictionary<string, object>();
       var SelectedCategory = Category.Find(parameters.id);
       var CategoryTasks = SelectedCategory.GetTasks();
       model.Add("category", SelectedCategory);
       model.Add("tasks", CategoryTasks);
       return View["category.cshtml", model];
     };
    }
  }
}
