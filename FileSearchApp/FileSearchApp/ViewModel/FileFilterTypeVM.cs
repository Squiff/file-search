using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FileSearchApp.ViewModel
{
    enum FileFilterTypeVM
    {
        Contains = 1,
        NotContains = 2,
        Regex = 3
    }
}
