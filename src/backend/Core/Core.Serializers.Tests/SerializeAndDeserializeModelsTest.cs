using System.IO;
using Core.BinarySerializer;
using Core.CryptProtocol.Domain;
using FluentAssertions;
using Xunit;

namespace Core.Serializers.Tests;

public class SerializeAndDeserializeModelsTest
{
    [Theory]
    [AutoMoqData]
    public void Serialize_And_Deserialize_GetDialogModel_Test(GetMessageRequest request)
    {
        var writerStream = new MemoryStream();
        var serializableResponseStream = new MemoryStream();
        
        var binaryWriter = new BinaryWriter(writerStream);
        var binarySerializer = new BinaryMessangerSerializer(binaryWriter);
        request.Serialize(binarySerializer);
        
        binarySerializer.WriteResult(serializableResponseStream);
        var binaryReader = new BinaryReader(serializableResponseStream);

        var binaryDeserializer = new BinaryMessangerDeserializer(binaryReader);
        
        var deserializedModel = binaryDeserializer.Deserialize<GetMessageRequest>();
        
        request.DialogId.Should().Be(deserializedModel.DialogId);
        request.UserName.Should().Be(deserializedModel.UserName);
        request.DateWichNeedSendMessage.ToString().Should().Be(deserializedModel.DateWichNeedSendMessage.ToString());
    }

    [Theory]
    [AutoMoqData]
    public void TestMessageBinarySerializerClass(GetMessageRequest request)
    {
        var serializedMessage = request.ToBinaryMessage();
        var deserializedModel = serializedMessage.FromBinaryMessage<GetMessageRequest>();
        
        request.DialogId.Should().Be(deserializedModel.DialogId);
        request.UserName.Should().Be(deserializedModel.UserName);
        request.DateWichNeedSendMessage.ToString().Should().Be(deserializedModel.DateWichNeedSendMessage.ToString());
    }
}