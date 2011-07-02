using System;
using System.Collections.Generic;
using Orchard.UI.Navigation;
using oforms.Models;

namespace oforms.ViewModels {
    public class FormResultViewModel {
        
        public List<OFormResultRecord> Results { get; set; }

        public dynamic Pager { get; set; }

        public string FormName { get; set; }
    }
}