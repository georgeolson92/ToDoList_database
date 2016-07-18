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
        Task newTask = new Task(Request.Form["task-description"], Request.Form["due-date"]);
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
        Category SelectedCategory = Category.Find(parameters.id);
        List<Task> CategoryTasks = SelectedCategory.GetTasks();
        List<Task> AllTasks = Task.GetAll();
        model.Add("category", SelectedCategory);
        model.Add("categoryTasks", CategoryTasks);
        model.Add("allTasks", AllTasks);
        return View["category.cshtml", model];
      };
      Get["tasks/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Task SelectedTask = Task.Find(parameters.id);
        List<Category> TaskCategories = SelectedTask.GetCategories();
        List<Category> AllCategories = Category.GetAll();
        model.Add("task", SelectedTask);
        model.Add("taskCategories", TaskCategories);
        model.Add("allCategories", AllCategories);
        return View["task.cshtml", model];
      };
      Post["task/add_category"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        task.AddCategory(category);
        return View["success.cshtml"];
      };
      Post["category/add_task"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };
      Get["/task/edit/{id}"] = parameters => {
        Task SelectedTask = Task.Find(parameters.id);
        return View["task_edit.cshtml", SelectedTask];
      };
      Patch["/task/edit/{id}"] = parameters => {
        Task SelectedTask = Task.Find(parameters.id);
        SelectedTask.Update(Request.Form["new-task-description"], Request.Form["new-due-date"], Request.Form["new-task-completed"]);
        Console.WriteLine(Request.Form["new-due-date"]);
        return View["success.cshtml"];
      };
      Get["/task/delete/{id}"] = parameters => {
        Task SelectedTask = Task.Find(parameters.id);
        return View["task_delete.cshtml", SelectedTask];
      };
      Delete["task/delete/{id}"] = parameters => {
        Task SelectedTask = Task.Find(parameters.id);
        SelectedTask.Delete();
        return View["success.cshtml"];
      };
      Get["/category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        return View["category_delete.cshtml", SelectedCategory];
      };
      Delete["category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        SelectedCategory.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
