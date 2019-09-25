using Microsoft.VisualStudio.TestTools.UnitTesting;
using XXXNotas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXNotas.Tests.ModelTests
{
    [TestClass()]
    public class CategoryTests
    {
        [TestMethod()]
        public void Equals_TwoEqualCategories_ReturnTrue()
        {
            GetEqualCategories(out Category cat1, out Category cat2);

            bool result = cat1.Equals(cat2);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Equals_TwoNotEqualCategories_ReturnFalse()
        {
            GetNotEqualCategories(out Category cat1, out Category cat2);

            bool result = cat1.Equals(cat2);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetHashCode_TwoEqualCategories_RetrunSameHashCode()
        {
            GetEqualCategories(out Category cat1, out Category cat2);

            bool result = cat1.GetHashCode() == cat2.GetHashCode();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void GetHashCode_TwoNotEqualCategories_RetrunDifferentHashCode()
        {
            GetNotEqualCategories(out Category cat1, out Category cat2);

            bool result = cat1.GetHashCode() != cat2.GetHashCode();

            Assert.IsTrue(result);
        }
        
        #region Private Method
        /// <summary>
        /// Get Two Equal Category
        /// </summary>
        /// <param name="cat1">category 1</param>
        /// <param name="cat2">category 2</param>
        private void GetEqualCategories(out Category cat1, out Category cat2)
        {
            Guid guid = Guid.NewGuid();
            cat1 = new Category
            {
                Id = guid,
                Name = "cat1"
            };
            cat2 = new Category
            {
                Id = guid,
                Name = "cat2"
            };
        }

        /// <summary>
        /// Get Two Not Equal Category
        /// </summary>
        /// <param name="cat1">category 1</param>
        /// <param name="cat2">category 2</param>
        private void GetNotEqualCategories(out Category cat1, out Category cat2)
        {
            cat1 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "cat1"
            };
            cat2 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "cat2"
            };
        }
        #endregion
    }
}