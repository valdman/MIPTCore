using System;
using Common;
using Common.Infrastructure;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using UserManagment;
using Xunit;
using Xunit.Sdk;

namespace IntegrationTestProject
{
    public class RepositoryAndMappingTestsForUser
    {
        [Fact]
        public async void CreatingAndThanDeletingUserCreateAndThanDeleteUser()
        {
            //Act
            var id = await _genericRepository.CreateAsync(alumniUser);
            var createdUser = await _genericRepository.GetById(id);
            alumniUser.Id = id;

            await _genericRepository.DeleteAsync(id);
            var deletedUser = await _genericRepository.GetById(id);
            
            //Assert
            Assert.Equal(alumniUser, createdUser);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async void CreatingAndThanUpdatingUserCreateAndThanUpdateUser()
        {
            //Act
            var id = await _genericRepository.CreateAsync(notAlumniUser);

            notAlumniUser.IsMiptAlumni = true;
            notAlumniUser.AlumniProfile = alumniUser.AlumniProfile;

            await _genericRepository.UpdateAsync(notAlumniUser);
            var updatedUser = await _genericRepository.GetById(id);
            
            await _genericRepository.DeleteAsync(id);
            
            //Assert
            Assert.Equal(updatedUser, notAlumniUser);
            Assert.NotNull(updatedUser.AlumniProfile);
        }

        public RepositoryAndMappingTestsForUser()
        {
            
            var contextOptions = new DbContextOptionsBuilder()
                .UseNpgsql(ConnectionString)
                .Options;
            var sessionProvider = new DbSessionProvider(contextOptions);
            
            _genericRepository = new GenericRepository<User>(sessionProvider);
            
            sessionProvider.Database.EnsureCreated();


            InitializeUsers();

        }

        private void InitializeUsers()
        {
            alumniUser = new User
            {
                FirstName = "Vitaly",
                LastName = "Ermagambet",
                Email = Guid.NewGuid().ToString() + "@test.ru",
                Password = new Password("qwerty1234"),
                CreatedTime = DateTimeOffset.Now,
                IsMiptAlumni = true,
                AlumniProfile = new AlumniProfile
                {
                    Diploma = DiplomaType.EngeenerResarcher,
                    Faculty = FacultyType.Faculty2,
                    YearOfGraduation = 2016
                }
            };
            
            notAlumniUser = new User
            {
                FirstName = "Boris",
                LastName = "Ermagambet",
                Email = Guid.NewGuid().ToString() + "@test.ru",
                Password = new Password("qwerty1234"),
                CreatedTime = DateTimeOffset.Now,
                IsMiptAlumni = false,
                AlumniProfile = null
            };
        }
        
        private const string ConnectionString = "Server=127.0.0.1;Port=32774;Database=mipt;User Id=postgres;";
        private readonly IGenericRepository<User> _genericRepository;
        private User alumniUser;
        private User notAlumniUser;

    }
}