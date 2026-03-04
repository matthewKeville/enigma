using Plugin;

namespace enigma.Test.Serialization;

public class SerializationTests
{

    [Fact]
    public void RequestSerializationSmoke() {
      Request request = new Request
      {
          Version = "v1",
          Type = Request.RequestType.Info,
      };
      Console.WriteLine(Request.SerializeJson(request));

    }
}

public class DeserializationTests
{

    [Fact]
    public void RequestSerializationSmoke() {

      String json = """{"Version":"v1","Type":0,"Info":{"Name":"Testing Plugin","Description":"A Testing Plugin","Version":"v1.23"}}""";
      Response response = Response.DeserializeJson(json);
      Console.WriteLine(response.ToString());

    }
}
