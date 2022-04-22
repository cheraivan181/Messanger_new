using Core.BinarySerializer;
using FluentAssertions;
using Xunit;

namespace Core.Serializers.Tests;

public class DeserializerTest
{
    [Theory]
    [InlineAutoMoqData(false)]
    [InlineAutoMoqData(true)]
    public void Read_Boolean_Success(bool value, 
        BinaryMessangerSerializer serializer, BinaryMessangerDeserializer deserializer)
    {
        // Act
        serializer.Write(value);
        deserializer.ResetPosition();
        var result = deserializer.ReadBoolean();

        // Assert
        result.Should().Be(value);
    }

    [Theory]
    [AutoMoqData]
    public void Read_String_Success(string value, 
        BinaryMessangerSerializer serializer, BinaryMessangerDeserializer deserializer)
    {
        // Act
        serializer.Write(value);
        deserializer.ResetPosition();
        var result = deserializer.ReadString();

        // Assert
        result.Should().Be(value);
    }
}