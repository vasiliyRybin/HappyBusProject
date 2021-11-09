using HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.Interfaces
{
    public interface ICarsStateRepository<T> : IBusAppObject<T>
        where T : IActionResult
    {
        public void UpdateState(string FullName);
    }
}
