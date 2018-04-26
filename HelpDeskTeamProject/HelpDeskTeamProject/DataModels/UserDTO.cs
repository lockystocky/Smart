using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class UserDTO
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(20)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(20)]
        public string Surname { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        public string AppId { get; set; }

        public bool IsBanned { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ApplicationRole AppRole { get; set; }

        public UserDTO()
        {

        }

        public UserDTO(string email, string appId, ApplicationRole appRole)
        {
            AppId = appId;
            Email = email;
            AppRole = appRole;
            IsBanned = false;
        }

        public UserDTO(string name, string surname, string email, string appId, ApplicationRole appRole)
        {
            Name = name;
            Surname = surname;
            AppId = appId;
            Email = email;
            AppRole = appRole;
            IsBanned = false;
        }

        public UserDTO(User input)
        {
            Id = input.Id;
            Name = input.Name;
            Surname = input.Surname;
            AppId = input.AppId;
            Email = input.Email;
            AppRole = input.AppRole;
            IsBanned = input.IsBanned;
            IsAdmin = input.IsAdmin;
        }
    }
}