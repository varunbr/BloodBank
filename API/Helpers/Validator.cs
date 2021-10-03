using System.Linq;
using API.DTOs;

namespace API.Helpers
{
    public static class Validator
    {
        public static bool ValidateBloodData(BloodGroupUpdateDto updateDto)
        {
            var groupList = Util.GetBloodGroupList();
            return updateDto.Groups.All(
                i => groupList.Contains(i.Group)
                     && i.Value >= 0);
        }
    }
}
