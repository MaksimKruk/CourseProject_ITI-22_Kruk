using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class RunnerWrapper<T>
    {
        public T field { get; set; }
        public Sprite sprite;

        public RunnerWrapper(Sprite sprite, T obj)
        {
            this.sprite = sprite;
            this.field = obj;
        }
    }
}
