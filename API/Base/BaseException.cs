using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    public class BaseException
    {
        public const string FailedRequest = "Failed to Request data";
        public const string SuccessRequest = "Success to Request data";
        public const string FailedRegister = "Failed to Register data";
        public const string SuccessRegister = "Success to Register data";
        public const string FailedUpdate = "Failed to Update data";
        public const string SuccessUpdate = "Success to Update data";
        public const string FailedDelete = "Failed to Delete data";
        public const string SuccessDelete = "Success to Delete data";
        public const string FailedChangePass = "Failed to Change Password";

        public const string Msg_SuccessRegister = "Data submitted for Register";
        public const string Msg_SuccessDelete = "Request submitted successfully";
        public const string Msg_SuccessUpdate = "Data has been updated";

        public const string DataDuplicate_Email = "Email sudah pernah terdaftar.";
        public const string DataDuplicate_Phone = "Nomor telepon sudah pernah terdaftar.";
        public const string DataNotFound_Email = "Email yang dimasukkan tidak ditemukan.";
    }
}
