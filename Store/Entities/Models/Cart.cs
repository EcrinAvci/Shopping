namespace Entities.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }
        public Cart()
        {
            Lines = new List<CartLine>();
        }

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line= Lines.Where(I => I.Product.ProductId.Equals(product)).FirstOrDefault();

            if(line is null)
            {
                Lines.Add(new CartLine()
                {
                    Product =product,
                    Quantity= quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }


        public virtual void RemoveLine(Product product)=>
        Lines.RemoveAll(I => I.Product.ProductId.Equals(product.ProductId));

        public decimal ComputeTotalValue()=> 
        Lines.Sum(e => e.Product.Price*e.Quantity);

        public virtual void clear() => Lines.Clear();
    }
}