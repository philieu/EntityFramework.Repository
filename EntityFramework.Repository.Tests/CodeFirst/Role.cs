using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Repository.Tests.CodeFirst
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
