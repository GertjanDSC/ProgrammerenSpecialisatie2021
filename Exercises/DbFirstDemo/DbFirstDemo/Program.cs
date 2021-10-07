using DbFirstDemo.Models;
using System;

namespace DbFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DatabaseFirstDemoEntities();
            Post post = new() { Body = "In de les", DatePublished = DateTime.Now, Title = "Eerste keer" /*, PostId = 1*/ };
            context.Posts.Add(post);
            context.SaveChanges();
        }
    }
}
