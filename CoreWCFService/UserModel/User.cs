using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public User() { }

        public User(string username, string encryptedPass)
        {
            Username = username;
            EncryptedPassword = encryptedPass;
        }
    }
}
