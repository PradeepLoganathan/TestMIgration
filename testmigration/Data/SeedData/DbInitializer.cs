using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using testmigration.Models;
namespace testmigration.Data.SeedData
{
    public class DbInitializer
    {
        public async static void InitializeAync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            // check if any users exist.
            if (context.Users.Any())
            {
                return;   // exit method, Database has been seeded
            }

            string[] roleNames = { "candidate", "administrator", "corporate" };
            // loop through roleNames Array
            foreach (var roleName in roleNames)
            {
                bool isExist = !String.IsNullOrEmpty(roleName);
                ApplicationRole applicationRole = await roleManager.FindByNameAsync(roleName);
                if (applicationRole == null)
                {
                    ApplicationRole applicationNewRole = new ApplicationRole
                    {
                        CreatedDate = DateTime.UtcNow
                    };
                    string rn = roleName.ToString().Trim();
                    applicationNewRole.Name = rn;
                    applicationNewRole.Description = rn;
                    applicationNewRole.IPAddress = "";
                    IdentityResult roleRuslt = await roleManager.CreateAsync(applicationNewRole);
                }

            }

            //create an array of users 
            //string adminRoleId = string.Empty;
            //string candidateRoleId = string.Empty;
            //string corporateRoleId = string.Empty;
            //foreach (var roleName in roleNames)
            //{
            //    ApplicationRole applicationRole1 = await roleManager.FindByNameAsync(roleName);
            //    if (applicationRole1 != null)
            //    {
            //        switch (roleName)
            //        {
            //            case "candidate":
            //                candidateRoleId = applicationRole1.Id;
            //                break;
            //            case "administrator":
            //                adminRoleId = applicationRole1.Id;
            //                break;
            //            case "corporate":
            //                corporateRoleId = applicationRole1.Id;
            //                break;
            //        }

            //    }
            //}

            List<ApplicationUser> canusers = new List<ApplicationUser>();


            canusers.Add(
                new ApplicationUser
                {

                    UserName = "candidate1@bootminds.com",
                    Email = "candidate1@bootminds.com",

                });

            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate2@bootminds.com",
                    Email = "candidate2@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate3@bootminds.com",
                    Email = "candidate3@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate4@bootminds.com",
                    Email = "candidate4@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate5@bootminds.com",
                    Email = "candidate5@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate6@bootminds.com",
                    Email = "candidate6@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate7@bootminds.com",
                    Email = "candidate7@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate8@bootminds.com",
                    Email = "candidate8@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate9@bootminds.com",
                    Email = "candidate9@bootminds.com",
                });
            canusers.Add(new ApplicationUser
                {

                    UserName = "candidate10@bootminds.com",
                    Email = "candidate10@bootminds.com",
                });

            //var corpusers =new  
                List<ApplicationUser> corpusers = new List<ApplicationUser> ();
            corpusers.Add(new ApplicationUser
            {

                UserName = "corporate1@bootminds.com",
                Email = "corporate1@bootminds.com",

            });

           

            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate2@bootminds.com",
                    Email = "corporate2@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate3@bootminds.com",
                    Email = "corporate3@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate4@bootminds.com",
                    Email = "corporate4@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate5@bootminds.com",
                    Email = "corporate5@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate6@bootminds.com",
                    Email = "corporate6@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate7@bootminds.com",
                    Email = "corporate7@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate8@bootminds.com",
                    Email = "corporate8@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate9@bootminds.com",
                    Email = "corporate9@bootminds.com",
                });
            corpusers.Add(new ApplicationUser
                {

                    UserName = "corporate10@bootminds.com",
                    Email = "corporate10@bootminds.com",
                });


            //loop through candidate users array
            foreach (ApplicationUser _user in canusers)
            {
                // create user
                var result = await userManager.CreateAsync(_user, "Subtest@123");
                if (result.Succeeded)
                {
                    //add user to "Member" role
                    await userManager.AddToRoleAsync(_user, "candidate");
                }
            }
            //loop through corporate users array
            foreach (ApplicationUser _user in corpusers)
            {
                // create user
                var result =  await userManager.CreateAsync(_user, "Subtest@123");
                if (result.Succeeded)
                {
                    //add user to "Member" role
                    await userManager.AddToRoleAsync(_user, "corporate");
                }
            }

            // adminuser
            var adminUser = new ApplicationUser
            {

                UserName = "administrator1@bootminds.com",
                Email = "administrator1@bootminds.com",
            };
            // create user

            var user = await userManager.CreateAsync(adminUser, "Subtest@123");
            if (user.Succeeded)
            {
                //add user to "Member" role
                await userManager.AddToRoleAsync(adminUser, "administrator");
            }
        }
    }
}
