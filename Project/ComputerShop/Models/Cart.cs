using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ComputerShop.Models
{
    //корзина
    public class Cart
    {
        private int CartID { get; set; }
        //первый int - ключ, то есть Id - товара, второй int - количество товаров
        private IDictionary<int, int> content;

        public IDictionary<int, int> Content
        {
            get
            {
                return content;
            }
        }

        //конструктор корзины, по умолчанию она пуста
        public Cart()
        {
            content = new Dictionary<int, int>();
        }


        //добавление товара в корзину
        public void AddProduct(int id)
        {
            //если такой товар есть в корзине (поиск по ключу)
            if (Content.Keys.Contains(id))
            {
                //значит, контент увеличивается, то есть количество товара по Id увеличивается, например mouse - 4 (3, 4)
                Content[id]++;
            }
            else
            {
                //в противном случае - создаем новый объект в нашей корзине, то есть добавляем новый товар в размере 1
                Content.Add(id, 1);
            }
        }

        public void RemoveProduct(int id)
        {
            if (Content.Keys.Contains(id))
            {
                if (Content[id] > 1)
                {
                    Content[id]--;
                }
                else
                {
                    Content.Remove(id);
                }
            }
        }

        public bool IsEmpty()
        {
            return content.Count == 0;
        }

        public void Clear()
        {
            Content.Clear();
        }
    }
}