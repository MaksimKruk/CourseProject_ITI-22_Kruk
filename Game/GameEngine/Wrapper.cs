using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Wrapper<T>
    {
        public T field { get; set; }
        public Sprite sprite;

        public Wrapper(Sprite sprite, T obj)
        {
            this.sprite = sprite;
            this.field = obj;
        }
    }
}
