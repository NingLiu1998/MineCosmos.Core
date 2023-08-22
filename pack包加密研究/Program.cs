// See https://aka.ms/new-console-template for more information
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using KYSharp.SM;
using Newtonsoft.Json;
using pack包加密研究;
using SharpNBT;
using SharpNBT.SNBT;



Console.WriteLine("NBT转JSON测试");


//普通物品
string testNbtStr = "{Count:64b,id:\"minecraft:experience_bottle\"}";
//一个物品一个附魔
string testNbtStr1 = "{Count:1b,id:\"minecraft:netherite_sword\",tag:{Damage:0,Enchantments:[{id:\"minecraft:sharpness\",lvl:1s}]}}";
//一个箱子
string testNbtStt2 = "{Count:1b,id:\"minecraft:lime_shulker_box\",tag:{BlockEntityTag:{Items:[{Count:64b,Slot:0b,id:\"minecraft:grass_block\"},{Count:64b,Slot:1b,id:\"minecraft:grass_block\"},{Count:64b,Slot:2b,id:\"minecraft:grass_block\"},{Count:64b,Slot:3b,id:\"minecraft:grass_block\"},{Count:64b,Slot:4b,id:\"minecraft:grass_block\"},{Count:64b,Slot:5b,id:\"minecraft:grass_block\"},{Count:64b,Slot:6b,id:\"minecraft:grass_block\"},{Count:64b,Slot:7b,id:\"minecraft:grass_block\"},{Count:64b,Slot:8b,id:\"minecraft:grass_block\"},{Count:64b,Slot:9b,id:\"minecraft:grass_block\"},{Count:64b,Slot:10b,id:\"minecraft:grass_block\"},{Count:64b,Slot:11b,id:\"minecraft:grass_block\"},{Count:64b,Slot:12b,id:\"minecraft:grass_block\"},{Count:64b,Slot:13b,id:\"minecraft:grass_block\"},{Count:64b,Slot:14b,id:\"minecraft:grass_block\"},{Count:64b,Slot:15b,id:\"minecraft:grass_block\"},{Count:64b,Slot:16b,id:\"minecraft:grass_block\"},{Count:64b,Slot:17b,id:\"minecraft:grass_block\"},{Count:64b,Slot:18b,id:\"minecraft:grass_block\"},{Count:64b,Slot:19b,id:\"minecraft:grass_block\"},{Count:64b,Slot:20b,id:\"minecraft:grass_block\"},{Count:64b,Slot:21b,id:\"minecraft:grass_block\"},{Count:64b,Slot:22b,id:\"minecraft:grass_block\"},{Count:64b,Slot:23b,id:\"minecraft:grass_block\"},{Count:64b,Slot:24b,id:\"minecraft:grass_block\"},{Count:64b,Slot:25b,id:\"minecraft:grass_block\"},{Count:64b,Slot:26b,id:\"minecraft:grass_block\"}],id:\"minecraft:shulker_box\"}}}";
CompoundTag? tags = StringNbt.Parse(testNbtStr1);

Dictionary<string, object>? testDic =nbtHelper.TagToDic(tags, null);

string webJson =JsonConvert.SerializeObject(testDic);
Console.WriteLine($"展示用json:{webJson}");

var  dics = JsonConvert.DeserializeObject<Dictionary<string, object>>(webJson);

var Ntag = nbtHelper.DicToTag2(dics).Create();
string nSNBT =Ntag.Stringify();

Console.WriteLine($"回传NBT字符串:{webJson}");


Console.ReadKey();

//var builder = new TagBuilder("My NBT Document");

//foreach (var tagItem in tags)
//{
//    if (tagItem is StringTag stringTag)
//    {
//        Console.WriteLine($"{stringTag.Name} : {stringTag.Value}");

//        builder.AddString(stringTag.Name, stringTag.Value);
//    }
//    else if (tagItem is ByteTag byteTag)
//    {
//        Console.WriteLine($"{byteTag.Name} : {byteTag.Value}");
//        builder.AddByte(byteTag.Name, byteTag.Value);
//        //builder.AddTag()
//    }

//}

//CompoundTag? revertTag = builder.Create();

////string aa =revertTag.Stringify();
////string ss = revertTag.ToJsonString();


//var settings =new DataContractJsonSerializerSettings {

//    UseSimpleDictionaryFormat = true,
//    EmitTypeInformation = EmitTypeInformation.Never,
//    //KnownTypes = Get
//};




//NbtFile.Write("test.nbt", revertTag, FormatOptions.Java, CompressionType.None);







//pack 包
string jdata = "json数据1";
string guid1 = "e4dd81a3946e4cd8b6fe737481530a60";
string md51 = "5778eb2fd4c2810f8d5010802450061c";

//封包,提供json 数据和guid给用户，数据库存入 MD5 和 UUID
string guid = Guid.NewGuid().ToString("N").ToLower();
string? dMd5 = md5(jdata + guid);

//验证恢复还原，用户传入json数据和guid, 通过GUID 找到 MD5，验证用户传入的json数据MD5是否一致
if (md51 == md5(jdata+guid1))
{
    Console.WriteLine("验证通过,物品数据已存入仓库");
}
else
{
    Console.WriteLine("验证失败");
}


string md5(string val)
{
   return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(val)).Select(x => x.ToString("x2")));
}
