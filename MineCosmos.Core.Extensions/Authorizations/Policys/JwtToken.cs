using MineCosmos.Core.AuthHelper.OverWrite;
using MineCosmos.Core.Model.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MineCosmos.Core.AuthHelper
{
    /// <summary>
    /// JWTToken生成类
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims">需要在登陆的时候配置</param>
        /// <param name="permissionRequirement">在startup中定义的参数</param>
        /// <returns></returns>
        public static TokenInfoViewModel BuildJwtToken(
            Claim[] claims, 
            PermissionRequirement permissionRequirement,
            int uType =0,string uuid = "")
        {
            var now = DateTime.Now;
            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience: permissionRequirement.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(permissionRequirement.Expiration),
                signingCredentials: permissionRequirement.SigningCredentials
            );

            //0用户 1玩家  3 第三方系统
            jwt.Payload.Add("UserType", uType);
            jwt.Payload.Add("Uuid", uuid);

            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new TokenInfoViewModel
            {
                success = true,
                token = encodedJwt,
                expires_in = permissionRequirement.Expiration.TotalSeconds,
                token_type = "Bearer"
            };
            return responseJson;
        }

        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            // token校验
            if (jwtStr.IsNotEmptyOrNull() && jwtHandler.CanReadToken(jwtStr))
            {

                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                object role;
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out object name);

                jwtToken.Payload.TryGetValue("UserType", out object userTypeObject);
                int.TryParse(userTypeObject.ToString(), out int userType);

                jwtToken.Payload.TryGetValue("Uuid", out object uuid);


                tokenModelJwt = new TokenModelJwt
                {
                    Uid = (jwtToken.Id).ObjToInt(),
                    Role = role != null ? role.ObjToString() : "",
                    Uuid = uuid.ToString(),
                    Name = name.ToString(),
                    UserTypeObject = userType
                };
            }
            return tokenModelJwt;
        }
    }
}
