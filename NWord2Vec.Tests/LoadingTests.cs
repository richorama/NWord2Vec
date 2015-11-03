using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NWord2Vec.Tests
{
    [TestClass]
    public class LoadingTests
    {
        [TestMethod]
        public void TestLoading()
        {
            var model = Model.Load("model.txt");
            Assert.IsNotNull(model);
            Assert.AreEqual(4501, model.Words);
            Assert.AreEqual(100, model.Size);
            Assert.AreEqual(4501, model.Vectors.Count());
            Assert.IsTrue(model.Vectors.Any(x => x.Word == "whale"));


            var whale = model.GetByWord("whale");
            Assert.IsNotNull(whale);

            var xyz = model.GetByWord("xyz");
            Assert.IsNull(xyz);

            var results = model.Nearest(whale.Vector).Take(10).ToArray();
            Assert.AreEqual(10, results.Length);
            Assert.AreEqual("whale", results[0].Word);

            var results2 = model.Nearest("whale").Take(10).ToArray();
            Assert.AreEqual(10, results2.Length);
            Assert.AreNotEqual("whale", results2[0].Word);
            Assert.AreEqual("whale,", results2[0].Word);

            var nearest = model.NearestSingle(model.GetByWord("whale").Subtract(model.GetByWord("sea")));
            Assert.IsNotNull(nearest);

            Assert.AreNotEqual(0, model.Distance("whale", "boat"));



            var king = model.GetByWord("whale");
            var man = model.GetByWord("boat");
            var woman = model.GetByWord("sea");

            var vector = king.Subtract(man).Add(woman);
            Console.WriteLine(model.NearestSingle(vector));

        }

        [TestMethod]
        public void TestVectorAddition()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Add(x);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(4, result[1]);
            Assert.AreEqual(6, result[2]);
        }

        [TestMethod]
        public void TestVectorSubtraction()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Subtract(x);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
            Assert.AreEqual(0, result[2]);
        }

        [TestMethod]
        public void TestVectorDistance()
        {
            var x = new float[] { 1, 3, 4 };
            var y = new float[] { 1, 0, 0 };
            var result = x.Distance(y);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }



    }
}
