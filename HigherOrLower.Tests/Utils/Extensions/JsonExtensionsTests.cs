using Xunit;
using HigherOrLower.Utils.Extensions;
using FluentAssertions;
using Newtonsoft.Json;

namespace HigherOrLower.Tests.Utils.Extensions
{
    public class JsonExtensionsTests
    {
        [Theory]
        [InlineData(null, "{Message: null}")]
        [InlineData("", "{Message: \"\"}")]
        [InlineData("abc", "{Message: \"abc\"}")]
        [InlineData("a1b2c3", "{Message: \"a1b2c3\"}")]
        public void ToJsonMessageTests(string input, string expectedOutput)
        {
            input.ToJsonMessage().Should().Be(IndentJsonStr(expectedOutput));
        }

        [Fact]
        public void ToJsonTest()
        {
            var testClass = new TestClassForToJson
            {
                SomeField = "abc",
                OtherStrField = null,
                OtherField = 9,
                EnumField = TestEnumForToJSon.Dummy2,
                ListOfStrings = new List<string> { "a", "b" },
                ListOfClasses = new List<AnotherTestClassForToJson>
                {
                    new AnotherTestClassForToJson
                    {
                        Field1 = "a",
                        Field2 = true,
                    },
                    new AnotherTestClassForToJson
                    {
                        Field1 = "b",
                        Field2 = false,
                    },
                }
            };


            testClass.ToJson().Should().Be(IndentJsonStr(@"
                {
                    ""SomeField"": ""abc"",
                    ""OtherStrField"": null,
                    ""OtherField"": 9,
                    ""EnumField"": ""Dummy2"",
                    ""ListOfStrings"": [""a"", ""b""],
                    ""ListOfClasses"": [
                        {
                            ""Field1"": ""a"",
                            ""Field2"": true,
                        },
                        {
                            ""Field1"": ""b"",
                            ""Field2"": false,
                        }
                    ]
                }
                "));
        }

        private enum TestEnumForToJSon
        {
            Dummy1,
            Dummy2
        }

        private class TestClassForToJson
        {
            public string SomeField { get; set; }

            public string? OtherStrField { get; set; }

            public int OtherField { get; set; }

            public TestEnumForToJSon EnumField { get; set; }

            public List<string> ListOfStrings { get; set; }

            public List<AnotherTestClassForToJson> ListOfClasses { get; set; }
        }

        private class AnotherTestClassForToJson
        {
            public string Field1 { get; set; }

            public bool Field2 { get; set; }
        }

        private string IndentJsonStr(string str)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(str), Formatting.Indented);
        }
    }
}
