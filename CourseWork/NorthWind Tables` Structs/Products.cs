namespace CourseWork.NorthWind_Tables__Structs
{
    public class Products
    {
        public Products() { }

        public int product_id;
        public string product_name;
        public int supplier_id;
        public int category_id;
        public string quantity_per_unit;
        public decimal unit_price;
        public int units_in_stock;
        public int units_on_order;
        public int reorder_level;
        public int discontinued;
    }
}
