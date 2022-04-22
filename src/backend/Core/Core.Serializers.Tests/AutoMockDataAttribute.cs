using System.IO;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Core.Serializers.Tests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() =>
        {
            var ms = new MemoryStream();
            var fixture = new Fixture().Customize(new AutoMoqCustomization())
                .Customize(new SupportMutableValueTypesCustomization());
            fixture.Register(() => new BinaryWriter(ms));
            fixture.Register(() => new BinaryReader(ms));
            fixture.Register(() => new MemoryStream());
            return fixture;
        })
    {
    }
}
public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] arguments) 
        : base(new AutoMoqDataAttribute(), arguments)
    {
            
    }
}