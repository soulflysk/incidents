using DOTNETCORE_DEV.Dtos;
using DOTNETCORE_DEV.Models;

namespace DOTNETWEBAPI_DEV.Mappers
{
    public static class IncidentsMapper
    {
        public static Incident ToEmployeeFromCreateDto(this UpdateEmployeeDto employeeModel){
            return new Incident{
                EmployeeId = employeeModel.EmployeeId
            };
        }}}