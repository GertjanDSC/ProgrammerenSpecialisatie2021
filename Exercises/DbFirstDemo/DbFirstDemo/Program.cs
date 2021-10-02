using DbFirstDemo.Models;
using System;

namespace DbFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DatabaseFirstDemoEntities();
            Post post = new() { Body = "body", DatePublished = DateTime.Now, Title = "title" /*, PostId = 1*/ };
            context.Posts.Add(post);
            context.SaveChanges();
        }
    }
}
