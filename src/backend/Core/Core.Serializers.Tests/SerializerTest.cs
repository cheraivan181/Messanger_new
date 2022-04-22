using System;
using System.IO;
using System.Linq;
using System.Text;
using Core.BinarySerializer;
using FluentAssertions;
using Xunit;

namespace Core.Serializers.Tests;

public class SerializerTest
{
    [Theory]
    [AutoMoqData]
    public void Serialize_Int_Test(int value,
        BinaryMessangerSerializer messangerSerializer, MemoryStream output)
    {
        messangerSerializer.Write(value);
        messangerSerializer.WriteResult(output);

        BitConverter.ToInt32(output.ToArray()).Should().Be(value);
    }
    
    
    [Theory]
    [AutoMoqData]
    public void Serialize_String_Test(string value,
        BinaryMessangerSerializer messangerSerializer, MemoryStream output)
    {
        messangerSerializer.WriteString(value);
        messangerSerializer.WriteResult(output);

        BitConverter.ToInt32(output.ToArray().Take(4).ToArray()).Should().BeGreaterThan(0);
        Encoding.UTF8.GetString(output.ToArray().Skip(4).ToArray()).Should().Be(value);
    }
}