using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSV_RealEstate
{
    class Program
    {
        static void Main(string[] args)
        {
            List<RealEstateData> realEstateDataList = new List<RealEstateData>();
            //read in the realestatedata.csv file.  As you process each row, you'll add a new 
            // RealEstateData object to the list for each row of the document, excluding the first.

            StreamReader sr = new StreamReader("../../realestatedata.csv");
            string data = sr.ReadLine();
            data = sr.ReadLine(); //Skip the first line of data.

            while(data != null)
            {
                realEstateDataList.Add(new RealEstateData(data));
                data = sr.ReadLine();
            }

            //Display the average square footage of a Condo sold in the city of Sacramento, 
            // round to 2 decimal points
            Console.WriteLine("Average Sq. Ft. of condos sold in Sacramento: {0}\n", realEstateDataList
                .Where(x => x.Type == RealEstateData.RealEstateType.Condo)  //Condos only
                .Where(y => y.City.ToLower() == "sacramento")               //Sacramento only
                .Average(z => z.Sq__ft)                                     //Sq. Ft. Averaged
                .ToString("N2"));                                           //Rounded to 2 decimal points
            
            //Display the total sales of all residential homes in Elk Grove, display in dollars
            Console.WriteLine("Total sales of all residential homes in Elk Grove: {0}\n", realEstateDataList
                .Where(x => x.Type == RealEstateData.RealEstateType.Residential)    //Residental only
                .Where(y => y.City.ToLower() == "elk grove")                        //Elk Grove only
                .Sum(z => z.Price)                                                  //Sum of all prices
                .ToString("C"));                                                    //Currency format
            
            //Display the total number of residential homes sold in the following  
            // zip codes: 95842, 95825, 95815
            Console.WriteLine("Total number of residential homes in zip codes 95842, 95825, and 95815: {0}\n", realEstateDataList
                .Where(x => x.Zip == "95842" || x.Zip == "95825" || x.Zip == "95815")   //Specified zip codes only
                .Count(y => y.Type == RealEstateData.RealEstateType.Residential));      //Count of Residential

            //Display the average sale price of a lot in Sacramento, display in dollars
            Console.WriteLine("Average sale price of a lot in Sacramento: {0}\n", realEstateDataList
                .Where(x => x.Type == RealEstateData.RealEstateType.Lot)    //Lot only
                .Where(y => y.City.ToLower() == "sacramento")               //Sacramento only
                .Average(z => z.Price)                                      //Price average
                .ToString("C"));                                            //Currency format

            //Display the average price per square foot for a condo in Sacramento, display in dollars
            Console.WriteLine("Average price per squre foot for a condo in Sacramento: {0}\n", realEstateDataList
                .Where(x => x.Type == RealEstateData.RealEstateType.Condo)  //Condo only
                .Where(y => y.City.ToLower() == "sacramento")               //Sacramento only
                .Average(z => z.Price / z.Sq__ft)                           //Average price / average Sq. Ft.
                .ToString("C"));                                            //Currency format                         

            //Display the number of all sales that were completed on a Wednesday
            Console.WriteLine("Total sales completed on Wednesdays: {0}\n", realEstateDataList
                .Count(x => x.Sale_date.DayOfWeek == DayOfWeek.Wednesday)); //Count of Wednesdays.

            //Display the average number of bedrooms for a residential home in Sacramento when the 
            // price is greater than 300000, round to 2 decimal points
            Console.WriteLine("Average bedroom count of residental homes worth more than $300,000: {0}", realEstateDataList
                .Where(x => x.Price > 300000)                                       //Price over 300000 only
                .Where(y => y.Type == RealEstateData.RealEstateType.Residential)    //Residential only
                .Average(z => z.Beds)                                               //Average beds
                .ToString("N2"));                                                   //Rounded to 2 decimals.
            
            //Extra Credit:
            //Display top 5 cities and the number of homes sold (using the GroupBy extension)

            //IEnumerable<IGrouping<bool, string>> groups = realEstateDataList.GroupBy(x => (x.City == "SACRAMENTO"));

            Console.WriteLine("Top 5 cities with most homes sold: {0}", realEstateDataList
                .GroupBy(x => x.City));

            Console.ReadKey();
        }
    }

    public enum RealEstateType
    {
        //fill in with enum types: Residential, MultiFamily, Condo, Lot
    }

    public class RealEstateData
    {
        //Create properties, using the correct data types (not all are strings) for all columns of the CSV
        public enum RealEstateType { Residential = 1, Condo, Multi_Family, Unkown, Lot }

        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public int Sq__ft { get; set; }
        public RealEstateType Type { get; set; }
        public DateTime Sale_date { get; set; }
        public decimal Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        //The constructor will take a single string arguement.  This string will be one line of the real estate data.
        // Inside the constructor, you will seperate the values into their corrosponding properties, and do the necessary conversions

        public RealEstateData(string data)
        {
            List<string> dataList = data.Split(',').ToList();

            this.Street = dataList[0];
            this.City = dataList[1];
            this.Zip = dataList[2];
            this.State = dataList[3];
            this.Beds = int.Parse(dataList[4]);
            this.Baths = int.Parse(dataList[5]);
            this.Sq__ft = int.Parse(dataList[6]);
            if (this.Sq__ft == 0)
                this.Type = RealEstateType.Lot;
            else
            {
                if(dataList[7] == "Multi-Family")
                    dataList[7] = dataList[7].Replace('-', '_');
                this.Type = (RealEstateType)Enum.Parse(typeof(RealEstateType), dataList[7]);
            }
            this.Sale_date = DateTime.Parse(dataList[8]);
            this.Price = decimal.Parse(dataList[9]);
            this.Latitude = double.Parse(dataList[10]);
            this.Longitude = double.Parse(dataList[11]);
        }
        //When computing the RealEstateType, if the square footage is 0, then it is of the Lot type, otherwise, use the string
        // value of the "Type" column to determine its corresponding enumeration type.
    }
}
