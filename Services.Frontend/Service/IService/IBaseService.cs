using Services.Frontend.Models;

namespace Services.Frontend.Service.IService
{
	public interface IBaseService
	{
		Task<ResponseDto?> SendAsync(RequestDto requestDto);
	}
}
