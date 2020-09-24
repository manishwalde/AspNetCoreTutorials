using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        /// <summary>
        /// Property Shortcut "prop"
        /// </summary>
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
