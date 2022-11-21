using Discord;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDbService.Models;

namespace Sancus_Discord.NET.Models;

public class BotSettings : EntityBase
{
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color MessageEditLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color MessageDeleteLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color MessageCreatedLogColor { get; set; }

    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color UserJoinLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color UserLeaveLogColor { get; set; }

    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color BanLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color KickLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color TimeoutLogColor { get; set; }
    [BsonSerializer(typeof(BsonColorStringSerializer))] public Color CaseLogColor { get; set; }
}

public class BsonColorStringSerializer : SerializerBase<Color>
{
    public override Color Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var colorString = context.Reader.ReadString();
        var uint32String = colorString.Substring(1);

        var colorUInt = Convert.ToUInt32(uint32String, 16);

        return new Color(colorUInt);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Color value)
    {
        context.Writer.WriteString(value.ToString());
    }
}