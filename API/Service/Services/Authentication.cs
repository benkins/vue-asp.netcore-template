using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data;
using Data.Models;
using Service.Interfaces;
using Service.Models;

namespace Service.Services
{
    public class Authentication : IAuthentication
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher _passwordHasher;

        public Authentication(IMapper mapper, IRepository<User> repository)
        {
            _mapper = mapper;
            _repository = repository;
            _passwordHasher = new PasswordHasher();

        }

        public UserModel Authenticate(string email, string password)
        {
            var user = _repository.Get(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            var verificationResult = _passwordHasher.VerifyHashedPassword(user.Password, password);
            return verificationResult == PasswordVerificationResult.Success ? _mapper.Map<UserModel>(user) : null;
        }

        public IEnumerable<UserModel> GetAll()
        {
            var users = _repository.Get();

            return users.Select(user => _mapper.Map<UserModel>(user)).ToList();
        }

        public UserModel GetById(int id)
        {
            return _mapper.Map<UserModel>(_repository.GetById(id));
        }

        public void Add(UserModel user)
        {
            user.Password = _passwordHasher.HashPassword(user.Password);
            _repository.Add(_mapper.Map<User>(user));
        }
    }
}
