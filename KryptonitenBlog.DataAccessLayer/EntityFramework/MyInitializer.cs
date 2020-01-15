using KryptonitenBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KryptonitenBlog.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //Adding Admin 
            BlogUser admin = new BlogUser()
            {
                Name = "Ulaş",
                Surname = "Şentürk",
                Email = "ulascansenturk@hotmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "kryptoniten",
                ProfileImageName = "user.png",
                Password = "toor",
                CreateadOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifierUserName = "kryptoniten"
            };
            //Adding Standart user 
            BlogUser StandartUser = new BlogUser()
            {
                Name = "test",
                Surname = "test2",
                Email = "test@test@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "test",
                ProfileImageName = "user.png",
                Password = "test",
                CreateadOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(30),
                ModifierUserName = "kryptoniten"
            };

            context.BlogUsers.Add(admin);
            context.BlogUsers.Add(StandartUser);

            for (int i = 0; i < 8; i++)
            {
                BlogUser user = new BlogUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageName = "user.png",
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreateadOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now), 
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifierUserName = $"user{i}"
                };
                context.BlogUsers.Add(user);


            }


            context.SaveChanges();
            //user list for using..
            List<BlogUser> userlist = context.BlogUsers.ToList();

            //Adding Some Fake Categories
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreateadOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifierUserName = "kryptoniten"
                };
                context.Categories.Add(cat);    
                //adding notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(1, 10); k++)
                {
                    BlogUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1,9),
                        Owner = owner,
                        CreateadOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifierUserName = owner.Username,
                    };
                    cat.Notes.Add(note);

                    //Adding FAke Comments
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 7); j++)
                    {
                        BlogUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            CreateadOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifierUserName = comment_owner.Username,
                            Owner = comment_owner
                        };
                        note.Comments.Add(comment);
                    }
                    //adding fake likes
                    for (int l = 0; l <note.LikeCount; l++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[l]
                            
,
                        };
                        note.Likes.Add(liked);


                    }
                }
            }

            context.SaveChanges();

        }

    }
}