using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SharpNBT;
using SharpNBT.SNBT;

namespace MineCosmos.Core.Common.Helper;

#nullable enable

/// <summary>
/// Minecraft NBT 数据操作类
/// </summary>
public class NbtHelper
{
    #region Minecraft NBT 常见属性名

    static string NBTID => "id";
    static string NBTCOUNT => "Count";
    static string NBTLVL => "lvl";
    static string NBTDAMAGE => "Damage";
    static string NBTTAG => "tag";
    static string NBTEnchantments => "Enchantments";
    static string NBTSlot => "Slot";
    static string NBTBlockEntityTag => "BlockEntityTag";
    static string NBTItems => "Items";

   

    /// <summary>
    /// 物品的自定义显示信息（名称、描述、皮革盔甲的颜色等等）
    /// </summary>
    static string NBTdisplay => "display";

    /// <summary>
    /// 物品名称底下的文字，必须是原始JSON文本
    /// * 原版物品很少见
    /// </summary>
    static string NBTLore => "Lore";
    /// <summary>
    /// 书本
    /// </summary>
    static string NBTpages => "pages";

    /// <summary>
    /// 能够为生物以及物品增加属性修饰符
    /// </summary>
    static string NBTAttributeModifiers => "AttributeModifiers";

    #endregion

    public static CompoundTag NbtStrToString(string nbtStr)
    {
        return StringNbt.Parse(nbtStr);
    }

    public static Dictionary<string, object> NbtStrToDic(string nbtStr)
    {
       return TagToDic(NbtStrToString(nbtStr),null);
    }

    #region Tag 转 Dic

    public static Dictionary<string, object> TagToDic(CompoundTag tags, Dictionary<string, object>? dic)
    {
        if (dic is null)
            dic = new();
        //   dic.TryAdd(tagItem.Name, TagHandle(tagItem, dic));
        foreach (Tag tagItem in tags)
        {

            //dic.TryAdd(tagItem.Name, TagHandle(tagItem, dic));

            switch (tagItem)
            {

                case StringTag stringTag when stringTag.Name.Equals(NBTID):
                    dic.TryAdd(stringTag.Name, stringTag.Value);
                    break;

                case ByteTag byteTag when byteTag.Name.Equals(NBTCOUNT):
                    dic.TryAdd(byteTag.Name, (int)byteTag.Value);
                    break;

                case ByteTag byteTag when byteTag.Name.Equals(NBTSlot):
                    dic.TryAdd(byteTag.Name, (int)byteTag.Value);
                    break;

                case CompoundTag tagTag when tagTag.Name.Equals(NBTdisplay):
                    dic.TryAdd(tagTag.Name, TagToDic(tagTag, null));
                    break;

                case CompoundTag tagTag when tagTag.Name.Equals(NBTTAG):
                    dic.TryAdd(tagTag.Name, TagToDic(tagTag, null));
                    break;

                case CompoundTag tagTag when tagTag.Name.Equals(NBTBlockEntityTag):
                    dic.TryAdd(tagTag.Name, TagToDic(tagTag, null));
                    break;

                //ListTag

                //比较特殊的字符串数组值（书本
                case ListTag tagList when tagList.Name.Equals(NBTpages):
                    dic.TryAdd(tagItem.Name, TagStringListHandle(tagList));
                    break;
                case ListTag tagList when tagList.Name.Equals(NBTAttributeModifiers):
                    dic.TryAdd(tagItem.Name, TagListToDicHandle(tagList));
                    break;
                case ListTag tagList when tagList.Name.Equals(NBTLore):
                    dic.TryAdd(tagItem.Name, TagListToDicHandle(tagList));
                    break;
                case ListTag tagList when tagList.Name.Equals(NBTItems):
                    dic.TryAdd(tagItem.Name, TagListToDicHandle(tagList));
                    break;
                case ListTag tagList when tagList.Name.Equals(NBTEnchantments):
                    dic.TryAdd(tagItem.Name, TagListToDicHandle(tagList));
                    break;

                case ShortTag shortTag when shortTag.Name.Equals(NBTLVL):
                    dic.TryAdd(shortTag.Name, (int)shortTag.Value);
                    break;

                //int (就是后没没带英文的数字
                case IntTag intTag when intTag.Name.Equals("RepairCost"):
                    dic.TryAdd(intTag.Name, intTag.Value);
                    break;
                case IntTag intTag when intTag.Name.Equals(NBTDAMAGE):
                    dic.TryAdd(intTag.Name, intTag.Value);
                    break;
            }
        }
        return dic;
    }

