using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Action_Function_Predicate_Delegates
{
    class Program
    {
        static void Main(string[] args)
        {

            // Delegeler bir cok yerde kullanılan yapılar.  event base programming, asenkron programming, thread programming vs.
            // Delegeler metodlarla aynı duzene sahiptirler.  Aynı tipte parametrelere ve dönüş değerlerine sahip olmalıdırlar. 
            // Action, Func ve Predicate gibi yapılar ile de son zamanlarda karsı karsıya kalıyoruz. 
            //.Net framework içerisine bakıldıgında Action, Func gibi yapıların bizler için onceden tanımlanmıs delegateler olduklarını goruruz. 


            //var list1 = BookManager.BookSearch(new Book() { ISBN = "1" });
            //foreach (var item in list1)
            //{
            //    Console.WriteLine("List 1 : {0} ", item.Author);
            //}


            var list1 = BookManager.BookSearch(new Book() {  Title="asda", Author="x" });
            foreach (var item in list1)
            {
                Console.WriteLine("List 1 : {0} ", item.Author);
            }

            Console.ReadKey();

        }


        #region Normal Delege

        //1- Normal Delege Yontemi =>
        delegate void NormalDelege(long sayi);

        private void NormalDelegeMethod()
        {
            NormalDelege mydelg = DelegeKaresiniAl;
            mydelg(3);

            //or 
            // Associate method1
            //mydelg += DelegeKaresiniAl;
            // Associate Method2
            //mydelg += DelegeKaresiniAl;
            // Invoke the Method1 and Method2 sequentially
            // mydelg.Invoke(5);
        }
        #endregion

        #region Action Sample
        //2- Action Delegesi Yontemi =>         
        // Action ın oluşturulma yapısına gore uygun methodlarda ekstradan delege tanımlamaksızın kullanılabilir. 

        private void DelegeAction()
        {
            Action<long> mydelg = ActionKaresiniAl;
            mydelg(3);
        }
        // Action ile kullanabilmem için void olan ve içerisine T tipinde herhangi bir parametre alan methodları gostermem yeterli .
        private void ActionSample(List<int> sayilar)
        {
            sayilar.ForEach(x => Console.WriteLine(Math.Pow(x, 2).ToString()));
        }

        #endregion

        #region Func Sample
        //3- Func Delegesi Yontemi => 
        //Action geri deger donmez ama Func geriye deger donebilen bir yapıya sahip.  2 kullanımını goruyoruz. 
        //A- parametre almadan bir değer dönebilir.
        //B- parametre alarak bir değer dönebilir. 


        private void DelegeFunc()
        {
            Func<string> stringDonenFunc = MerhabaDe;
            Console.WriteLine(stringDonenFunc);

            //Bir parametre alıp bir parametre donen ornek
            Func<long, string> returnerDelg = FuncKaresiniAl;
            var sonucum = returnerDelg(3);
        }
        #endregion

        #region Predicate Sample

        //4- Predicate Delege Yontemi=> 
        // herhangi bir tipteki değişkeni alır ve geriye boolean bir deger donmek zorundadır. 

        private void PredicateSample()
        {
            Predicate<long> predicateDelegesi = PredicateIkiHaneliMi;
            var sonuc = predicateDelegesi(4);
            if (sonuc)
            {

            }
        }
        #endregion

        #region MethodsForDelegates

        //Delegeler için Ornek methodlar 
        private string MerhabaDe()
        {
            return "Merhaba";
        }

        private void DelegeKaresiniAl(long sayi)
        {
            Console.WriteLine(Math.Pow(sayi, 2).ToString());
        }

        private void ActionKaresiniAl(long sayi)
        {
            Console.WriteLine(Math.Pow(sayi, 2).ToString());
        }

        private string FuncKaresiniAl(long sayi)
        {
            return Math.Pow(sayi, 2).ToString();
        }

        private bool PredicateIkiHaneliMi(long sayi)
        {
            return sayi < 100 && sayi > 9;
        }
        #endregion

        #region PredicateBuilder Sample

        public class Book
        {
            public string ISBN { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
        }

        //5- Birden fazla Predicatei and ve or kullanarak birbirlerine eklemek için kullanılır. 
        //PredicateBuilder classına bakmak gerekiyor. 
        public static class BookManager
        {
            public static IQueryable<Book> BookList()
            {
                return new List<Book>() {
                    new Book(){ ISBN= "1", Author="x", Title="asda"  },
                    new Book(){ ISBN= "2", Author="d", Title="fgdfgd"},
                    new Book(){ ISBN= "3", Author="f", Title="fdhfguhjhk"},
                    new Book(){ ISBN= "4", Author="g", Title="aswrwtre"},
                    new Book(){ ISBN= "5", Author="h", Title="jkşljkl" }
                }.AsQueryable<Book>();
            }

            public static List<Book> BookSearch(Book searchedBook)
            {
                var bookPredicate = PredicateBuilder.True<Book>();

                if (!String.IsNullOrEmpty(searchedBook.Author))
                {
                    bookPredicate = bookPredicate.And(e => e.Author.Contains(searchedBook.Author));
                }
                if (!String.IsNullOrEmpty(searchedBook.Title))
                {
                    bookPredicate = bookPredicate.And(e => e.Title.Contains(searchedBook.Title));
                }
                if (searchedBook.ISBN != null)
                {
                    bookPredicate = bookPredicate.And(e => e.ISBN == searchedBook.ISBN);
                }

                //ISBN esitligi, title veya yazar adında girilen degerlerin iceriliyor olmasu
                var aramaSonucu = BookManager.BookList().Where(bookPredicate);
                return aramaSonucu.ToList<Book>();
            }
        }       

        #endregion


    }
}
