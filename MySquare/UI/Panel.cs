﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI
{
    interface IView
    {
    }

    interface IViewWithTabs
    {
        event EventHandler TabChanged;
    }


}
