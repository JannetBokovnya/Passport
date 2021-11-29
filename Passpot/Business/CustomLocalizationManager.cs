using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Passpot.Business
{
    public class CustomLocalizationManager: LocalizationManager
    {
        public override string GetStringOverride( string key )
 {
     switch( key )
     {
         case "GridViewGroupPanelText":
             return "Zum gruppieren ziehen Sie den Spaltenkopf in diesen Bereich.";
         //---------------------- RadGridView Filter Dropdown items texts:
         case "GridViewClearFilter":
             return "Filter lö";
         case "GridViewFilterShowRowsWithValueThat":
             return "Anzeigen der Werte mit Bedingung:";
         case "GridViewFilterSelectAll":
             return "Alles anzeigen";
         case "GridViewFilterContains":
             return "Enthä";
         case "GridViewFilterEndsWith":
             return "Endet mit";
         case "GridViewFilterIsContainedIn":
             return "Enthalten in";
         case "GridViewFilterIsEqualTo":
             return "Gleich";
         case "GridViewFilterIsGreaterThan":
             return "Gröals ";
         case "GridViewFilterIsGreaterThanOrEqualTo":
             return "Gröoder gleich";
         case "GridViewFilterIsLessThan":
             return "Kleiner als";
         case "GridViewFilterIsLessThanOrEqualTo":
             return "Kleiner oder gleich";
         case "GridViewFilterIsNotEqualTo":
             return "Ungleich";
         case "GridViewFilterStartsWith":
             return "Beginnt mit";
         case "GridViewFilterAnd":
             return "Und";
         case "GridViewFilter":
             return "Filter";
     }
     return base.GetStringOverride( key );
    }
    }
}
