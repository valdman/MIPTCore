using System;
using System.Threading;
using Common;
using Common.Infrastructure;
using DataAccess;
using DataAccess.Contexts;
using DataAccess.Repositories;
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
            var createdUser = await _genericRepository.GetByIdAsync(id);
            //alumniUser.Id = id;

            await _genericRepository.DeleteAsync(id);
            var deletedUser = await _genericRepository.GetByIdAsync(id);
            
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
            var updatedUser = await _genericRepository.GetByIdAsync(id);
            
            await _genericRepository.DeleteAsync(id);
            
            //Assert
            Assert.Equal(updatedUser, notAlumniUser);
            Assert.NotNull(updatedUser.AlumniProfile);
        }
        
        [Fact]
        public async void CreatingAndThanDoubleUpdatingUserCreateAndThanDoubleUpdateUser()
        {
            //Act
            var id = await _genericRepository.CreateAsync(notAlumniUser);

            notAlumniUser.IsMiptAlumni = true;
            notAlumniUser.AlumniProfile = alumniUser.AlumniProfile;
            await _genericRepository.UpdateAsync(notAlumniUser);
            
            var updatedUser = await _genericRepository.GetByIdAsync(id);

            notAlumniUser.FirstName = Guid.NewGuid().ToString().Substring(0, 10);
            notAlumniUser.AlumniProfile.Faculty = "ФАКИ";
            await _genericRepository.UpdateAsync(notAlumniUser);
            Thread.Sleep(5000);
            
            var secondTimeUpdatedUser = await _genericRepository.GetByIdAsync(id);
            
            await _genericRepository.DeleteAsync(id);
            
            //Assert
            Assert.Equal(updatedUser, notAlumniUser);
            Assert.Equal(secondTimeUpdatedUser, notAlumniUser);
            Assert.NotNull(updatedUser.AlumniProfile);
            Assert.NotNull(secondTimeUpdatedUser.AlumniProfile);
        }

        public RepositoryAndMappingTestsForUser()
        {
            throw new NotImplementedException();
            
            var contextOptions = new DbContextOptionsBuilder()
                .UseNpgsql(ConnectionString)
                .Options;
            var sessionProvider = new UserContext(null);
            
            _genericRepository = new UserRepository(sessionProvider);
            
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
                CreatingTime = DateTimeOffset.Now,
                IsMiptAlumni = true,
                AlumniProfile = new AlumniProfile
                {
                    Faculty = "ФОПФ",
                    YearOfGraduation = 2016
                }
            };
            
            notAlumniUser = new User
            {
                FirstName = "Boris",
                LastName = "Ermagambet",
                Email = Guid.NewGuid().ToString() + "@test.ru",
                Password = new Password("qwerty1234"),
                CreatingTime = DateTimeOffset.Now,
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