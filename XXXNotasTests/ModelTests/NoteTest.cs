using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXNotas.Model;

namespace XXXNotas.Tests.ModelTests
{
    [TestClass()]
    public class NoteTest
    {
        [TestMethod()]
        public void Equals_TwoEqualNotes_ReturnTrue()
        {
            GetEqualNotes(out Note note1, out Note note2);

            bool result = note1.Equals(note2);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Equals_TwoNotEqualNotes_ReturnFalse()
        {
            GetNotEqualNotes(out Note note1, out Note note2);

            bool result = note1.Equals(note2);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetHashCode_TwoEqualNotes_GetSameHashCode()
        {
            GetEqualNotes(out Note note1, out Note note2);

            bool result = note1.GetHashCode() == note2.GetHashCode();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void GetHashCode_TwoNotEqualNotes_GetDifferentHashCode()
        {
            GetNotEqualNotes(out Note note1, out Note note2);

            bool result = note1.GetHashCode() != note2.GetHashCode();

            Assert.IsTrue(result);
        }
        
        #region Private Method
        /// <summary>
        /// Get Two Equal Notes
        /// </summary>
        /// <param name="note1">note 1</param>
        /// <param name="note2">note 2</param>
        private void GetEqualNotes(out Note note1, out Note note2)
        {
            Guid guid = Guid.NewGuid();
            note1 = new Note
            {
                Id = guid,
                Content = "note1"
            };
            note2 = new Note
            {
                Id = guid,
                Content = "note2"
            };
        }

        /// <summary>
        /// Get Two Note Equal Notes
        /// </summary>
        /// <param name="note1">note 1</param>
        /// <param name="note2">note 2</param>
        private void GetNotEqualNotes(out Note note1, out Note note2)
        {
            note1 = new Note
            {
                Id = Guid.NewGuid(),
                Content = "note1"
            };
            note2 = new Note
            {
                Id = Guid.NewGuid(),
                Content = "note2"
            };
        }
        #endregion
    }
}
