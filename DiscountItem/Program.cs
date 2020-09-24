using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/* Don't change anything here.
 * Do not add any other imports. You need to write
 * this program using only these libraries 
 */

namespace ProgramNamespace
{   
    class DiscountProduct
    {
        public string ProductName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    } 

    class BuyersData
    {
        public string CustomerName;
        public string StoreLocation;
        public int DayOfMonth;
        public string ProductName;
        public decimal Price;
        public string PaymentType;

        public static List<BuyersData> GetConvertedData(IEnumerable<string> lines)
        {
            List<BuyersData> Data = new List<BuyersData>();

            foreach(string line in lines)
            {
                string[] values = line.Split(',');
                BuyersData buyerValues = new BuyersData();
                buyerValues.CustomerName = values[0].Trim();
                buyerValues.StoreLocation = values[1].Trim();
                buyerValues.DayOfMonth = Convert.ToInt16(values[2].Trim());
                buyerValues.ProductName = values[3].Trim();
                buyerValues.Price = Convert.ToDecimal(values[4].Replace("Rs", "").Trim());
                buyerValues.PaymentType = values[5].Trim();
                
                Data.Add(buyerValues);
            }
            return Data;
        }
    }

    public class Program
    {
        public static List<String> processData(IEnumerable<string> lines)
        {
            List<String> retVal = new List<String>();

            List<DiscountProduct> discountProducts = new List<DiscountProduct>();

            List<BuyersData> ConvertedData = BuyersData.GetConvertedData(lines);

            IEnumerable<string> DistinctProducts = ConvertedData.Select(x => x.ProductName).Distinct();

            var q1 = from b in ConvertedData select b;
            var q2 = (from b in ConvertedData select b).ToArray();

            foreach(string product in DistinctProducts)
            {
                var minPrice = ConvertedData.Where(x => x.ProductName == product).Min(p => p.Price);
                var maxPrice = ConvertedData.Where(x => x.ProductName == product).Max(p => p.Price);

                if(minPrice != maxPrice)
                {
                    DiscountProduct discountProduct = new DiscountProduct();
                    discountProduct.ProductName = product;
                    discountProduct.MinPrice = minPrice;
                    discountProduct.MaxPrice = maxPrice;

                    discountProducts.Add(discountProduct);
                }
            }
            
            foreach(DiscountProduct discountProduct in discountProducts)
            {
                var customerName = ConvertedData.FirstOrDefault(x => 
                        x.Price == discountProduct.MinPrice && x.ProductName == discountProduct.ProductName)
                        .CustomerName;
                
                var sometimesBroughtOnHigherPrice = false;

                foreach(DiscountProduct innerLoop in discountProducts)
                {
                    sometimesBroughtOnHigherPrice = ConvertedData.Any(x => 
                                                        x.CustomerName == customerName 
                                                        && x.ProductName == innerLoop.ProductName 
                                                        && x.Price == innerLoop.MaxPrice);

                    if(sometimesBroughtOnHigherPrice)
                        break;
                }

                if(!sometimesBroughtOnHigherPrice)
                    retVal.Add(customerName);
            }

            return retVal;
        }

        static void Main(string[] args)
        {
            try
            {
                String line;
                var inputLines = new List<String>();
                while((line = Console.ReadLine()) != null) {
                  line = line.Trim();
                  if (line != "")
                    inputLines.Add(line);
                  else
                    break;
                }
                
                var retVal = processData(inputLines);

                foreach(var res in retVal)
                  Console.WriteLine(res);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}
