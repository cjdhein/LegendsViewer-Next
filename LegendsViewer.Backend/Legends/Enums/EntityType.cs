using System.ComponentModel;

namespace LegendsViewer.Backend.Legends.Enums;

public enum EntityType // legends_plus.xml
{
    Unknown,
    Civilization,
    [Description("Nomadic Group")]
    NomadicGroup,
    [Description("Migrating Group")]
    MigratingGroup,
    [Description("Collection of Outcasts")]
    Outcast,
    [Description("Religious Group")]
    Religion,
    [Description("Site Government")]
    SiteGovernment,
    [Description("Performance Troupe")]
    PerformanceTroupe,
    [Description("Mercenary Company")]
    MercenaryCompany,
    Guild,
    [Description("Mercenary Order")]
    MilitaryUnit,
    [Description("Merchant Company")]
    MerchantCompany,
}