using System;
using GenesisChallenge.Helpers.Hashing;
using NUnit.Framework;

namespace Tests.Helpers.Hashing
{
    public class HashTests
    {    
        [Test]
        public void ShouldMatchHashWhenHashUntampered()
        {
            GivenStringToHash("passw0rd");
            GivenSalt();
            GivenHash();
            WhenValidateHash();
            ThenHashIsValid();
        }

        [Test]
        public void ShouldNotMatchHashWhenHashTampered()
        {
            GivenStringToHash("passw0rd");
            GivenSalt();
            GivenTamperedHash();
            WhenValidateHash();
            ThenHashIsInvalid();
        }

        [Test]
        public void ShouldGenerateDifferentHashesWhenDifferentStringsToHash()
        {
            GivenStringToHash("passw0rd");
            GivenAnotherStringToHash("password");
            GivenSalt();
            GivenHash();
            GivenAnotherHash();
            ThenHashesDontMatch();
        }

        private void GivenSalt()
        {
            _salt = Salt.Create();
        }

        private void GivenStringToHash(string value)
        {
            _stringToHash = value;
        }

        private void GivenAnotherStringToHash(string value)
        {
            _anotherStringToHash = value;
        }

        private void GivenHash()
        {
            _hash = Hash.Create(_stringToHash, _salt);
        }

        private void GivenAnotherHash()
        {
            _anotherHash = Hash.Create(_anotherStringToHash, _salt);
        }

        private void GivenTamperedHash()
        {
            _hash = "lalalalala";
        }

        private void WhenValidateHash() {
            _validation = Hash.Validate(_stringToHash, _salt, _hash);
        }

        private void ThenHashIsValid()
        {
            Assert.True(_validation);
        }

        private void ThenHashIsInvalid()
        {
            Assert.False(_validation);
        }

        private void ThenHashesDontMatch()
        {
            Assert.True(_hash != _anotherHash);
        }

        private string _stringToHash;
        private string _anotherStringToHash;
        private string _salt;
        private string _hash;
        private string _anotherHash;
        private bool _validation;
    }
}