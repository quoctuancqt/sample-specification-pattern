namespace src.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using src.Domain;
    using src.Dtos;
    using src.Specifications;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OrdersController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var orderSpec = new OrderWithDetailsSpecification(name);

            return Ok(await _unitOfWork.OrderRepository
                .FindAllWithSpecification(orderSpec)
                .Select(x => new OrderDto
                {
                    Name = x.Name,
                    OrderDetails = x.OrderDetails.Select(y => new OrderDetailDto
                    {
                        Name = y.Name
                    })
                })
                .ToListAsync()); ;
        }

        [HttpPost("")]
        public async Task<string> Create([FromBody] OrderDto dto)
        {
            var order = new Order(dto.Name);

            _unitOfWork.OrderRepository.Add(order);

            _unitOfWork
                .OrderDetailRepository
                .AddRange(dto.OrderDetails
                .Select(x => new OrderDetail(order.Id, x.Name)));

            await _unitOfWork.CommitAsync();


            return "Ok";
        }

        [HttpPut("{id}")]
        public async Task<string> Update(int id, [FromBody] OrderDto dto)
        {
            var order = await CheckOrderAsync(id);

            if (order == null) return "Not Found";

            order.Name = dto.Name;

            _unitOfWork.OrderRepository.Update(order);

            await _unitOfWork.CommitAsync();

            return "Ok";
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            var order = await CheckOrderAsync(id);

            if (order == null) return "Not Found";

            if (order.OrderDetails.Any())
                _unitOfWork.OrderDetailRepository.RemoveRange(order.OrderDetails);

            _unitOfWork.OrderRepository.Remove(order);

            await _unitOfWork.CommitAsync();

            return "Ok";
        }


        [HttpPost("{orderId}/detail")]
        public async Task<string> CreateOrderDetail(int orderId, [FromBody] OrderDetail dto)
        {
            if (await CheckOrderAsync(orderId) == null) return "Not Found";

            var orderDetail = new OrderDetail(orderId, dto.Name);

            _unitOfWork.OrderDetailRepository.Add(orderDetail);

            await _unitOfWork.CommitAsync();

            return "Ok";
        }

        [HttpPut("{orderId}/detail/{id}")]
        public async Task<string> UpdateOrderDetail(int orderId, int id, [FromBody] OrderDetail dto)
        {
            if (await CheckOrderAsync(orderId) == null) return "Not Found";

            var orderDetail = await _unitOfWork.OrderDetailRepository.FindByAsync(id);

            orderDetail.Name = dto.Name;

            _unitOfWork.OrderDetailRepository.Update(orderDetail);

            await _unitOfWork.CommitAsync();

            return "Ok";
        }

        [HttpDelete("{orderId}/detail/{id}")]
        public async Task<string> DeleteOrderDetail(int orderId, int id)
        {
            if (await CheckOrderAsync(orderId) == null) return "Not Found";

            var orderDetail = await _unitOfWork.OrderDetailRepository.FindByAsync(id);

            if (orderDetail == null) return "Not Found";

            _unitOfWork.OrderDetailRepository.Remove(orderDetail);

            await _unitOfWork.CommitAsync();

            return "Ok";
        }

        private async Task<Order> CheckOrderAsync(int id)
        {
            return await _unitOfWork
                .OrderRepository
                .FindAll(x => x.Id == id)
                .Include(x => x.OrderDetails)
                .SingleOrDefaultAsync();
        }
    }
}
