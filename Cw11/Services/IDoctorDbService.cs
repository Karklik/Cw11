using Cw11.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cw11.Services
{
    public interface IDoctorDbService
    {
        public Task<IEnumerable<Doctor>> GetDoctors();
        public Task<Doctor> GetDoctor(int id);
        public Task<Doctor> AddDoctor(Doctor doctor);
        public Task<Doctor> UpdateDoctor(Doctor doctor);
        public Task<Doctor> RemoveDoctor(int id);
    }
}
