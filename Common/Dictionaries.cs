using ToTypeScript.Tests;

namespace ToTypeScript.Test.Common
{
    public class Order
    {
        public Dictionary<string, OrderLineItem> OrderLines { get; set; }

        public Dictionary<int, OrderLineItem> LinesByIndex { get; set; }

        public Dictionary<string, string> SimpleDict1 { get; set; }

        public Dictionary<int, int> SimpleDict2 { get; set; }

        public OrderDictionary SimpleDict3 { get; set; }

        public Order(Dictionary<string, OrderLineItem> orderLines, Dictionary<int, OrderLineItem> linesByIndex, Dictionary<string, string> simpleDict1, Dictionary<int, int> simpleDict2)
        {
            OrderLines = orderLines;
            LinesByIndex = linesByIndex;
            SimpleDict1 = simpleDict1;
            SimpleDict2 = simpleDict2;
        }
    }

    public class OrderLineItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderDictionary : Dictionary<string, Order>
    {
    }

    [TestClass]
    public class Dictionaries : BaseTest
    {
        [TestMethod]
        public void CanOutputDictionaryTypes()
        {
            var scripter = new TypeScriber();
            var output = scripter
               .AddType(typeof(Order))
               .ToString();

            ValidateTypeScript(output);
        }

        [TestMethod]
        public void TestThatDictionaryIsRendered()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(Order))
                .ToString();
            Assert.IsTrue(output.Contains("OrderLineItem"));
            ValidateTypeScript(output);
            Assert.IsTrue(output.Contains("LinesByIndex: {[key: number]: ToTypeScript.Test.Common.OrderLineItem;}"));
            Assert.IsTrue(output.Contains("OrderLines: {[key: string]: ToTypeScript.Test.Common.OrderLineItem;}"));
            Assert.IsTrue(output.Contains("SimpleDict1: {[key: string]: string;}"));
            Assert.IsTrue(output.Contains("SimpleDict2: {[key: number]: number;}"));
            Assert.IsTrue(output.Contains("SimpleDict3: ToTypeScript.Test.Common.OrderDictionary;"));
        }
    }
}
