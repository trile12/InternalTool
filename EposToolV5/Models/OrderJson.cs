using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

namespace EposToolV5.Presentation.Models;
public class RestOrder
{
    [JsonProperty("type")]
    public string Type { get; set; } = "order";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("origin")]
    public string Origin { get; set; } = "epos";

    [JsonProperty("shift_id")]
    public string ShiftId { get; set; }

    [JsonProperty("order_no")]
    public string OrderNo { get; set; }

    [JsonProperty("item_total")]
    public decimal ItemTotal { get; set; }

    [JsonProperty("tax_total")]
    public decimal TaxTotal { get; set; }

    [JsonProperty("adjustments_total")]
    public decimal AdjustmentsTotal { get; set; }

    [JsonProperty("grand_total")]
    public decimal GrandTotal { get; set; }

    [JsonProperty("session_token")]
    public string SessionToken { get; set; }

    [JsonProperty("opened_at")]
    public string OpenedAt { get; set; }

    [JsonProperty("closed_at")]
    public string ClosedAt { get; set; }

    [JsonProperty("customer_id")]
    public string CustomerId { get; set; }

    [JsonProperty("notes")]
    public string Notes { get; set; }

    [JsonProperty("register_id")]
    public string RegisterId { get; set; }

    [JsonProperty("table_id")]
    public string TableId { get; set; }

    [JsonProperty("order_reference_id")]
    public string OrderReferenceId { get; set; }

    [JsonProperty("pax")]
    public int Pax { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } = "opened";

    [JsonProperty("external_attributes")]
    public List<JObject> OrderExternalAttributes { get; set; }

    [JsonProperty("line_items")]
    public List<RestOrderLine> LineItems { get; set; }

    [JsonProperty("sales")]
    public List<RestOrderStockSale> StockSales { get; set; }

    [JsonProperty("returns")]
    public List<RestOrderStockReturn> StockReturns { get; set; }

    [JsonProperty("point_earneds")]
    public List<RestOrderPointEarn> PointEarns { get; set; }

    [JsonProperty("point_reduceds")]
    public List<RestOrderPointReduced> PointReduces { get; set; }

    [JsonProperty("payments")]
    public List<RestOrderOperationPayment> OrderOperationPayments { get; set; }

    [JsonProperty("refunds")]
    public List<RestOrderOperationRefund> OrderOperationRefunds { get; set; }

    [JsonProperty("voids")]
    public List<RestOrderOperationVoid> OrderOperationVoids { get; set; }

    [JsonProperty("roundings")]
    public List<RestOrderOperationRounding> OperationRoundings { get; set; }

    [JsonProperty("adjustments")]
    public List<RestOrderAdjustment> OrderAdjustments { get; set; }

    [JsonProperty("taxes")]
    public List<RestOrderTax> OrderTaxes { get; set; }
}
public class RestOrderLine
{
    [JsonProperty("type")]
    public string Type { get; set; } = "line_item";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("catalog_source_id")]
    public string CatalogSourceId { get; set; }

    [JsonProperty("catalog_category_id")]
    public string CatalogCategoryId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("unit_price")]
    public decimal UnitPrice { get; set; }

    [JsonProperty("tax")]
    public decimal TaxAmount { get; set; }

    [JsonProperty("taxes")]
    public List<RestOrderLineTax> Taxes { get; set; }

    [JsonProperty("sub_total")]
    public decimal SubTotal { get; set; }

    [JsonProperty("net_price")]
    public decimal NetPrice { get; set; }

    [JsonProperty("adjustment")]
    public decimal TotalAdjustment { get; set; }

    [JsonProperty("sold_by")]
    public List<string> SoldBy { get; set; }

    [JsonProperty("notes")]
    public string Notes { get; set; }

    [JsonProperty("properties")]
    public List<RestOrderLineProperty> LineItemProperties { get; set; }

    [JsonProperty("sequence")]
    public int Sequence { get; set; }
}
public class RestOrderLineTax
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("source_id")]
    public string SourceId { get; set; }

    [JsonProperty("source_type")]
    public string SourceType { get; set; } = "Catalog::TaxRule";

    [JsonProperty("percentage")]
    public decimal Percentage { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("priority")]
    public int Priority { get; set; }

    [JsonProperty("tax_inclusive")]
    public bool TaxInclusive { get; set; }
}
public class RestOrderLineProperty
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("source_id")]
    public string SourceId { get; set; }

    [JsonProperty("base_price")]
    public decimal BasePrice { get; set; }

    [JsonProperty("unit_price")]
    public decimal UnitPrice { get; set; }

    [JsonProperty("source_type")]
    public string SourceType { get; set; }//SetMenu::SeparatorItem or //ProductAddon::Addon

    [JsonProperty("properties")]
    public dynamic Properties { get; set; } //"properties": [], using for add-on inside set-menu

    [JsonProperty("additional_price")]
    public decimal AdditionalPrice { get; set; }

    [JsonProperty("additional_total")]
    public decimal AdditionalTotal { get; set; }
}
public class RestOrderStockSale
{
    [JsonProperty("type")]
    public string Type { get; set; } = "sale";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("stock_id")]
    public string StockId { get; set; }

    [JsonProperty("line_item_id")]
    public string LineItemId { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }
}
public class RestOrderStockReturn
{
    [JsonProperty("type")]
    public string Type { get; set; } = "return";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("stock_id")]
    public string StockId { get; set; }

    [JsonProperty("line_item_id")]
    public string LineItemId { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }
}
public class RestOrderPointEarn
{
    [JsonProperty("type")]
    public string Type { get; set; } = "point_earned";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("points")]
    public decimal Points { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }

    [JsonProperty("tier_id")]
    public string TierId { get; set; }
}
public class RestOrderPointReduced
{
    [JsonProperty("type")]
    public string Type { get; set; } = "point_reduced";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("points")]
    public decimal Points { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }

    [JsonProperty("tier_id")]
    public string TierId { get; set; }
}
public class RestOrderOperationPayment : RestOrderOperation
{
    [JsonProperty("type")]
    public string Type { get; set; } = "payment";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("tendered")]
    public decimal Tendered { get; set; }

    [JsonProperty("change")]
    public decimal Change { get; set; }

    [JsonProperty("rounding")]
    public decimal Rounding { get; set; }

    [JsonProperty("external_attributes")]
    public string ExternalAttributes { get; set; }
}
public class RestOrderOperation
{
    [JsonProperty("payment_option_id")]
    public string PaymentOptionId { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }

}
public class RestOrderOperationRefund : RestOrderOperation
{
    [JsonProperty("type")]
    public string Type { get; set; } = "refund";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("rounding")]
    public decimal Rounding { get; set; }

    [JsonProperty("external_attributes")]
    public string ExternalAttributes { get; set; }
}
public class RestOrderOperationVoid
{
    [JsonProperty("type")]
    public string Type { get; set; } = "void";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("transacted_at")]
    public string TransactedAt { get; set; }
}
public class RestOrderOperationRounding
{
    [JsonProperty("type")]
    public string Type { get; set; } = "rounding";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("source_type")]
    public string SourceType { get; set; }//"Operations::Payment", "Operations::Refund"

    [JsonProperty("source_id")]
    public string SourceId { get; set; }//Operation payment id
}
public class RestOrderAdjustment
{
    [JsonProperty("type")]
    public string Type { get; set; } = "adjustment";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("source_id")]
    public string SourceId { get; set; }

    [JsonProperty("source_type")]
    public string SourceType { get; set; }

    [JsonProperty("external_attributes")]
    public List<RestAdjustmentExternalAttributes> ExternalAttributes { get; set; }
}

public class RestAdjustmentExternalAttributes
{
    [JsonProperty("line_item_id")]
    public string LineItemId { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }
}
public class RestOrderTax
{
    [JsonProperty("type")]
    public string Type { get; set; } = "tax";

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("source_id")]
    public string SourceId { get; set; }

    [JsonProperty("source_type")]
    public string SourceType { get; set; } = "Catalog::TaxRule";

    [JsonProperty("percentage")]
    public decimal Percentage { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("priority")]
    public int Priority { get; set; }

    [JsonProperty("tax_inclusive")]
    public bool TaxInclusive { get; set; }
}

