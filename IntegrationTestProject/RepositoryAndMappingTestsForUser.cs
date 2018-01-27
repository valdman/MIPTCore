using System;
using System.Threading;
using Common.Entities;
using Common.Infrastructure;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using UserManagment;
using Xunit;

namespace IntegrationTestProject
{
    public class RepositoryAndMappingTestsForUser
    {
        [Fact]
        public void CreatingAndThanDeletingUserCreateAndThanDeleteUser()
        {
            //Act
            var id = _genericRepository.Create(alumniUser);
            var createdUser = _genericRepository.GetById(id);
            //alumniUser.Id = id;

            _genericRepository.Delete(id);
            var deletedUser = _genericRepository.GetById(id);
            
            //Assert
            Assert.Equal(alumniUser, createdUser);
            Assert.Null(deletedUser);
        }

        [Fact]
        public void CreatingAndThanUpdatingUserCreateAndThanUpdateUser()
        {
            //Act
            var id = _genericRepository.Create(notAlumniUser);

            notAlumniUser.IsMiptAlumni = true;
            notAlumniUser.AlumniProfile = alumniUser.AlumniProfile;

            _genericRepository.Update(notAlumniUser);
            var updatedUser = _genericRepository.GetById(id);
            
            _genericRepository.Delete(id);
            
            //Assert
            Assert.Equal(updatedUser, notAlumniUser);
            Assert.NotNull(updatedUser.AlumniProfile);
        }
        
        [Fact]
        public void CreatingAndThanDoubleUpdatingUserCreateAndThanDoubleUpdateUser()
        {
            //Act
            var id = _genericRepository.Create(notAlumniUser);

            notAlumniUser.IsMiptAlumni = true;
            notAlumniUser.AlumniProfile = alumniUser.AlumniProfile;
            _genericRepository.Update(notAlumniUser);
            
            var updatedUser = _genericRepository.GetById(id);

            notAlumniUser.FirstName = Guid.NewGuid().ToString().Substring(0, 10);
            notAlumniUser.AlumniProfile.Faculty = "ФАКИ";
            _genericRepository.Update(notAlumniUser);
            Thread.Sleep(5000);
            
            var secondTimeUpdatedUser = _genericRepository.GetById(id);
            
            _genericRepository.Delete(id);
            
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