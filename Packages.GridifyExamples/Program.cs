using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer("Server=.;Database=TestDb;User ID=sa;Password=sasa@123;TrustServerCertificate=True;"));

var app = builder.Build();

app.MapGet("/", () => "Hello World.");

//app.MapGet("/blog", async ([AsParameters] GridifyQuery query, AppDbContext db) =>
//{
//    return Results.Ok(await db.Blogs.GridifyAsync(query));
//});

app.MapGet("/blog/{pageNo}/{pageSize}", async (int pageNo, int pageSize, AppDbContext db) =>
{
    var query = new GridifyQuery()
    {
        Page = pageNo,
        PageSize = pageSize,
    };
    return Results.Ok(await db.Blogs.GridifyAsync(query));
});

app.MapGet("/blog-generate", async (AppDbContext db) =>
{
    //for (int i = 0; i < 100; i++)
    //{
    //    await db.Blogs.AddAsync(new BlogDataModel()
    //    {
    //        Blog_Title = (i + 1) + "title",
    //        Blog_Author = (i + 1) + "author",
    //        Blog_Content = (i + 1) + "content",
    //    });
    //}
    await db.Blogs.AddRangeAsync(Enumerable.Range(1, 10).Select(x =>
        new BlogDataModel()
        {
            BlogTitle = x + "title",
            BlogAuthor = x + "author",
            BlogContent = x + "content",
        }
    ));
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
