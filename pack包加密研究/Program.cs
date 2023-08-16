// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text;
using KYSharp.SM;

Console.WriteLine("Hello, World!");


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


List<NormalDataModel> lstNormal = new List<NormalDataModel>() {
     new (){ Data = "json数据1", UserId  ="u1"},
     new (){ Data = "json数据2", UserId  ="u2"},
     new (){ Data = "json数据3", UserId  ="u3"}
};

List<PackDatModel> packDats = new()
{
     new (){ Id ="e4dd81a3946e4cd8b6fe737481530a60",Md5 =  "5778eb2fd4c2810f8d5010802450061c"}
};

//转包数据存凭证
string guid =  Guid.NewGuid().ToString("N").ToLower();
string?  dMd5 = md5(lstNormal[0].Data+guid);
packDats.Add(new PackDatModel { Md5 = dMd5, Id = guid});

//保存实体
using (StreamWriter sw = new StreamWriter(@$"D:\ln2023\PrivateProject\commonWeb\target\{dMd5}.mcm.pack", false, System.Text.Encoding.UTF8))
{
    sw.Write(lstNormal[0].Data);
}


Console.ReadLine();

string md5(string val)
{
   return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(val)).Select(x => x.ToString("x2")));
}


class PackDatModel
{
    public string Id { get; set; }
    public string Md5 { get; set; }
}

class NormalDataModel
{
    public string Data { get; set; }
    public string UserId { get; set; }
}
