using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    public class DigestNonce
    {
        //public static IMemoryCache _cache;
        static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static string Generate()
        {
            var bytes = new byte[16];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var nonce = bytes.ToMd5Hash();
            SetCache(nonce,0);
            return nonce;
        }
        


        public static bool IsValid(string nonce, string nonceCount)
        {

            var cacheNonce = _cache.Get(nonce) as int? ;

            // nonce has not expired yet
            if (cacheNonce != null)
            {
                //十六进制转换成十进制
                Int32 nc = Convert.ToInt32(nonceCount,16);
                //nonceCount is greater than the one in record
                if (nc > cacheNonce)
                {
                    //update the dictionary to reflect the nonce count just received in this request
                    //更新缓存的值，以表明nonceCount刚刚接收到了请求
                    SetCache(nonce,System.Convert.ToInt32(cacheNonce) +1);

                    return true;
                }
            }
            return false;
        }

        private static void SetCache(string nonce, int count)
        {
            //将nonce作为key,0作为初始值存入缓存,过期时间为10分钟
            _cache.Set(nonce, count,
               new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
               .SetPriority(CacheItemPriority.Normal));
        }
    }
}
