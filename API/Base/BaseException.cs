using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    public class BaseException
    {
        public const string DataDuplicate_Email = "Email sudah pernah terdaftar.";
        public const string DataDuplicate_Phone = "Nomor telepon sudah pernah terdaftar.";
    }
}