    private static List<string> TagStringListHandle(ListTag tagList)
    {
        var lstString = new List<string>();
        foreach (Tag tag in tagList)
        {
            if (tag is StringTag strTag)
            {
                lstString.Add(strTag.Value);
            }
            else throw new Exception("未处理数组异常");
        }
        return lstString;
    }

    private static List<Dictionary<string, object>> TagListToDicHandle(ListTag tagList)
    {
        var lst = new List<Dictionary<string, object>>();
        foreach (Tag tag in tagList)
        {
            var tagListDic = new Dictionary<string, object>();
            if (tag is CompoundTag)
            {
                tagListDic = TagToDic((CompoundTag)tag, tagListDic);
            }
            else
            {
                throw new Exception("未处理的TagList");
            }

            lst.Add(tagListDic);
        }
        return lst;
    }

    #endregion

    #region Dic 转 Tag

    public static TagBuilder DicToTag(Dictionary<string, object> dic, string? name = null)
    {
        TagBuilder? builder = new TagBuilder(name);
        foreach (var dicItem in dic)
        {
            switch (dicItem.Value)
            {

                case Int64 intVal when dicItem.Key.Equals(NBTCOUNT):
                    builder.AddByte(dicItem.Key, Convert.ToByte(dicItem.Value));
                    break;
                case Int64 intVal when dicItem.Key.Equals("Slot"):
                    builder.AddByte(dicItem.Key, Convert.ToByte(dicItem.Value));
                    break;

                case string intVal when dicItem.Key.Equals(NBTID):
                    builder.AddString(dicItem.Key, dicItem.Value.ToString());
                    break;

                case Int64 intVal when dicItem.Key.Equals(NBTLVL):
                    builder.AddShort(dicItem.Key, Convert.ToInt16(dicItem.Value));
                    break;

                case Int64 intVal when dicItem.Key.Equals(NBTDAMAGE):
                    builder.AddInt(dicItem.Key, Convert.ToInt32(dicItem.Value));
                    break;


                case JObject blockJobject when dicItem.Key.Equals("BlockEntityTag"):
                    builder.AddTag(DicJobjectHandle(blockJobject, dicItem.Key));
                    break;

                case JObject dicJobject when dicItem.Key.Equals(NBTTAG):
                    Dictionary<string, object> dictionary = dicJobject.ToObject<Dictionary<string, object>>();
                    var dicJobjectTag = DicToTag(dictionary, dicItem.Key).Create();
                    builder.AddTag(dicJobjectTag);
                    break;

                case JArray dicPagesJArry when dicItem.Key.Equals(NBTpages):

                    List<string> stringList = dicPagesJArry.Select(item => (string)item).ToList();
                    using (builder.NewList(TagType.String, dicItem.Key))
                    {
                        foreach (var str in stringList)
                        {
                            builder.AddString(str);
                        }
                    }

                    break;
                case JArray dicJArry when dicItem.Key.Equals(NBTEnchantments):
                    List<Dictionary<string, object>> list = dicJArry.ToObject<List<Dictionary<string, object>>>();
                    using (builder.NewList(TagType.Compound, dicItem.Key))
                    {
                        foreach (var dicJArryItem in list)
                        {
                            foreach (var item in dicJArryItem)
                            {
                                var itemTagBuilder = DicToTag(ConvertToDictionary(item));
                                builder.AddTag(itemTagBuilder.Create());
                            }
                        }
                    }
                    break;
            }
        }
        return builder;
    }

    private static CompoundTag? DicJobjectHandle(JObject dicJobject, string name)
    {
        Dictionary<string, object> dictionary = dicJobject.ToObject<Dictionary<string, object>>();
        var dicJobjectTag = DicToTag(dictionary, name).Create();
        return dicJobjectTag;
    }

    private static Dictionary<string, object> ConvertToDictionary(KeyValuePair<string, object> pair)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add(pair.Key, pair.Value);
        return dictionary;
    }

    #endregion

}

