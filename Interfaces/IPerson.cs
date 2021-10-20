using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Interfaces
{
    interface IPerson<T>
    {
        T GetAll();
        T GetByName(string name);
    }
}
