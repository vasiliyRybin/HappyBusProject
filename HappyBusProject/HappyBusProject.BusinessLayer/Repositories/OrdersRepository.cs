using AutoMapper;
using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels;
using HappyBusProject.HappyBusProject.DataLayer.InputValidators;
using HappyBusProject.HappyBusProject.DataLayer.Methods;
using HappyBusProject.HappyBusProject.DataLayer.ViewModels;
using HappyBusProject.HappyBusProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.BusinessLayer.Repositories
{
    public class OrdersRepository : IOrderRepository<OrderViewModel>
    {
        private readonly MyShuttleBusAppNewDBContext _repository;
        private readonly IMapper _mapper;

        public OrdersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext, IMapper mapper)
        {
            _repository = myShuttleBusAppNewDBContext ?? throw new ArgumentNullException(nameof(myShuttleBusAppNewDBContext));
            _mapper = mapper;
        }

        public async Task<OrderViewModel[]> GetAllAsync()
        {
            var orders = await _repository.Orders.ToListAsync();

            if (orders.Count != 0)
            {
                var result = new OrderViewModel[orders.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = _mapper.Map<OrderViewModel>(orders[i]);
                    result[i].StartPoint = OrderMethods.GetPointName(_repository, orders[i].StartPointId);
                    result[i].EndPoint = OrderMethods.GetPointName(_repository, orders[i].EndPointId);
                }

                return result;
            }

            return null;
        }


        /// <summary>
        /// Get last order by input name
        /// </summary>
        /// <param name="Full Name"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> GetByNameAsync(string FullName)
        {
            var customer = await _repository.Users.FirstOrDefaultAsync(u => u.FullName == FullName);

            if (customer != null)
            {
                var order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).FirstOrDefault(c => c.CustomerId == customer.Id);
                if (order != null)
                {
                    var result = _mapper.Map<OrderViewModel>(order);
                    result.StartPoint = OrderMethods.GetPointName(_repository, order.StartPointId);
                    result.EndPoint = OrderMethods.GetPointName(_repository, order.EndPointId);
                    return result;
                }
            }

            return null;
        }

        public async Task<OrderViewModel> CreateOrder(OrderInputModel orderInput)
        {
            var check = OrderInputValidation.OrderValuesValidation(_repository, orderInput, out string errorMessage);

            if (check)
            {
                OrderMethods.RetrieveDataForCreatingOrder(_repository, orderInput, out Guid carIDReadyToOrder, out Guid whoOrdered, out int availableSeatsNum);
                if (availableSeatsNum < orderInput.OrderSeatsNum) return null;
                var startPointKM = OrderMethods.GetLengthKM(_repository, orderInput.StartPoint);
                var endPointKM = OrderMethods.GetLengthKM(_repository, orderInput.EndPoint);

                try
                {
                    var order = _mapper.Map<Order>(orderInput);
                    double totalPrice = OrderMethods.CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);

                    OrderMethods.AssignValuesToOrder(order, _repository, carIDReadyToOrder, whoOrdered, orderInput, totalPrice);

                    var view = _mapper.Map<OrderViewModel>(order);
                    _mapper.Map(orderInput, view);


                    _repository.CarCurrentStates.First(c => c.Id == carIDReadyToOrder).FreeSeatsNum -= orderInput.OrderSeatsNum;
                    await _repository.Orders.AddAsync(order);
                    _repository.SaveChanges();

                    return null;
                }
                catch (Exception e)
                {
                    LogWriter.ErrorWriterToFile(e.Message + " " + e.InnerException + " " + "POST Method, Orders Repository");
                    return null;
                }
            }
            else return null;
        }


        /// <summary>
        /// This method updates last active user's order
        /// </summary>
        /// <param name="FullName"></param>
        public void UpdateOrder(OrderInputModelPutMethod putModel)
        {
            var user = _repository.Users.FirstOrDefault(u => u.FullName == putModel.FullName);
            var check = OrderInputValidation.OrderValuesValidation(_repository, (OrderInputModel)putModel, out string _);

            if (user != null && check)
            {
                Order order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);
                OrderMethods.PutMethodMyMapper(_repository, putModel, order, user, out bool isPointModified);

                if (isPointModified)
                {
                    var startPointKM = OrderMethods.GetLengthKM(_repository, order.StartPointId);
                    var endPointKM = OrderMethods.GetLengthKM(_repository, order.EndPointId);
                    order.TotalPrice = OrderMethods.CountTotalPrice(startPointKM, endPointKM, order.OrderSeatsNum);
                }

                _repository.SaveChanges();
            }
        }

        /// <summary>
        /// This method deletes(cancels) last active user's order
        /// </summary>
        /// <param name="FullName"></param>
        public void DeleteOrder(string FullName)
        {
            var user = _repository.Users.FirstOrDefault(u => u.FullName == FullName);

            if (user != null)
            {
                Order order = _repository.Orders.OrderByDescending(o => o.OrderDateTime).First(o => o.CustomerId == user.Id && o.IsActual);

                if (order != null)
                {
                    _repository.Remove(order);
                    _repository.SaveChanges();
                }
            }
        }
    }
}
