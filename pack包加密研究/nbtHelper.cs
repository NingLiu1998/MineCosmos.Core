using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpNBT;

namespace pack包加密研究
{
    public class nbtHelper
    {

        static string NBTID => "id";
        static string NBTCOUNT => "Count";
        static string NBTLVL => "lvl";
        static string NBTDAMAGE => "Damage";
        static string NBTTAG => "tag";
        static string NBTEnchantments => "Enchantments";

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
                    case StringTag stringTag when stringTag.Name.Equals("id"):
                        dic.TryAdd(stringTag.Name, stringTag.Value);
                        break;

                    case ByteTag byteTag when byteTag.Name.Equals("Count"):
                        dic.TryAdd(byteTag.Name, (int)byteTag.Value);
                        break;

                    case CompoundTag tagTag when tagTag.Name.Equals("tag"):
                        dic.TryAdd(tagTag.Name, TagToDic(tagTag, null));
                        break;

                    case ListTag tagList when tagList.Name.Equals("Enchantments"):
                        var lst = new List<Dictionary<string, object>>();
                        foreach (Tag tag in tagList)
                        {
                            var tagListDic = new Dictionary<string, object>();
                            tagListDic = TagToDic((CompoundTag)tag, tagListDic);
                            lst.Add(tagListDic);
                        }
                        dic.TryAdd(tagItem.Name, lst);
                        //dic.TryAdd(tagItem.Name, ListTagHandle(tagList));
                        break;

                    case ShortTag shortTag when shortTag.Name.Equals("lvl"):
                        dic.TryAdd(shortTag.Name, (int)shortTag.Value);
                        break;
                    case IntTag intTag when intTag.Name.Equals("Damage"):
                        dic.TryAdd(intTag.Name, intTag.Value);
                        break;
                }
            }
            return dic;
        }



        public static TagBuilder DicToTag2(Dictionary<string, object> dic, string name = "")
        {
            
             TagBuilder?   builder = new TagBuilder(name);
            foreach (var dicItem in dic)
            {
                switch (dicItem.Value)
                {
                    //对应long 也就是mc中的byte  b
                    case Int64 intVal when dicItem.Key.Equals(NBTCOUNT):
                        builder.AddByte("Count", Convert.ToByte(dicItem.Value));
                        break;

                    case string intVal when dicItem.Key.Equals(NBTID):
                        builder.AddString(dicItem.Key, dicItem.Value.ToString());
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTLVL):
                        builder.AddShort(NBTLVL, Convert.ToInt16(dicItem.Value));
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTDAMAGE):
                        builder.AddInt(NBTDAMAGE, Convert.ToInt32(dicItem.Value));
                        break;

                    case Newtonsoft.Json.Linq.JObject dicJobject when dicItem.Key.Equals(NBTTAG):
                        //Newtonsoft.Json.Linq.JArray
                        Dictionary<string, object> dictionary = dicJobject.ToObject<Dictionary<string, object>>();
                        var dicJobjectTag = DicToTag2(dictionary, NBTTAG).Create();
                        builder.AddTag(dicJobjectTag);

                        break;

                    case Newtonsoft.Json.Linq.JArray dicJArry when dicItem.Key.Equals(NBTEnchantments):

                        List<Dictionary<string, object>> list = dicJArry.ToObject<List<Dictionary<string, object>>>();
                        using (builder.NewList(TagType.Compound, dicItem.Key))
                        {
                            foreach (var dicJArryItem in list)
                            {
                                //foreach (var item in dicJArryItem)
                                //{
                                //    TagBuilder tb = new TagBuilder();
                                //    tb.AddString(item.Key,"test");
                                //    tb.AddString(item.Key+"111","test111");
                                //    builder.AddTag(tb.Create());
                                //}

                                foreach (var item in dicJArryItem)
                                {
                                    var itemTagBuilder = DicToTag2(ConvertToDictionary(item));
                                    builder.AddTag(itemTagBuilder.Create());
                                }

                                //var dicJarrTagBuilder = DicToTag2(dicJArryItem);
                                //builder.AddTag(dicJarrTagBuilder.Create());
                            }
                        }
                        break;
                }
            }
            return builder;
        }




        private static Dictionary<string, object> ConvertToDictionary(KeyValuePair<string, object> pair)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add(pair.Key, pair.Value);
            return dictionary;
        }


        public static CompoundTag DicToTag(Dictionary<string, object> dic, string name = "",TagBuilder? builder = null)
        {
            if(builder is null)
             builder = new TagBuilder(name);
            foreach (var dicItem in dic)
            {
                switch (dicItem.Value)
                {
                    //对应long 也就是mc中的byte  b
                    case Int64 intVal when dicItem.Key.Equals(NBTCOUNT):
                        builder.AddByte("Count", Convert.ToByte(dicItem.Value));
                        break;

                    case string intVal when dicItem.Key.Equals(NBTID):
                        builder.AddString(NBTID, dicItem.Key.ToString());
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTLVL):
                        builder.AddShort(NBTLVL, Convert.ToInt16(dicItem.Value));
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTDAMAGE):
                        builder.AddInt(NBTDAMAGE, Convert.ToInt32(dicItem.Value));
                        break;

                    case Newtonsoft.Json.Linq.JObject dicJobject when dicItem.Key.Equals(NBTTAG):
                        //Newtonsoft.Json.Linq.JArray
                        Dictionary<string, object> dictionary = dicJobject.ToObject<Dictionary<string, object>>();
                        var dicJobjectTag = DicToTag(dictionary, NBTTAG);
                        builder.AddTag(dicJobjectTag);

                        break;

                    case Newtonsoft.Json.Linq.JArray dicJArry when dicItem.Key.Equals(NBTEnchantments):

                        List<Dictionary<string, object>> list = dicJArry.ToObject<List<Dictionary<string, object>>>();
                        using (builder.NewList(TagType.Compound, dicItem.Key))
                        {
                            foreach (var dicJArryItem in list)
                            {
                                //foreach (var item in dicJArryItem)
                                //{
                                //    TagBuilder tb = new TagBuilder();
                                //    tb.AddString(item.Key,"test");
                                //    tb.AddString(item.Key+"111","test111");
                                //    builder.AddTag(tb.Create());
                                //}

                               CompoundTag? dicJarrTag = DicToTag(dicJArryItem);
                               builder.AddTag(dicJarrTag);
                            }
                        }
                        break;
                }
            }
            return builder.Create();
        }


    }

}
