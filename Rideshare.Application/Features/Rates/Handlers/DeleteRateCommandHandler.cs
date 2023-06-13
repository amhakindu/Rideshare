using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Handlers;
public class DeleteRateCommandHandler : IRequestHandler<DeleteRateCommand, BaseResponse<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRateCommandHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<Unit>> Handle(DeleteRateCommand request, CancellationToken cancellationToken)
    {
        var Rate = await _unitOfWork.RateRepository.Get(request.RateId);
        if (Rate == null)
        {
            var error = $"Rate with id={request.RateId} does not found";
            var response = new BaseResponse<Unit>
            {
                Success = false,
                Message = "Rate Deletion Failed",
            };
            response.Errors.Add(error);
            return response;
        }
        var operations = await _unitOfWork.RateRepository.Delete(Rate);

        if (operations > 0)
        {
            return new BaseResponse<Unit>
            {
                Success = true,
                Message = "Rate Deleted Successfully",
            };
        }

        return new BaseResponse<Unit>()
        {
            Success = true,
            Message = "Rate Deleted Successfully",
        };
    }
}