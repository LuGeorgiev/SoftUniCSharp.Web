using System.Collections.Generic;

namespace BasicExam04112018.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<ProductViewModel> Pending { get; set; }

        public IEnumerable<ProductViewModel> Shipped { get; set; }

        public IEnumerable<ProductViewModel> Delivered { get; set; }
    }
}
