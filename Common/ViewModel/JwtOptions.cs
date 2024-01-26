using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class JwtOptions
    {
        public string Issuer {  get; set; } //发行者

        public string Audience { get; set; } //观众

        public string SecKey { get; set; } //密钥

        public string Expireseconds {  get; set; } //过期时间
    }
}
